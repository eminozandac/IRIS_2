using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CameraControl.Portable.Messages;
using CameraControl.Portable.Models;
using CameraControl.Portable.Services.Onvif.Abstract;
using CameraControl.Portable.Services.Onvif.Exceptions;
using CameraControl.Portable.Services.Onvif.Models;
using CameraControl.Portable.Utils;
using CameraControl.SoapClient.Exceptions;
using Cheesebaron.MvxPlugins.Settings.Interfaces;
using MvvmCross.Core.ViewModels;
using MvvmCross.Platform.Platform;
using MvvmCross.Plugins.Messenger;
using MvvmValidation;

namespace CameraControl.Portable.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        #region Fields

        private const int DefaultOnvifPort = 580;
        private const int DefaultRtspPort = 554;

        private readonly IOnvifServiceAggregator _onvifServiceAggregator;
        private readonly ISettings _settingsStorage;

        private string _uri;
        private int _onvifPort;
        private int _rtspPort;
        private ICollection<ProfileModel> _profileModels;

        private MvxAsyncCommand _connectCommand;

        #endregion

        #region Constructors

        public MainViewModel(IOnvifServiceAggregator onvifServiceAggregator, IMvxMessenger messenger, IMvxJsonConverter jsonConverter, ISettings settingsStorage)
            : base(messenger, jsonConverter)
        {
            _onvifServiceAggregator = onvifServiceAggregator;
            _settingsStorage = settingsStorage;

            LoginViewModel = new LoginViewModel(Messenger, JsonConverter);
        }

        #endregion

        #region Properties

        public LoginViewModel LoginViewModel { get; }

        public string Uri
        {
            get { return _uri; }
            set
            {
                if (_uri == value)
                {
                    return;
                }

                _uri = value;
                RaisePropertyChanged(() => Uri);
            }
        }

        public int OnvifPort
        {
            get { return _onvifPort; }
            set
            {
                if (_onvifPort == value)
                {
                    return;
                }

                _onvifPort = value;
                RaisePropertyChanged(() => OnvifPort);
            }
        }

        public int RtspPort
        {
            get { return _rtspPort; }
            set
            {
                if (_rtspPort == value)
                {
                    return;
                }

                _rtspPort = value;
                RaisePropertyChanged(() => RtspPort);
            }
        }

        #endregion

        #region Commands

        public MvxAsyncCommand ConnectCommand => _connectCommand ?? (_connectCommand = new MvxAsyncCommand(ConnectAsync, CanConnect));

        #endregion

        #region Public methods

        public void Init()
        {
            RestoreSettings();
        }

        #endregion

        #region Protected methods

        protected override void OnIsBusyChanged(bool isBusy)
        {
            base.OnIsBusyChanged(isBusy);

            ConnectCommand.RaiseCanExecuteChanged();
        }

        protected override void ConfigureValidationRules(ValidationHelper validationHelper)
        {
            validationHelper.AddRule(() => Uri, () => RuleResult.Assert(Uri != null && System.Uri.CheckHostName(Uri) != UriHostNameType.Unknown, "Uri is incorrect"));
            validationHelper.AddRule(() => OnvifPort, () => RuleResult.Assert(OnvifPort > 0 && OnvifPort < 65536, "Onvif Port is incorrect"));
            validationHelper.AddRule(() => RtspPort, () => RuleResult.Assert(RtspPort > 0 && RtspPort < 65536, "Rtsp Port is incorrect"));
        }

        #endregion

        #region Private methods

        private async Task ConnectAsync()
        {
            Validate();
            LoginViewModel.Validate();

            if (!IsValid || !LoginViewModel.IsValid)
            {
                return;
            }

            IsBusy = true;

            if (LoginViewModel.Username != null)
            {
                _onvifServiceAggregator.Credentials = new OnvifCredentials(LoginViewModel.Username, LoginViewModel.Password);
            }
            _onvifServiceAggregator.BaseUri = $"{Uri}:{OnvifPort}";

            try
            {
                var profilesResponse = await _onvifServiceAggregator.OnvifMediaService.GetProfilesAsync();

                if (profilesResponse?.Profiles == null)
                {
                    var toast = new ToastMessage(this, "Media profiles not found.");
                    Messenger.Publish(toast);

                    return;
                }

                _profileModels = profilesResponse.Profiles.Select(p => new ProfileModel(p.token)
                {
                    Name = p.Name,
                    VideosSourceToken = p.VideoSourceConfiguration?.SourceToken
                }).ToList();

                SaveSettings();

                ShowViewModel<LiveViewModel>();
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

        private bool CanConnect() => !IsBusy;

        private void SaveSettings()
        {
            _settingsStorage.AddOrUpdateValue(SettingsKeys.Profiles, _profileModels.ToJson(JsonConverter));

            var endpointModel = new EndpointModel
            {
                Uri = Uri,
                OnvifPort = OnvifPort,
                RtspPort = RtspPort
            };

            _settingsStorage.AddOrUpdateValue(SettingsKeys.Endpoint, endpointModel.ToJson(JsonConverter));

            // TODO: Use secure storage for credentials and uri
            var credentialsModel = new CredentialsModel
            {
                Username = LoginViewModel.Username,
                Password = LoginViewModel.Password,
                SaveCredentials = LoginViewModel.SaveCredentials
            };

            _settingsStorage.AddOrUpdateValue(SettingsKeys.Credentials, credentialsModel.ToJson(JsonConverter));
        }

        private void RestoreSettings()
        {
            var endpointModel = _settingsStorage
                .GetValue<string>(SettingsKeys.Endpoint)?
                .FromJson<EndpointModel>(JsonConverter);

            Uri = endpointModel?.Uri;
            OnvifPort = endpointModel?.OnvifPort ?? DefaultOnvifPort;
            RtspPort = endpointModel?.RtspPort ?? DefaultRtspPort;

            var credentialsModel = _settingsStorage
                .GetValue<string>(SettingsKeys.Credentials)?
                .FromJson<CredentialsModel>(JsonConverter);

            if (credentialsModel == null || !credentialsModel.SaveCredentials)
            {
                return;
            }

            LoginViewModel.Username = credentialsModel.Username;
            LoginViewModel.Password = credentialsModel.Password;
            LoginViewModel.SaveCredentials = credentialsModel.SaveCredentials;
        }

        #endregion
    }
}
