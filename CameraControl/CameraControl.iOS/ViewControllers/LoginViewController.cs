using System;

using UIKit;
using CoreAnimation;
using Foundation;
using CameraControl.iOS.ViewControllers;

namespace CameraControl.iOS
{
    public partial class LoginViewController : UIViewController
    {
        public LoginViewController() : base("LoginViewController", null)
        {
        }

		protected LoginViewController(IntPtr handle) : base(handle)
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

            btnConnect.Layer.CornerRadius = 3.0f;
        }

        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();
            // Release any cached data, images, etc that aren't in use.
        }

        partial void onTouchConnect(NSObject sender)
        {
            ViewControllerManager theViewControllerManager = new ViewControllerManager();
			UIStoryboard storyboard = UIStoryboard.FromName("Main", null);

			LiveViewController liveViewController = storyboard.InstantiateViewController("LiveViewController") as LiveViewController;
            theViewControllerManager.viewControllers.Add(liveViewController);
            liveViewController.viewControllerManager = theViewControllerManager;
            theViewControllerManager.rootViewController = liveViewController;

            VideoListViewController videoListViewController = storyboard.InstantiateViewController("VideoListViewController") as VideoListViewController;
			theViewControllerManager.viewControllers.Add(videoListViewController);
			videoListViewController.viewControllerManager = theViewControllerManager;

            SnapshotListViewController snapshotListViewController = storyboard.InstantiateViewController("SnapshotListViewController") as SnapshotListViewController;
			theViewControllerManager.viewControllers.Add(snapshotListViewController);
			snapshotListViewController.viewControllerManager = theViewControllerManager;

			StatusViewController statusViewController = storyboard.InstantiateViewController("StatusViewController") as StatusViewController;
			theViewControllerManager.viewControllers.Add(statusViewController);
			statusViewController.viewControllerManager = theViewControllerManager;

            SettingsViewController settingsViewController = storyboard.InstantiateViewController("SettingsViewController") as SettingsViewController;
			theViewControllerManager.viewControllers.Add(settingsViewController);
			settingsViewController.viewControllerManager = theViewControllerManager;

			PresentViewController(liveViewController, true, null);
		}
    }
}

