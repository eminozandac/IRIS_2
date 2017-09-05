using System;
using System.Collections.Generic;
using CameraControl.Portable.Messages;
using CameraControl.Portable.Models;
using CameraControl.Portable.Services.Onvif.Abstract;
using CameraControl.Portable.Services.Onvif.Exceptions;
using CameraControl.Portable.Services.Onvif.Models.Contracts.Media;
using CameraControl.Portable.Utils;
using CameraControl.SoapClient.Exceptions;
using Cheesebaron.MvxPlugins.Settings.Interfaces;
using MvvmCross.Core.ViewModels;
using MvvmCross.Platform.Platform;
using MvvmCross.Plugins.Messenger;

namespace CameraControl.Portable.ViewModels
{
    public class QuadCellViewModel : MvxViewModel
    {
        #region Fields

        private readonly QuadViewModel _quadViewModel;
        private readonly IOnvifServiceAggregator _onvifServiceAggregator;
        private readonly IMvxMessenger _messenger;
        private readonly ISettings _settingsStorage;
        private readonly IMvxJsonConverter _jsonConverter;

        private ProfileModel _selectedProfileModel;

        private MvxCommand _showLiveCommand;

        #endregion

        #region Constructors

        public QuadCellViewModel(QuadViewModel quadViewModel, IOnvifServiceAggregator onvifServiceAggregator, IMvxMessenger messenger, ISettings settingsStorage, IMvxJsonConverter jsonConverter)
        {
            _quadViewModel = quadViewModel;
            _onvifServiceAggregator = onvifServiceAggregator;
            _messenger = messenger;
            _settingsStorage = settingsStorage;
            _jsonConverter = jsonConverter;

            MediaPlayerViewModel = new MediaPlayerViewModel(messenger);
        }

        #endregion

        #region Properties

        public MediaPlayerViewModel MediaPlayerViewModel { get; }

        public ProfileModel SelectedProfileModel
        {
            get { return _selectedProfileModel; }
            set
            {
                _selectedProfileModel = value;
                RaisePropertyChanged(() => SelectedProfileModel);
                ShowLiveCommand.RaiseCanExecuteChanged();

                ProfileSelectAsync();
            }
        }

        #endregion

        #region Commands
        
        public MvxCommand ShowLiveCommand => _showLiveCommand ?? (_showLiveCommand = new MvxCommand(ShowLive, CanShowLive));

        #endregion

        #region Private Methods

        private void ShowLive()
        {
            ShowViewModel<LiveViewModel>(new {autoPlay = true, token = SelectedProfileModel.Token});
        }

        private bool CanShowLive() => !_quadViewModel.IsBusy && SelectedProfileModel?.Token != null;

        private async void ProfileSelectAsync()
        {
            if (SelectedProfileModel?.Token == null)
            {
                if (MediaPlayerViewModel.StopCommand.CanExecute())
                {
                    MediaPlayerViewModel.StopCommand.Execute();
                }
                return;
            }

            _quadViewModel.IsBusy = true;

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
                    var toast = new ToastMessage(_quadViewModel, "Stream Uri is unavailable.");
                    _messenger.Publish(toast);

                    return;
                }

                var endpointModel = _settingsStorage
                    .GetValue<string>(SettingsKeys.Endpoint)
                    .FromJson<EndpointModel>(_jsonConverter);

                var credentialsModel = _settingsStorage
                    .GetValue<string>(SettingsKeys.Credentials)
                    .FromJson<CredentialsModel>(_jsonConverter);

                var streamUri = new Uri(streamUriResponse.MediaUri.Uri);
                MediaPlayerViewModel.ConnectionUrl = $"rtsp://{credentialsModel.Username}:{credentialsModel.Password}@{endpointModel.Uri}:{endpointModel.RtspPort}{streamUri.PathAndQuery}";
                if (MediaPlayerViewModel.RestartCommand.CanExecute())
                {
                    MediaPlayerViewModel.RestartCommand.Execute();
                }
            }
            catch (OnvifServiceException e)
            {
                var toast = e.ToToastMessage(_quadViewModel);
                _messenger.Publish(toast);
            }
            catch (FaultException e)
            {
                var toast = e.ToToastMessage(_quadViewModel);
                _messenger.Publish(toast);
            }
            finally
            {
                _quadViewModel.IsBusy = false;
            }
        }

        #endregion
    }
}
