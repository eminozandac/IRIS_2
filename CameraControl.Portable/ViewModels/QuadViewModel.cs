using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using CameraControl.Portable.Messages;
using CameraControl.Portable.Models;
using CameraControl.Portable.Services.Onvif.Abstract;
using CameraControl.Portable.Utils;
using Cheesebaron.MvxPlugins.Settings.Interfaces;
using MvvmCross.Core.ViewModels;
using MvvmCross.Platform.Platform;
using MvvmCross.Plugins.Messenger;

namespace CameraControl.Portable.ViewModels
{
    public class QuadViewModel : BaseViewModel
    {
        #region Fields

        private static readonly ProfileModel EmptyProfileModel = new ProfileModel
        {
            Name = "Not Selected"
        };

        private readonly ISettings _settingsStorage;

        private ICollection<ProfileModel> _profileModels;
        private ICollection<PresetModel> _presetModels;
        private PresetModel _selectedPresetModel;
        private string _presetName;
        private string _token;

        private MvxCommand _showLiveCommand;
        private MvxCommand _savePresetCommand;
        private MvxCommand _loadPresetCommand;

        #endregion

        #region Constructors

        public QuadViewModel(IOnvifServiceAggregator onvifServiceAggregator, IMvxMessenger messenger, IMvxJsonConverter jsonConverter, ISettings settingsStorage)
            : base(messenger, jsonConverter)
        {
            _settingsStorage = settingsStorage;

            QuadCellViewModels = new ReadOnlyCollection<QuadCellViewModel>(new List<QuadCellViewModel>
            {
                new QuadCellViewModel(this, onvifServiceAggregator, messenger, settingsStorage, jsonConverter),
                new QuadCellViewModel(this, onvifServiceAggregator, messenger, settingsStorage, jsonConverter),
                new QuadCellViewModel(this, onvifServiceAggregator, messenger, settingsStorage, jsonConverter),
                new QuadCellViewModel(this, onvifServiceAggregator, messenger, settingsStorage, jsonConverter)
            });
        }

        #endregion

        #region Properties

        public ReadOnlyCollection<QuadCellViewModel> QuadCellViewModels { get; }

        public ICollection<ProfileModel> ProfileModels
        {
            get { return _profileModels; }
            set
            {
                _profileModels = value;
                RaisePropertyChanged(() => ProfileModels);
                LoadPresetCommand.RaiseCanExecuteChanged();
            }
        }

        public ICollection<PresetModel> PresetModels
        {
            get { return _presetModels; }
            set
            {
                _presetModels = value;
                RaisePropertyChanged(() => PresetModels);
            }
        }

        public PresetModel SelectedPresetModel
        {
            get { return _selectedPresetModel; }
            set
            {
                _selectedPresetModel = value;
                RaisePropertyChanged(() => SelectedPresetModel);
                SavePresetCommand.RaiseCanExecuteChanged();
                LoadPresetCommand.RaiseCanExecuteChanged();
            }
        }

        public string PresetName
        {
            get { return _presetName; }
            set
            {
                if (_presetName == value)
                {
                    return;
                }

                _presetName = value;
                RaisePropertyChanged(() => PresetName);
                SavePresetCommand.RaiseCanExecuteChanged();
            }
        }

        #endregion

        #region Commands

        public MvxCommand ShowLiveCommand => _showLiveCommand ?? (_showLiveCommand = new MvxCommand(ShowLive, CanShowLive));

        public MvxCommand SavePresetCommand => _savePresetCommand ?? (_savePresetCommand = new MvxCommand(SavePreset, CanSavePreset));

        public MvxCommand LoadPresetCommand => _loadPresetCommand ?? (_loadPresetCommand = new MvxCommand(LoadPreset, CanLoadPreset));

        #endregion

        #region Public methods

