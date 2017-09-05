namespace CameraControl.Portable.Models
{
    public class ProfileModel
    {
        #region Constructors

        public ProfileModel(string token = null)
        {
            Token = token;
        }

        #endregion

        #region Properties

        public string Name { get; set; }

        public string Token { get; }

        public string VideosSourceToken { get; set; }

        #endregion

        #region Equals
        
        public override bool Equals(object obj)
        {
            var profileModel = obj as ProfileModel;

            if (profileModel == null)
            {
                return false;
            }

            return profileModel.Token == Token;
        }

        public override int GetHashCode()
        {
            return Token == null ? 0 : Token.GetHashCode();
        }

        #endregion
    }
}
