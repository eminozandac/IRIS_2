using System;

namespace CameraControl.Portable.Models
{
    public class PresetModel
    {
        #region Constructors

        public PresetModel()
        {
            ProfileTokens = new string[4];
        }

        #endregion

        #region Properties

        // TODO: Move Id initialization to separate constructor.
        public Guid Id { get; set; } = Guid.NewGuid();

        public string Uri { get; set; }

        public string Name { get; set; }

        public string[] ProfileTokens { get; set; }

        #endregion

        #region Equals

        public override bool Equals(object obj)
        {
            var presetModel = obj as PresetModel;

            if (presetModel == null)
            {
                return false;
            }

            return presetModel.Id == Id;
        }

        public override int GetHashCode()
        {
            return Id.ToString().GetHashCode();
        }

        #endregion
    }
}
