using System.Collections.Generic;
using System.Linq;
using CameraControl.Portable.Utils;
using MvvmCross.Core.ViewModels;
using MvvmCross.Platform.Platform;
using MvvmCross.Plugins.Json;
using MvvmCross.Plugins.Messenger;
using MvvmValidation;

namespace CameraControl.Portable.ViewModels
{
    public abstract class BaseViewModel : MvxViewModel
    {
        #region Fields

        private ValidationHelper _validationHelper;
        private ObservableDictionary<string, string> _errors;
        private bool _isBusy;

        #endregion

        #region Constructors

        protected BaseViewModel(IMvxMessenger messenger, IMvxJsonConverter jsonConverter)
        {
            Messenger = messenger;
            JsonConverter = jsonConverter;
        }

        #endregion

        #region Properties

        public IMvxMessenger Messenger { get; }

        protected IMvxJsonConverter JsonConverter { get; }

        protected ValidationHelper Validator
        {
            get
            {
                if (_validationHelper == null)
                {
                    _validationHelper = new ValidationHelper();
                    ConfigureValidationRules(_validationHelper);
                }

                return _validationHelper;
            }
        }

        public ObservableDictionary<string, string> Errors
        {
            get { return _errors; }
            set
            {
                if (_errors == value)
                {
                    return;
                }

                _errors = value;
                RaisePropertyChanged(() => Errors);
                RaisePropertyChanged(() => IsValid);
            }
        }

        public bool IsValid => !Errors.Any();

        public bool IsBusy
        {
            get { return _isBusy; }
            set
            {
                if (_isBusy == value)
                {
                    return;
                }

                _isBusy = value;
                RaisePropertyChanged(() => IsBusy);
                OnIsBusyChanged(IsBusy);
            }
        }

        #endregion

        #region Protected methods

        protected virtual void OnIsBusyChanged(bool isBusy)
        {
        }

        protected virtual void ConfigureValidationRules(ValidationHelper validationHelper)
        {
        }

        protected new bool ShowViewModel<TViewModel, TInit>(TInit parameters, IMvxBundle presentationBundle = null)
            where TViewModel : MvxViewModel
            where TInit : class 
        {
            var text = parameters.ToJson(JsonConverter);
            return ShowViewModel<TViewModel>(new Dictionary<string, string> {{"parameters", text}}, presentationBundle);
        }

        protected TInit ParseInit<TInit>(string parameters)
            where TInit : class 
        {
            var deserialized = parameters.FromJson<TInit>(JsonConverter);
            return deserialized;
        }

        #endregion

        #region Public methods

        public void Validate()
        {
            var result = Validator.ValidateAll();

            Errors = result.AsObservableDictionary();
        }

        #endregion
    }
}
