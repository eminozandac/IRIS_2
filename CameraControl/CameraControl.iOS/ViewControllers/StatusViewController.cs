using System;

using UIKit;
using CoreAnimation;

namespace CameraControl.iOS.ViewControllers
{
    public partial class StatusViewController : BasicViewController
    {
        protected StatusViewController(IntPtr handle) : base(handle)
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

            btnStartEngine.Layer.CornerRadius = 5.0f;
            btnTuneRadios.Layer.CornerRadius = 5.0f;

            spvBatteryProgress.SetProgressValue(50);
            spvCpuTemp.SetProgressValue(34);
            spvStrogeProgress.SetProgressValue(99);
            spvVoltageProgress.SetProgressValue(50);
		}

        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();
            // Release any cached data, images, etc that aren't in use.
        }

        //IBAction
        partial void OnTouchUp(Foundation.NSObject sender)
        {
            DismissViewController(true, null);
        }

		partial void OnTouchMenu(Foundation.NSObject sender)
		{
			dropDownMenuView.Hidden = !dropDownMenuView.Hidden;
		}
    }
}

