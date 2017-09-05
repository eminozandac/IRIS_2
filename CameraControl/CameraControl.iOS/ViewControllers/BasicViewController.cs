using System;

using UIKit;
using CameraControl.iOS.Views;
using System.Collections.Generic;
using System.Linq;


namespace CameraControl.iOS
{
    public partial class BasicViewController : UIViewController
    {
        public ViewControllerManager viewControllerManager;
        public DropDownMenuView dropDownMenuView;


        protected BasicViewController(IntPtr handle) : base(handle)
        {
            // Note: this .ctor should not contain any initialization logic.
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
			// Perform any additional setup after loading the view, typically from a nib.
			List<string> menuItems = new List<string>();
			menuItems.Add("Live");
			menuItems.Add("Playback Videos");
			menuItems.Add("Playback Snapshots");
			menuItems.Add("Status");
            menuItems.Add("Settings");
            menuItems.Add("LogOff");
            dropDownMenuView = new DropDownMenuView(this, menuItems);
            dropDownMenuView.Hidden = true;
        }

        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();
            // Release any cached data, images, etc that aren't in use.
        }
    }
}
