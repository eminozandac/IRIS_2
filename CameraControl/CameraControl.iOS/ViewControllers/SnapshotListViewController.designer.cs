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
	[Register ("SnapshotListViewController")]
	partial class SnapshotListViewController
	{
		[Outlet]
		UIKit.UIButton btnBackward { get; set; }

		[Outlet]
		UIKit.UIButton btnDownward { get; set; }

		[Outlet]
		UIKit.UIButton btnForward { get; set; }

		[Outlet]
		UIKit.UIButton btnMenu { get; set; }

		[Outlet]
		UIKit.UIView headerView { get; set; }

		[Outlet]
		UIKit.UIButton OnTouchBackword { get; set; }

		[Outlet]
		UIKit.UIButton OnTouchDownward { get; set; }

		[Outlet]
		UIKit.UIButton OnTouchForward { get; set; }

		[Outlet]
		UIKit.UITableView tbVideoList { get; set; }

		[Action ("OnTouchMenu:")]
		partial void OnTouchMenu (Foundation.NSObject sender);
		
		void ReleaseDesignerOutlets ()
		{
			if (btnBackward != null) {
				btnBackward.Dispose ();
				btnBackward = null;
			}

			if (btnDownward != null) {
				btnDownward.Dispose ();
				btnDownward = null;
			}

			if (btnForward != null) {
				btnForward.Dispose ();
				btnForward = null;
			}

			if (btnMenu != null) {
				btnMenu.Dispose ();
				btnMenu = null;
			}

			if (headerView != null) {
				headerView.Dispose ();
				headerView = null;
			}

			if (OnTouchBackword != null) {
				OnTouchBackword.Dispose ();
				OnTouchBackword = null;
			}

			if (OnTouchDownward != null) {
				OnTouchDownward.Dispose ();
				OnTouchDownward = null;
			}

			if (OnTouchForward != null) {
				OnTouchForward.Dispose ();
				OnTouchForward = null;
			}

			if (tbVideoList != null) {
				tbVideoList.Dispose ();
				tbVideoList = null;
			}
		}
	}
}
