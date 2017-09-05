using System;

using UIKit;
using CoreAnimation;
using Foundation;

namespace CameraControl.iOS.ViewControllers
{
    public partial class LiveViewController : BasicViewController
    {
		protected LiveViewController(IntPtr handle) : base(handle)
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

            //Testing
            vCamera1.Layer.BorderColor = UIColor.LightGray.CGColor;
            vCamera1.Layer.BorderWidth = 1.0f;
            vCamera2.Layer.BorderColor = UIColor.LightGray.CGColor;
            vCamera2.Layer.BorderWidth = 1.0f;
			vCamera3.Layer.BorderColor = UIColor.LightGray.CGColor;
			vCamera3.Layer.BorderWidth = 1.0f;
			vCamera4.Layer.BorderColor = UIColor.LightGray.CGColor;
			vCamera4.Layer.BorderWidth = 1.0f;
            vCamera5.Layer.BorderColor = UIColor.LightGray.CGColor;
            vCamera5.Layer.BorderWidth = 1.0f;
            vOfficeCamera.Layer.BorderColor = UIColor.LightGray.CGColor;
			vOfficeCamera.Layer.BorderWidth = 1.0f;

			vCamera5.UserInteractionEnabled = true;
			UITapGestureRecognizer tapGesture = new UITapGestureRecognizer(TapFullScreen);
			tapGesture.NumberOfTapsRequired = 1;
			vCamera5.AddGestureRecognizer(tapGesture);

		}

		public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();
            // Release any cached data, images, etc that aren't in use.
        }

		partial void onTouchBackward(NSObject sender)
        {
			UIStoryboard storyboard = UIStoryboard.FromName("Main", null);
            SnapshotListViewController viewController = storyboard.InstantiateViewController("SnapshotListViewController") as SnapshotListViewController;

			PresentViewController(viewController, true, null);
        }


		partial void onTouchDownward(NSObject sender)
        {
			UIStoryboard storyboard = UIStoryboard.FromName("Main", null);
            StatusViewController viewController = storyboard.InstantiateViewController("StatusViewController") as StatusViewController;

			PresentViewController(viewController, true, null);
        }

        partial void onTouchForward(NSObject sender)
        {
			UIStoryboard storyboard = UIStoryboard.FromName("Main", null);
            VideoListViewController viewController = storyboard.InstantiateViewController("VideoListViewController") as VideoListViewController;

			PresentViewController(viewController, true, null);
            
        }

		void TapFullScreen(UITapGestureRecognizer tap1)
		{
			UIStoryboard storyboard = UIStoryboard.FromName("Main", null);
			FullScreenViewController viewController = storyboard.InstantiateViewController("FullScreenViewController") as FullScreenViewController;

			PresentViewController(viewController, true, null);
		}

        partial void OnTouchMenu(Foundation.NSObject sender) {
            dropDownMenuView.Hidden = !dropDownMenuView.Hidden;
        }
	}
}

