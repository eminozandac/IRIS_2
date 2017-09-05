using Android.App;
using Android.Content.PM;
using CameraControl.Portable.ViewModels;

namespace CameraControl.Droid.Views
{
    //[Activity(Label = "Iris Mobile", MainLauncher = true, ScreenOrientation = ScreenOrientation.Landscape)]
    [Activity(ScreenOrientation = ScreenOrientation.Landscape)]
    public class MainActivity : BaseActivity<MainViewModel>
    {
        protected override void OnViewModelSet()
        {
            base.OnViewModelSet();

            SetContentView(Resource.Layout.Status);
        }
    }
}
