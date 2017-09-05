using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CameraControl.Portable.Messages;
using CameraControl.Portable.Models;
using CameraControl.Portable.Services.Onvif.Abstract;
using CameraControl.Portable.Services.Onvif.Exceptions;
using MvvmCross.Core.ViewModels;
using MvvmCross.Platform.Platform;
using MvvmCross.Plugins.Messenger;
using CameraControl.Portable.Services.Onvif.Models.Contracts.Replay;
using CameraControl.Portable.Utils;
using CameraControl.SoapClient.Exceptions;
using Cheesebaron.MvxPlugins.Settings.Interfaces;
using CameraControl.Portable.RestAPI;

namespace CameraControl.Portable.ViewModels
{
    public class PlaybackViewModel : BaseViewModel
    {
        #region Fields

        private readonly IOnvifServiceAggregator _onvifServiceAggregator;
        private readonly ISettings _settingsStorage;

        private ProfileModel _profileModel;

        private MvxCommand _showLiveCommand;
        private MvxCommand _showQuadCommand;

        #endregion

        #region Constructors

        public PlaybackViewModel(IOnvifServiceAggregator onvifServiceAggregator, IMvxMessenger messenger, IMvxJsonConverter jsonConverter, ISettings settingsStorage)
            : base(messenger, jsonConverter)
        {
            _onvifServiceAggregator = onvifServiceAggregator;
            _settingsStorage = settingsStorage;

            MediaPlayerViewModel = new MediaPlayerViewModel(messenger, true);
			RestAPIViewModel = new RestAPIViewModel(messenger, jsonConverter);
        }

        #endregion

        #region Properties

        public MediaPlayerViewModel MediaPlayerViewModel { get; }

		public RestAPIViewModel RestAPIViewModel { get; }

        public ProfileModel ProfileModel
        {
            get { return _profileModel; }
            set
            {
                _profileModel = value;
                RaisePropertyChanged(() => ProfileModel);
            }
        }

		#endregion

		#region Commands

		public MvxCommand ShowLiveCommand => _showLiveCommand ?? (_showLiveCommand = new MvxCommand(ShowLive, CanShowLive));

        public MvxCommand ShowQuadCommand => _showQuadCommand ?? (_showQuadCommand = new MvxCommand(ShowQuad, CanShowQuad));

        #endregion

        #region Public methods

        public void Init(string token)
        {
            if (token == null)
            {
                throw new ArgumentNullException(nameof(token));
            }

            var profileModels = _settingsStorage
                .GetValue<string>(SettingsKeys.Profiles)
                .FromJson<ICollection<ProfileModel>>(JsonConverter);

            ProfileModel = profileModels.SingleOrDefault(pm => pm.Token == token);

			RestAPIViewModel.InitRestAPI();
			RestAPIViewModel.CameraName = ProfileModel.Name;
			RestAPIViewModel.SelectedPlaybackStartsEvent += HandleSelectedPlaybackStartsEvent;
        }

        public override async void Start()
        {
            base.Start();

            await PlaybackStart();
        }

        #endregion

        #region Protected methods

        protected override void OnIsBusyChanged(bool isBusy)
        {
            base.OnIsBusyChanged(isBusy);

            ShowLiveCommand.RaiseCanExecuteChanged();
        }

        #endregion

        #region Private methods

        private async Task PlaybackStart()
        {
            IsBusy = true;

            try
            {
                var replayUriResponse = await _onvifServiceAggregator.OnvifReplayService.GetStreamUriAsync(new StreamSetup
                {
                    Stream = StreamType.RTPUnicast,
                    Transport = new Transport
                    {
                        Protocol = TransportProtocol.RTSP
                    }
                }, ProfileModel.Token);

                if (replayUriResponse == null)
                {
                    var toast = new ToastMessage(this, "Replay Uri is unavailable.");
                    Messenger.Publish(toast);

                    return;
                }

                var endpointModel = _settingsStorage
                    .GetValue<string>(SettingsKeys.Endpoint)
                    .FromJson<EndpointModel>(JsonConverter);

                var credentialsModel = _settingsStorage
                    .GetValue<string>(SettingsKeys.Credentials)
                    .FromJson<CredentialsModel>(JsonConverter);

                var replayUri = new Uri(replayUriResponse.Uri);
                MediaPlayerViewModel.ConnectionUrl = $"rtsp://{credentialsModel.Username}:{credentialsModel.Password}@{endpointModel.Uri}:{endpointModel.RtspPort}{replayUri.PathAndQuery}";
                if (MediaPlayerViewModel.RestartCommand.CanExecute())
                {
                    MediaPlayerViewModel.RestartCommand.Execute();
                }
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
        }

		private void HandleSelectedPlaybackStartsEvent(object sender, PlaybackItemEventArgs e)
		{
			MediaPlayerViewModel.SetStartPlayTime(e.StartTime);
		}

		private void ShowLive()
        {
            if (MediaPlayerViewModel.StopCommand.CanExecute())
            {
                MediaPlayerViewModel.StopCommand.Execute();
            }

            var presentationBundle = new MvxBundle(new Dictionary<string, string> {{"NavigationMode", "ClearTop"}});
            ShowViewModel<LiveViewModel>(new {autoPlay = true, token = ProfileModel.Token}, presentationBundle);
        }

        private bool CanShowLive() => !IsBusy;

        private void ShowQuad()
        {
            if (MediaPlayerViewModel.StopCommand.CanExecute())
            {
                MediaPlayerViewModel.StopCommand.Execute();
            }

            var presentationBundle = new MvxBundle(new Dictionary<string, string> {{"NavigationMode", "ClearTop"}});
            ShowViewModel<QuadViewModel>(new {token = ProfileModel.Token}, presentationBundle);
        }

        private bool CanShowQuad() => !IsBusy;

        #endregion
    }
}
