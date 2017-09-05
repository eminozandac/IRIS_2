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
	[Register ("StatusViewController")]
	partial class StatusViewController
	{
		[Outlet]
		UIKit.UIButton btnStartEngine { get; set; }

		[Outlet]
		UIKit.UIButton btnTuneRadios { get; set; }

		[Outlet]
		UIKit.UIButton btnUP { get; set; }

		[Outlet]
		UIKit.UIView headerView { get; set; }

		[Outlet]
		UIKit.UILabel lbBatteryValue { get; set; }

		[Outlet]
		UIKit.UILabel lbCpuTemp { get; set; }

		[Outlet]
		UIKit.UILabel lbEnginStatus { get; set; }

		[Outlet]
		UIKit.UILabel lbStorageValue { get; set; }

		[Outlet]
		UIKit.UILabel lbVoltageValue { get; set; }

		[Outlet]
		CameraControl.iOS.Views.StatusProgressView spvBatteryProgress { get; set; }

		[Outlet]
		CameraControl.iOS.Views.StatusProgressView spvCpuTemp { get; set; }

		[Outlet]
		CameraControl.iOS.Views.StatusProgressView spvStrogeProgress { get; set; }

		[Outlet]
		CameraControl.iOS.Views.StatusProgressView spvVoltageProgress { get; set; }

		[Action ("OnTouchMenu:")]
		partial void OnTouchMenu (Foundation.NSObject sender);

		[Action ("OnTouchUp:")]
		partial void OnTouchUp (Foundation.NSObject sender);
		
		void ReleaseDesignerOutlets ()
		{
			if (btnStartEngine != null) {
				btnStartEngine.Dispose ();
				btnStartEngine = null;
			}

			if (btnTuneRadios != null) {
				btnTuneRadios.Dispose ();
				btnTuneRadios = null;
			}

			if (btnUP != null) {
				btnUP.Dispose ();
				btnUP = null;
			}

			if (headerView != null) {
				headerView.Dispose ();
				headerView = null;
			}

			if (lbBatteryValue != null) {
				lbBatteryValue.Dispose ();
				lbBatteryValue = null;
			}

			if (lbCpuTemp != null) {
				lbCpuTemp.Dispose ();
				lbCpuTemp = null;
			}

			if (lbEnginStatus != null) {
				lbEnginStatus.Dispose ();
				lbEnginStatus = null;
			}

			if (lbStorageValue != null) {
				lbStorageValue.Dispose ();
				lbStorageValue = null;
			}

			if (lbVoltageValue != null) {
				lbVoltageValue.Dispose ();
				lbVoltageValue = null;
			}

			if (spvBatteryProgress != null) {
				spvBatteryProgress.Dispose ();
				spvBatteryProgress = null;
			}

			if (spvCpuTemp != null) {
				spvCpuTemp.Dispose ();
				spvCpuTemp = null;
			}

			if (spvStrogeProgress != null) {
				spvStrogeProgress.Dispose ();
				spvStrogeProgress = null;
			}

			if (spvVoltageProgress != null) {
				spvVoltageProgress.Dispose ();
				spvVoltageProgress = null;
			}
		}
	}
}
