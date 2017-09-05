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
    [Register ("TuneRadiosViewController")]
    partial class TuneRadiosViewController
    {
        [Outlet]
        UIKit.UIButton btnMenu { get; set; }

        [Outlet]
        UIKit.UIButton btnUpdate { get; set; }

        [Outlet]
        UIKit.UIView headerView { get; set; }

        [Outlet]
        UIKit.UILabel lbCurrentFreq { get; set; }

        [Outlet]
        UIKit.UILabel lbCurrentTitle { get; set; }

        [Outlet]
        UIKit.UILabel lbSystemSerial { get; set; }

        [Outlet]
        UIKit.UISegmentedControl scSelectItem { get; set; }

        [Outlet]
        UIKit.UITextField tfCustomFreq { get; set; }

        [Action ("onTouchUpdate:")]
        partial void onTouchUpdate (Foundation.NSObject sender);
        
        void ReleaseDesignerOutlets ()
        {
            if (headerView != null) {
                headerView.Dispose ();
                headerView = null;
            }

            if (lbSystemSerial != null) {
                lbSystemSerial.Dispose ();
                lbSystemSerial = null;
            }

            if (btnMenu != null) {
                btnMenu.Dispose ();
                btnMenu = null;
            }

            if (scSelectItem != null) {
                scSelectItem.Dispose ();
                scSelectItem = null;
            }

            if (lbCurrentTitle != null) {
                lbCurrentTitle.Dispose ();
                lbCurrentTitle = null;
            }

            if (lbCurrentFreq != null) {
                lbCurrentFreq.Dispose ();
                lbCurrentFreq = null;
            }

            if (tfCustomFreq != null) {
                tfCustomFreq.Dispose ();
                tfCustomFreq = null;
            }

            if (btnUpdate != null) {
                btnUpdate.Dispose ();
                btnUpdate = null;
            }
        }
    }
}
