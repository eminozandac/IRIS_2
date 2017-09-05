using System;

using UIKit;
using CoreAnimation;

namespace CameraControl.iOS.ViewControllers
{
    public partial class SnapshotListViewController : BasicViewController
    {
		protected SnapshotListViewController(IntPtr handle) : base(handle)
        {
			// Note: this .ctor should not contain any initialization logic.
		}


		public override void ViewDidLoad()
        {
            base.ViewDidLoad();
			// Perform any additional setup after loading the view, typically from a nib.
			CALayer headerBorder = new CALayer();
			headerBorder.Frame = new CoreGraphics.CGRect(0.0f, 63.0f, UIScreen.MainScreen.Bounds.Width, 1.0f);
			headerBorder.BackgroundColor = UIColor.LightGray.CGColor;
			headerView.Layer.AddSublayer(headerBorder);
        }

        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();
            // Release any cached data, images, etc that aren't in use.
        }

		partial void OnTouchMenu(Foundation.NSObject sender)
		{
			dropDownMenuView.Hidden = !dropDownMenuView.Hidden;
		}
    }
}

