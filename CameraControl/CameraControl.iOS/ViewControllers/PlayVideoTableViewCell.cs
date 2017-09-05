using System;

using Foundation;
using UIKit;

namespace CameraControl.iOS.ViewControllers
{
    public partial class PlayVideoTableViewCell : UITableViewCell
    {
        public static readonly NSString Key = new NSString("PlayVideoTableViewCell");
        public static readonly UINib Nib;

        static PlayVideoTableViewCell()
        {
            Nib = UINib.FromName("PlayVideoTableViewCell", NSBundle.MainBundle);

        }

        protected PlayVideoTableViewCell(IntPtr handle) : base(handle)
        {
            // Note: this .ctor should not contain any initialization logic.

        }

        public UILabel GetCameraLabel() { return lbCamera; }
        public UILabel GetDateTimeLabel() { return lbDataTime; }
        public UILabel GetDurationLabel() { return lbDuration; }
        public UILabel GetViewLabel() { return lbView; }

    }
}
