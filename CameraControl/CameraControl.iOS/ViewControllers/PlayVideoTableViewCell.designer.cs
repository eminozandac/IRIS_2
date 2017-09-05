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
	[Register ("PlayVideoTableViewCell")]
	partial class PlayVideoTableViewCell
	{
		[Outlet]
		UIKit.UILabel lbCamera { get; set; }

		[Outlet]
		UIKit.UILabel lbDataTime { get; set; }

		[Outlet]
		UIKit.UILabel lbDuration { get; set; }

		[Outlet]
		UIKit.UILabel lbView { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (lbCamera != null) {
				lbCamera.Dispose ();
				lbCamera = null;
			}

			if (lbDataTime != null) {
				lbDataTime.Dispose ();
				lbDataTime = null;
			}

			if (lbDuration != null) {
				lbDuration.Dispose ();
				lbDuration = null;
			}

			if (lbView != null) {
				lbView.Dispose ();
				lbView = null;
			}
		}
	}
}
