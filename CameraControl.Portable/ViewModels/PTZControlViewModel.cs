using System;
using System.Threading.Tasks;
using CameraControl.Portable.Models;
using CameraControl.Portable.Services.Onvif.Abstract;
using CameraControl.Portable.Services.Onvif.Exceptions;
using CameraControl.Portable.Services.Onvif.Models.Contracts.Imaging;
using CameraControl.Portable.Services.Onvif.Models.Contracts.PTZ;
using CameraControl.SoapClient.Exceptions;
using MvvmCross.Core.ViewModels;
using MvvmCross.Platform.Platform;
using MvvmCross.Plugins.Messenger;
using Stop = CameraControl.Portable.Services.Onvif.Models.Contracts.PTZ.Stop;
using FocusStop = CameraControl.Portable.Services.Onvif.Models.Contracts.Imaging.Stop;

namespace CameraControl.Portable.ViewModels
{
    public class PTZControlViewModel : BaseViewModel
    {
        #region Fields

        private string _profileToken;
        private string _videoSourceToken;

        private readonly MvxViewModel _parentViewModel;
        private readonly IOnvifServiceAggregator _onvifServiceAggregator;

        private MvxAsyncCommand<ContinuousMoveDirection> _continuousMoveCommand;
        private MvxAsyncCommand _stopCommand;

        private MvxAsyncCommand<FocusMoveDirection> _focusMoveCommand;
        private MvxAsyncCommand _stopFocusCommand;

        private MvxAsyncCommand _autoFocusCommand;
        private MvxAsyncCommand _goToHomeCommand;

        #endregion

        #region Constructors

        public PTZControlViewModel(MvxViewModel parentViewModel, IOnvifServiceAggregator onvifServiceAggregator, IMvxMessenger messenger, IMvxJsonConverter jsonConverter) : base(messenger, jsonConverter)
        {
            if (parentViewModel == null)
            {
                throw new ArgumentNullException(nameof(parentViewModel));
            }

            _onvifServiceAggregator = onvifServiceAggregator;
            _parentViewModel = parentViewModel;
        }

        #endregion

        #region Properties
        
        public string ProfileToken
        {
            get { return _profileToken; }
            set
            {
                if (_profileToken == value)
                {
                    return;
                }

                _profileToken = value;
                RaisePropertyChanged(() => ProfileToken);

                ContinuousMoveCommand.RaiseCanExecuteChanged();
                StopCommand.RaiseCanExecuteChanged();
                GoToHomeCommand.RaiseCanExecuteChanged();
            }
        }

        public string VideoSourceToken
        {
            get { return _videoSourceToken; }
            set
            {
                if (_videoSourceToken == value)
                {
                    return;
                }

                _videoSourceToken = value;
                RaisePropertyChanged(() => VideoSourceToken);

                FocusMoveCommand.RaiseCanExecuteChanged();
                StopFocusCommand.RaiseCanExecuteChanged();
                AutoFocusCommand.RaiseCanExecuteChanged();
            }
        }

        #endregion

        #region Commands

        public MvxAsyncCommand<ContinuousMoveDirection> ContinuousMoveCommand => _continuousMoveCommand
            ?? (_continuousMoveCommand = new MvxAsyncCommand<ContinuousMoveDirection>(ContinuousMoveAsync, CanContinuousMove));

        public MvxAsyncCommand StopCommand => _stopCommand ?? (_stopCommand = new MvxAsyncCommand(StopAsync, CanStop));

        public MvxAsyncCommand<FocusMoveDirection> FocusMoveCommand => _focusMoveCommand
            ?? (_focusMoveCommand = new MvxAsyncCommand<FocusMoveDirection>(FocusMoveAsync, CanFocusMove));

        public MvxAsyncCommand StopFocusCommand => _stopFocusCommand ?? (_stopFocusCommand = new MvxAsyncCommand(StopFocusAsync, CanStopFocus));
        
        public MvxAsyncCommand AutoFocusCommand => _autoFocusCommand ?? (_autoFocusCommand = new MvxAsyncCommand(AutoFocusAsync, CanAutoFocus));

        public MvxAsyncCommand GoToHomeCommand => _goToHomeCommand ?? (_goToHomeCommand = new MvxAsyncCommand(GoToHomeAsync, CanGoToHome));

        #endregion

        #region Protected methods

        protected override void OnIsBusyChanged(bool isBusy)
        {
            base.OnIsBusyChanged(isBusy);

            ContinuousMoveCommand.RaiseCanExecuteChanged();
            FocusMoveCommand.RaiseCanExecuteChanged();
            AutoFocusCommand.RaiseCanExecuteChanged();
            GoToHomeCommand.RaiseCanExecuteChanged();
        }

        #endregion

        #region Private methods

        private async Task ContinuousMoveAsync(ContinuousMoveDirection continuousMoveDirection)
        {
            IsBusy = true;

            var velocity = new PTZSpeed();
            switch (continuousMoveDirection)
            {
                case ContinuousMoveDirection.None:
                    break;
                case ContinuousMoveDirection.Upward:
                    velocity.PanTilt.y = 0.5f;
                    break;
                case ContinuousMoveDirection.Forward:
                    velocity.PanTilt.x = 0.5f;
                    break;
                case ContinuousMoveDirection.Downward:
                    velocity.PanTilt.y = -0.5f;
                    break;
                case ContinuousMoveDirection.Backward:
                    velocity.PanTilt.x = -0.5f;
                    break;
                case ContinuousMoveDirection.ZoomIn:
                    velocity.Zoom.x = 0.5f;
                    break;
                case ContinuousMoveDirection.ZoomOut:
                    velocity.Zoom.x = -0.5f;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(continuousMoveDirection), continuousMoveDirection, null);
            }

