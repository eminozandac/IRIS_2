// WARNING
//
// This file has been generated automatically by Visual Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace CameraControl.iOS.ViewControllers
{
	[Register ("PlayVideoViewController")]
	partial class PlayVideoViewController
	{
		[Outlet]
		UIKit.UIButton btnBack { get; set; }

		[Outlet]
		UIKit.UIButton btnMenu { get; set; }

		[Outlet]
		UIKit.UIView headerView { get; set; }

		[Outlet]
		UIKit.UILabel lbDate { get; set; }

		[Outlet]
		UIKit.UILabel lbDuration { get; set; }

		[Outlet]
		UIKit.UILabel lbName { get; set; }

		[Outlet]
		UIKit.UILabel lbSystemSerial { get; set; }

		[Outlet]
		UIKit.UILabel lbTime { get; set; }

		[Outlet]
		UIKit.UILabel lbTitle { get; set; }

		[Outlet]
		UIKit.UIView videoView { get; set; }

		[Action ("onTouchBack:")]
		partial void onTouchBack (Foundation.NSObject sender);

		[Action ("onTouchForward:")]
		partial void onTouchForward (Foundation.NSObject sender);

		[Action ("OnTouchMenu:")]
		partial void OnTouchMenu (Foundation.NSObject sender);
		
		void ReleaseDesignerOutlets ()
		{
			if (btnBack != null) {
				btnBack.Dispose ();
				btnBack = null;
			}

			if (btnMenu != null) {
				btnMenu.Dispose ();
				btnMenu = null;
			}

			if (headerView != null) {
				headerView.Dispose ();
				headerView = null;
			}

			if (lbDate != null) {
				lbDate.Dispose ();
				lbDate = null;
			}

			if (lbDuration != null) {
				lbDuration.Dispose ();
				lbDuration = null;
			}

			if (lbName != null) {
				lbName.Dispose ();
				lbName = null;
			}

			if (lbSystemSerial != null) {
				lbSystemSerial.Dispose ();
				lbSystemSerial = null;
			}

			if (lbTime != null) {
				lbTime.Dispose ();
				lbTime = null;
			}

			if (lbTitle != null) {
				lbTitle.Dispose ();
				lbTitle = null;
			}

			if (videoView != null) {
				videoView.Dispose ();
				videoView = null;
			}
		}
	}
}
