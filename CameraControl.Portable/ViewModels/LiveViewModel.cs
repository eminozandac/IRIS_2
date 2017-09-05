using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CameraControl.Portable.Messages;
using CameraControl.Portable.Models;
using CameraControl.Portable.Services.Onvif.Abstract;
using CameraControl.Portable.Services.Onvif.Exceptions;
using CameraControl.Portable.Services.Onvif.Models.Contracts.Media;
using CameraControl.SoapClient.Exceptions;
using MvvmCross.Core.ViewModels;
using MvvmCross.Platform.Platform;
using MvvmCross.Plugins.Messenger;
using CameraControl.Portable.Services.Onvif.Models.Contracts.PTZ;
using CameraControl.Portable.Utils;
using Cheesebaron.MvxPlugins.Settings.Interfaces;

namespace CameraControl.Portable.ViewModels
{
    public class LiveViewModel : BaseViewModel
    {
        #region Fields

        private readonly IOnvifServiceAggregator _onvifServiceAggregator;
        private readonly ISettings _settingsStorage;

        private bool _autoPlay;

        private ICollection<ProfileModel> _profileModels;
        private ProfileModel _selectedProfileModel;

        private MvxAsyncCommand _profileSelectCommand;
        private MvxCommand _showPlaybackCommand;
        private MvxCommand _showQuadCommand;

        #endregion

        #region Constructors

        public LiveViewModel(IOnvifServiceAggregator onvifServiceAggregator, IMvxMessenger messenger, IMvxJsonConverter jsonConverter, ISettings settingsStorage)
            : base(messenger, jsonConverter)
        {
            _onvifServiceAggregator = onvifServiceAggregator;
            _settingsStorage = settingsStorage;

            PTZControlViewModel = new PTZControlViewModel(this, onvifServiceAggregator, messenger, jsonConverter);
            
            MediaPlayerViewModel = new MediaPlayerViewModel(messenger);
        }

        #endregion

        #region Properties

        public PTZControlViewModel PTZControlViewModel { get; }

        public MediaPlayerViewModel MediaPlayerViewModel { get; }

        public ICollection<ProfileModel> ProfileModels
        {
            get { return _profileModels; }
            set
            {
                _profileModels = value;
                RaisePropertyChanged(() => ProfileModels);
            }
        }

        public ProfileModel SelectedProfileModel
        {
            get { return _selectedProfileModel; }
            set
            {
                _selectedProfileModel = value;
                RaisePropertyChanged(() => SelectedProfileModel);
                ShowPlaybackCommand.RaiseCanExecuteChanged();
            }
        }

        #endregion

        #region Commands

        public MvxAsyncCommand ProfileSelectCommand => _profileSelectCommand ?? (_profileSelectCommand = new MvxAsyncCommand(ProfileSelectAsync, CanProfileSelect));

        public MvxCommand ShowPlaybackCommand => _showPlaybackCommand ?? (_showPlaybackCommand = new MvxCommand(ShowPlayback, CanShowPlayback));

        public MvxCommand ShowQuadCommand => _showQuadCommand ?? (_showQuadCommand = new MvxCommand(ShowQuad, CanShowQuad));

        #endregion

        #region Public methods

        public void Init(bool autoPlay = false, string token = null)
        {
            _autoPlay = autoPlay;

            ProfileModels = _settingsStorage
                .GetValue<string>(SettingsKeys.Profiles)
                .FromJson<ICollection<ProfileModel>>(JsonConverter);

            SelectedProfileModel = token != null
                ? ProfileModels.SingleOrDefault(pm => pm.Token == token)
                : ProfileModels.FirstOrDefault();
        }

        public override async void Start()
        {
            base.Start();

            if (_autoPlay)
            {
                await ProfileSelectAsync();
            }
        }

        #endregion

        #region Protected methods

        protected override void OnIsBusyChanged(bool isBusy)
        {
            base.OnIsBusyChanged(isBusy);

            ProfileSelectCommand.RaiseCanExecuteChanged();
        }

        #endregion

        #region Private methods

        private async Task ProfileSelectAsync()
        {
            IsBusy = true;

            try
            {
                var streamUriResponse = await _onvifServiceAggregator.OnvifMediaService.GetStreamUriAsync(new StreamSetup
                {
                    Stream = StreamType.RTPUnicast,
                    Transport = new Transport
                    {
                        Protocol = TransportProtocol.RTSP
                    }
                }, SelectedProfileModel.Token);

                if (streamUriResponse == null)
                {
                    var toast = new ToastMessage(this, "Stream Uri is unavailable.");
                    Messenger.Publish(toast);

                    return;
                }

                var endpointModel = _settingsStorage
                    .GetValue<string>(SettingsKeys.Endpoint)
                    .FromJson<EndpointModel>(JsonConverter);

                var credentialsModel = _settingsStorage
                    .GetValue<string>(SettingsKeys.Credentials)
                    .FromJson<CredentialsModel>(JsonConverter);

                var streamUri = new Uri(streamUriResponse.MediaUri.Uri);
                MediaPlayerViewModel.ConnectionUrl = $"rtsp://{credentialsModel.Username}:{credentialsModel.Password}@{endpointModel.Uri}:{endpointModel.RtspPort}{streamUri.PathAndQuery}";
                if (MediaPlayerViewModel.RestartCommand.CanExecute())
                {
                    MediaPlayerViewModel.RestartCommand.Execute();
                }

                PTZControlViewModel.ProfileToken = SelectedProfileModel.Token;
                PTZControlViewModel.VideoSourceToken = SelectedProfileModel.VideosSourceToken;
            }
            catch (OnvifServiceException e)
            {
                var toast = e.ToToastMessage(this);
                Messenger.Publish(toast);
            }
            catch (FaultException e)
            {
                var toast = e.ToToastMessage(this);
                Messenger.Publish(toast);
            }
            finally
            {
                IsBusy = false;
			}

			// TODO: Check for FixedHomePosition configuration, otherwise log and swallow FaultException
            try
            {
				await _onvifServiceAggregator.OnvifPTZService.SetHomePositionAsync(new SetHomePosition
				{
					ProfileToken = SelectedProfileModel.Token
				});
			}
			catch (OnvifServiceException e)
			{
				var toast = e.ToToastMessage(this);
				Messenger.Publish(toast);
			}
			catch (FaultException)
			{
                // TODO: Log fault
			}
        }

        private bool CanProfileSelect() => !IsBusy;

        private void ShowPlayback()
        {
            if (MediaPlayerViewModel.StopCommand.CanExecute())
            {
                MediaPlayerViewModel.StopCommand.Execute();
            }

            var presentationBundle = new MvxBundle(new Dictionary<string, string> {{"NavigationMode", "ClearTop"}});
            ShowViewModel<PlaybackViewModel>(new {token = _selectedProfileModel.Token}, presentationBundle);
        }

        private bool CanShowPlayback() => SelectedProfileModel != null;

        private void ShowQuad()
        {
            if (MediaPlayerViewModel.StopCommand.CanExecute())
            {
                MediaPlayerViewModel.StopCommand.Execute();
            }

            var presentationBundle = new MvxBundle(new Dictionary<string, string> {{"NavigationMode", "ClearTop"}});
            ShowViewModel<QuadViewModel>(new {token = _selectedProfileModel.Token}, presentationBundle);
        }

        private bool CanShowQuad() => !IsBusy;
        
        #endregion
    }
}