            try
            {
                await _onvifServiceAggregator.OnvifPTZService.ContinuousMoveAsync(new ContinuousMove
                {
                    ProfileToken = ProfileToken,
                    Velocity = velocity
                });
            }
            catch (OnvifServiceException e)
            {
                var toast = e.ToToastMessage(_parentViewModel);
                Messenger.Publish(toast);
            }
            catch (FaultException e)
            {
                var toast = e.ToToastMessage(_parentViewModel);
                Messenger.Publish(toast);
            }
            finally
            {
                IsBusy = false;
            }
        }

        private bool CanContinuousMove(ContinuousMoveDirection continuousMoveDirection) => !IsBusy && ProfileToken != null;

        private async Task StopAsync()
        {
            try
            {
                await _onvifServiceAggregator.OnvifPTZService.StopAsync(new Stop
                {
                    ProfileToken = ProfileToken,
                    PanTilt = true,
                    Zoom = false
                });
            }
            catch (OnvifServiceException e)
            {
                var toast = e.ToToastMessage(_parentViewModel);
                Messenger.Publish(toast);
            }
            catch (FaultException e)
            {
                var toast = e.ToToastMessage(_parentViewModel);
                Messenger.Publish(toast);
            }
        }

        private bool CanStop() => ProfileToken != null;

        private async Task FocusMoveAsync(FocusMoveDirection focusMoveDirection)
        {
            IsBusy = true;

            var relativeFocus = new RelativeFocus();
            switch (focusMoveDirection)
            {
                case FocusMoveDirection.None:
                    break;
                case FocusMoveDirection.FocusIn:
                    relativeFocus.Distance = 0.5f;
                    break;
                case FocusMoveDirection.FocusOut:
                    relativeFocus.Distance = -0.5f;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(focusMoveDirection), focusMoveDirection, null);
            }

            try
            {
                await _onvifServiceAggregator.OnvifImagingService.SetImagingSettingsAsync(new SetImagingSettings
                {
                    VideoSourceToken = VideoSourceToken,
                    ImagingSettings = new ImagingSettings20
                    {
                        Focus = new FocusConfiguration20
                        {
                            AutoFocusMode = AutoFocusMode.MANUAL
                        }
                    }
                });

                await _onvifServiceAggregator.OnvifImagingService.MoveAsync(new Move
                {
                    VideoSourceToken = VideoSourceToken,
                    Focus = new FocusMove
                    {
                        Relative = relativeFocus
                    }
                });
            }
            catch (OnvifServiceException e)
            {
                var toast = e.ToToastMessage(_parentViewModel);
                Messenger.Publish(toast);
            }
            catch (FaultException e)
            {
                var toast = e.ToToastMessage(_parentViewModel);
                Messenger.Publish(toast);
            }
            finally
            {
                IsBusy = false;
            }
        }

        private bool CanFocusMove(FocusMoveDirection focusMoveDirection) => !IsBusy && VideoSourceToken != null;

        private async Task StopFocusAsync()
        {
            try
            {
                await _onvifServiceAggregator.OnvifImagingService.StopAsync(new FocusStop
                {
                    VideoSourceToken = VideoSourceToken
                });
            }
            catch (OnvifServiceException e)
            {
                var toast = e.ToToastMessage(_parentViewModel);
                Messenger.Publish(toast);
            }
            catch (FaultException e)
            {
                var toast = e.ToToastMessage(_parentViewModel);
                Messenger.Publish(toast);
            }
        }

        private bool CanStopFocus() => VideoSourceToken != null;

        private async Task AutoFocusAsync()
        {
            IsBusy = true;

            try
            {
                await _onvifServiceAggregator.OnvifImagingService.SetImagingSettingsAsync(new SetImagingSettings
                {
                    VideoSourceToken = VideoSourceToken,
                    ImagingSettings = new ImagingSettings20
                    {
                        Focus = new FocusConfiguration20
                        {
                            AutoFocusMode = AutoFocusMode.AUTO
                        }
                    }
                });
            }
            catch (OnvifServiceException e)
            {
                var toast = e.ToToastMessage(_parentViewModel);
                Messenger.Publish(toast);
            }
            catch (FaultException e)
            {
                var toast = e.ToToastMessage(_parentViewModel);
                Messenger.Publish(toast);
            }
            finally
            {
                IsBusy = false;
            }
        }

        private bool CanAutoFocus() => !IsBusy && VideoSourceToken != null;

        private async Task GoToHomeAsync()
        {
            IsBusy = true;

            try
            {
                await _onvifServiceAggregator.OnvifPTZService.GoToHomePositionAsync(new GotoHomePosition
                {
                    ProfileToken = ProfileToken
                });
            }
            catch (OnvifServiceException e)
            {
                var toast = e.ToToastMessage(_parentViewModel);
                Messenger.Publish(toast);
            }
            catch (FaultException e)
            {
                var toast = e.ToToastMessage(_parentViewModel);
                Messenger.Publish(toast);
            }
            finally
            {
                IsBusy = false;
            }
        }

        private bool CanGoToHome() => !IsBusy && ProfileToken != null;

        #endregion
    }
}
