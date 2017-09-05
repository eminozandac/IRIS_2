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
	[Register ("FullScreenViewController")]
	partial class FullScreenViewController
	{
		[Outlet]
		UIKit.UIButton btnAE { get; set; }

		[Outlet]
		UIKit.UIButton btnAF { get; set; }

		[Outlet]
		UIKit.UIButton btnBack { get; set; }

		[Outlet]
		UIKit.UIButton btnCamera { get; set; }

		[Outlet]
		UIKit.UIButton btnDefog { get; set; }

		[Outlet]
		UIKit.UIButton btnLower { get; set; }

		[Outlet]
		UIKit.UIButton btnMenu { get; set; }

		[Outlet]
		UIKit.UIButton btnMinus { get; set; }

		[Outlet]
		UIKit.UIButton btnMoveDown { get; set; }

		[Outlet]
		UIKit.UIButton btnMoveHome { get; set; }

		[Outlet]
		UIKit.UIButton btnMoveLeft { get; set; }

		[Outlet]
		UIKit.UIButton btnMoveRight { get; set; }

		[Outlet]
		UIKit.UIButton btnMoveUp { get; set; }

		[Outlet]
		UIKit.UIButton btnPlus { get; set; }

		[Outlet]
		UIKit.UIButton btnRaise { get; set; }

		[Outlet]
		UIKit.UIButton btnReady { get; set; }

		[Outlet]
		UIKit.UIButton btnVideoCamera { get; set; }

		[Outlet]
		UIKit.UIView headerView { get; set; }

		[Outlet]
		UIKit.UILabel lbSystemSerial { get; set; }

		[Outlet]
		UIKit.UILabel lbTitle { get; set; }

		[Outlet]
		UIKit.UIView videoView { get; set; }

		[Outlet]
		UIKit.UIView vZoom { get; set; }

		[Action ("onTouchBack:")]
		partial void onTouchBack (Foundation.NSObject sender);

		[Action ("OnTouchMenu:")]
		partial void OnTouchMenu (Foundation.NSObject sender);
		
		void ReleaseDesignerOutlets ()
		{
			if (btnAE != null) {
				btnAE.Dispose ();
				btnAE = null;
			}

			if (btnAF != null) {
				btnAF.Dispose ();
				btnAF = null;
			}

			if (btnBack != null) {
				btnBack.Dispose ();
				btnBack = null;
			}

			if (btnCamera != null) {
				btnCamera.Dispose ();
				btnCamera = null;
			}

			if (btnDefog != null) {
				btnDefog.Dispose ();
				btnDefog = null;
			}

			if (btnLower != null) {
				btnLower.Dispose ();
				btnLower = null;
			}

			if (btnMenu != null) {
				btnMenu.Dispose ();
				btnMenu = null;
			}

			if (btnMinus != null) {
				btnMinus.Dispose ();
				btnMinus = null;
			}

			if (btnMoveDown != null) {
				btnMoveDown.Dispose ();
				btnMoveDown = null;
			}

			if (btnMoveHome != null) {
				btnMoveHome.Dispose ();
				btnMoveHome = null;
			}

			if (btnMoveLeft != null) {
				btnMoveLeft.Dispose ();
				btnMoveLeft = null;
			}

			if (btnMoveRight != null) {
				btnMoveRight.Dispose ();
				btnMoveRight = null;
			}

			if (btnMoveUp != null) {
				btnMoveUp.Dispose ();
				btnMoveUp = null;
			}

			if (btnPlus != null) {
				btnPlus.Dispose ();
				btnPlus = null;
			}

			if (btnRaise != null) {
				btnRaise.Dispose ();
				btnRaise = null;
			}

			if (btnReady != null) {
				btnReady.Dispose ();
				btnReady = null;
			}

			if (btnVideoCamera != null) {
				btnVideoCamera.Dispose ();
				btnVideoCamera = null;
			}

			if (headerView != null) {
				headerView.Dispose ();
				headerView = null;
			}

			if (lbSystemSerial != null) {
				lbSystemSerial.Dispose ();
				lbSystemSerial = null;
			}

			if (lbTitle != null) {
				lbTitle.Dispose ();
				lbTitle = null;
			}

			if (videoView != null) {
				videoView.Dispose ();
				videoView = null;
			}

			if (vZoom != null) {
				vZoom.Dispose ();
				vZoom = null;
			}
		}
	}
}
