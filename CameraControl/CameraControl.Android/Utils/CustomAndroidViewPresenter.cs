using Android.Content;
using MvvmCross.Core.ViewModels;
using MvvmCross.Droid.Views;

namespace CameraControl.Droid.Utils
{
    public class CustomAndroidViewPresenter : MvxAndroidViewPresenter
    {
        public override void Show(MvxViewModelRequest request)
        {
            // TODO: Move string values to constants.
            if (request.PresentationValues != null && request.PresentationValues.ContainsKey("NavigationMode") && request.PresentationValues["NavigationMode"] == "ClearTop")
            {
                var intent = CreateIntentForRequest(request);
                intent.AddFlags(ActivityFlags.ClearTop);
                Show(intent);
                return;
            }

            base.Show(request);
        }
    }
}
