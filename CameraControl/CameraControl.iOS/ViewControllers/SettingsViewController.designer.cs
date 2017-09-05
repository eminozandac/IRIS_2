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
	[Register ("SettingsViewController")]
	partial class SettingsViewController
	{
		[Outlet]
		UIKit.UIButton btnMenu { get; set; }

		[Outlet]
		UIKit.UIButton btnSave { get; set; }

		[Outlet]
		UIKit.UIView headerView { get; set; }

		[Outlet]
		UIKit.UISegmentedControl sgcDropFrames { get; set; }

		[Outlet]
		UIKit.UISegmentedControl sgcSynchronization { get; set; }

		[Outlet]
		UIKit.UISegmentedControl sgcVideoDecoding { get; set; }

		[Outlet]
		UIKit.UITextField tfBufferingTime { get; set; }

		[Outlet]
		UIKit.UITextField tfProbeTime { get; set; }

		[Action ("OnTouchMenu:")]
		partial void OnTouchMenu (Foundation.NSObject sender);

		[Action ("OnTouchSave:")]
		partial void OnTouchSave (Foundation.NSObject sender);
		
		void ReleaseDesignerOutlets ()
		{
			if (btnSave != null) {
				btnSave.Dispose ();
				btnSave = null;
			}

			if (headerView != null) {
				headerView.Dispose ();
				headerView = null;
			}

			if (btnMenu != null) {
				btnMenu.Dispose ();
				btnMenu = null;
			}

			if (sgcDropFrames != null) {
				sgcDropFrames.Dispose ();
				sgcDropFrames = null;
			}

			if (sgcSynchronization != null) {
				sgcSynchronization.Dispose ();
				sgcSynchronization = null;
			}

			if (sgcVideoDecoding != null) {
				sgcVideoDecoding.Dispose ();
				sgcVideoDecoding = null;
			}

			if (tfBufferingTime != null) {
				tfBufferingTime.Dispose ();
				tfBufferingTime = null;
			}

			if (tfProbeTime != null) {
				tfProbeTime.Dispose ();
				tfProbeTime = null;
			}
		}
	}
}