        public void Init(string token)
        {
            _token = token;

            ProfileModels = _settingsStorage
                .GetValue<string>(SettingsKeys.Profiles)
                .FromJson<ICollection<ProfileModel>>(JsonConverter);

            ProfileModels.Add(EmptyProfileModel);
            foreach (var quadCellViewModel in QuadCellViewModels)
            {
                quadCellViewModel.SelectedProfileModel = EmptyProfileModel;
            }

            RestoreSettings();
        }

        #endregion

        #region Protected methods

        protected override void OnIsBusyChanged(bool isBusy)
        {
            foreach (var quadCellViewModel in QuadCellViewModels)
            {
                quadCellViewModel.ShowLiveCommand.RaiseCanExecuteChanged();
            }
        }

        #endregion

        #region Private methods

        private void ShowLive()
        {
            var presentationBundle = new MvxBundle(new Dictionary<string, string> {{"NavigationMode", "ClearTop"}});
            ShowViewModel<LiveViewModel>(new {autoPlay = true, token = _token}, presentationBundle);
        }

        private bool CanShowLive() => !IsBusy;

        private void SavePreset()
        {
            PresetModel presetModel;

            if (PresetName == null)
            {
                presetModel = SelectedPresetModel;
            }
            else
            {
                var endpointModel = _settingsStorage
                    .GetValue<string>(SettingsKeys.Endpoint)
                    .FromJson<EndpointModel>(JsonConverter);

                presetModel = new PresetModel
                {
                    Name = PresetName,
                    Uri = endpointModel.Uri
                };

                PresetModels.Add(presetModel);
                PresetModels = PresetModels.ToList();

                SelectedPresetModel = presetModel;
            }

            foreach (var quadCell in QuadCellViewModels.Select((vm, i) => new {vm, i}))
            {
                presetModel.ProfileTokens[quadCell.i] = quadCell.vm.SelectedProfileModel?.Token;
            }

            var presetModels = _settingsStorage
                .GetValue<string>(SettingsKeys.Presets)?
                .FromJson<ICollection<PresetModel>>(JsonConverter) ?? new List<PresetModel>();

            var savedPresetModel = presetModels.SingleOrDefault(pm => pm.Id == presetModel.Id);
            if (savedPresetModel != null)
            {
                savedPresetModel.Uri = presetModel.Uri;
                savedPresetModel.Name = presetModel.Name;
                savedPresetModel.ProfileTokens = presetModel.ProfileTokens;
            }
            else
            {
                presetModels.Add(presetModel);
            }

            _settingsStorage.AddOrUpdateValue(SettingsKeys.Presets, presetModels.ToJson(JsonConverter));

            var toastMesage = new ToastMessage(this, "Saved");
            Messenger.Publish(toastMesage);

            PresetName = null;
        }

        private bool CanSavePreset() => PresetName != null || SelectedPresetModel != null;

        private void LoadPreset()
        {
            var profileModelsDict = ProfileModels
                .Where(pm => pm.Token != null)
                .ToDictionary(pm => pm.Token);

            foreach (var profileToken in SelectedPresetModel.ProfileTokens.Select((t, i) => new {t, i}))
            {
                ProfileModel profileModel;

                QuadCellViewModels[profileToken.i].SelectedProfileModel = profileToken.t != null && profileModelsDict.TryGetValue(SelectedPresetModel.ProfileTokens[profileToken.i], out profileModel)
                    ? profileModel
                    : EmptyProfileModel;
            }
        }

        private bool CanLoadPreset() => SelectedPresetModel != null && ProfileModels != null;

        private void RestoreSettings()
        {
            var presetModels = _settingsStorage
                .GetValue<string>(SettingsKeys.Presets)?
                .FromJson<ICollection<PresetModel>>(JsonConverter) ?? new List<PresetModel>();

            var endpointModel = _settingsStorage
                .GetValue<string>(SettingsKeys.Endpoint)
                .FromJson<EndpointModel>(JsonConverter);

            PresetModels = presetModels.Where(pm => pm.Uri == endpointModel.Uri).ToList();
            if (PresetModels != null)
            {
                SelectedPresetModel = PresetModels.FirstOrDefault();
            }
        }

        #endregion
    }
}
