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
	[Register ("LiveViewController")]
	partial class LiveViewController
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
		UIKit.UIImageView imgLogo { get; set; }

		[Outlet]
		UIKit.UILabel lbCamera1 { get; set; }

		[Outlet]
		UIKit.UILabel lbCamera2 { get; set; }

		[Outlet]
		UIKit.UILabel lbCamera3 { get; set; }

		[Outlet]
		UIKit.UILabel lbCamera4 { get; set; }

		[Outlet]
		UIKit.UILabel lbCamera5 { get; set; }

		[Outlet]
		UIKit.UILabel lbOffierCamera { get; set; }

		[Outlet]
		UIKit.UILabel lbSystemSerial { get; set; }

		[Outlet]
		UIKit.UILabel lbTitle { get; set; }

		[Outlet]
		UIKit.UIView vCamera1 { get; set; }

		[Outlet]
		UIKit.UIView vCamera2 { get; set; }

		[Outlet]
		UIKit.UIView vCamera3 { get; set; }

		[Outlet]
		UIKit.UIView vCamera4 { get; set; }

		[Outlet]
		UIKit.UIView vCamera5 { get; set; }

		[Outlet]
		UIKit.UIView vOfficeCamera { get; set; }

		[Action ("onTouchBackward:")]
		partial void onTouchBackward (Foundation.NSObject sender);

		[Action ("onTouchDownward:")]
		partial void onTouchDownward (Foundation.NSObject sender);

		[Action ("onTouchForward:")]
		partial void onTouchForward (Foundation.NSObject sender);

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

			if (imgLogo != null) {
				imgLogo.Dispose ();
				imgLogo = null;
			}

			if (lbCamera1 != null) {
				lbCamera1.Dispose ();
				lbCamera1 = null;
			}

			if (lbCamera2 != null) {
				lbCamera2.Dispose ();
				lbCamera2 = null;
			}

			if (lbCamera3 != null) {
				lbCamera3.Dispose ();
				lbCamera3 = null;
			}

			if (lbCamera4 != null) {
				lbCamera4.Dispose ();
				lbCamera4 = null;
			}

			if (lbCamera5 != null) {
				lbCamera5.Dispose ();
				lbCamera5 = null;
			}

			if (lbOffierCamera != null) {
				lbOffierCamera.Dispose ();
				lbOffierCamera = null;
			}

			if (lbSystemSerial != null) {
				lbSystemSerial.Dispose ();
				lbSystemSerial = null;
			}

			if (lbTitle != null) {
				lbTitle.Dispose ();
				lbTitle = null;
			}

			if (vCamera1 != null) {
				vCamera1.Dispose ();
				vCamera1 = null;
			}

			if (vCamera2 != null) {
				vCamera2.Dispose ();
				vCamera2 = null;
			}

			if (vCamera3 != null) {
				vCamera3.Dispose ();
				vCamera3 = null;
			}

			if (vCamera4 != null) {
				vCamera4.Dispose ();
				vCamera4 = null;
			}

			if (vCamera5 != null) {
				vCamera5.Dispose ();
				vCamera5 = null;
			}

			if (vOfficeCamera != null) {
				vOfficeCamera.Dispose ();
				vOfficeCamera = null;
			}
		}
	}
}
