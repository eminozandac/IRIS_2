using MvvmCross.Platform.Platform;
using MvvmCross.Plugins.Messenger;
using MvvmValidation;

namespace CameraControl.Portable.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        #region Fields

        private string _username;
        private string _password;
        private bool _saveCredentials;

        #endregion

        #region Constructors

        public LoginViewModel(IMvxMessenger messenger, IMvxJsonConverter jsonConverter)
            : base(messenger, jsonConverter)
        {
        }

        #endregion

        #region Properties

        public string Username
        {
            get { return _username; }
            set
            {
                if (_username == value)
                {
                    return;
                }

                _username = value;
                RaisePropertyChanged(() => Username);
            }
        }

        public string Password
        {
            get { return _password; }
            set
            {
                if (_password == value)
                {
                    return;
                }

                _password = value;
                RaisePropertyChanged(() => Password);
            }
        }

        public bool SaveCredentials
        {
            get { return _saveCredentials; }
            set
            {
                if (_saveCredentials == value)
                {
                    return;
                }

                _saveCredentials = value;
                RaisePropertyChanged(() => SaveCredentials);
            }
        }

        #endregion

        #region Protected methods

        protected override void ConfigureValidationRules(ValidationHelper validationHelper)
        {
            validationHelper.AddRule(
                () => Username,
                () => !string.IsNullOrWhiteSpace(Password) && string.IsNullOrWhiteSpace(Username)
                    ? RuleResult.Invalid("Username is required")
                    : RuleResult.Valid()
            );

            validationHelper.AddRule(
                () => Password,
                () => !string.IsNullOrWhiteSpace(Username) && string.IsNullOrWhiteSpace(Password)
                    ? RuleResult.Invalid("Password is required")
                    : RuleResult.Valid()
            );
        }

        #endregion
    }
}
