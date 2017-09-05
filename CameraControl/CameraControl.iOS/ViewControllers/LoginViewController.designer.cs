// WARNING
//
// This file has been generated automatically by Visual Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace CameraControl.iOS
{
    [Register ("LoginViewController")]
    partial class LoginViewController
    {
        [Outlet]
        UIKit.UIButton btnConnect { get; set; }

        [Outlet]
        UIKit.UIView headerView { get; set; }

        [Outlet]
        UIKit.UIImageView imgLogo { get; set; }

        [Outlet]
        UIKit.UILabel lbAppName { get; set; }

        [Outlet]
        UIKit.UILabel lbTitle { get; set; }

        [Outlet]
        UIKit.UITextField tfIPAddress { get; set; }

        [Outlet]
        UIKit.UITextField tfIrisNumber { get; set; }

        [Outlet]
        UIKit.UITextField tfPassword { get; set; }

        [Outlet]
        UIKit.UITextField tfUserName { get; set; }

        [Action ("onTouchConnect:")]
        partial void onTouchConnect (Foundation.NSObject sender);
        
        void ReleaseDesignerOutlets ()
        {
            if (btnConnect != null) {
                btnConnect.Dispose ();
                btnConnect = null;
            }

            if (headerView != null) {
                headerView.Dispose ();
                headerView = null;
            }

            if (imgLogo != null) {
                imgLogo.Dispose ();
                imgLogo = null;
            }

            if (lbAppName != null) {
                lbAppName.Dispose ();
                lbAppName = null;
            }

            if (lbTitle != null) {
                lbTitle.Dispose ();
                lbTitle = null;
            }

            if (tfIPAddress != null) {
                tfIPAddress.Dispose ();
                tfIPAddress = null;
            }

            if (tfIrisNumber != null) {
                tfIrisNumber.Dispose ();
                tfIrisNumber = null;
            }

            if (tfPassword != null) {
                tfPassword.Dispose ();
                tfPassword = null;
            }

            if (tfUserName != null) {
                tfUserName.Dispose ();
                tfUserName = null;
            }
        }
    }
}
