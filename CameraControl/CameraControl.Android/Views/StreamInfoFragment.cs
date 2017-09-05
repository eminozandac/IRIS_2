using Android.App;
using Android.OS;
using MvvmCross.Binding.Droid.BindingContext;
using MvvmCross.Droid.Support.V4;

namespace CameraControl.Droid.Views
{
    public class StreamInfoFragment : MvxDialogFragment
    {
        public string Title { get; set; }

        public override Dialog OnCreateDialog(Bundle savedState)
        {
            EnsureBindingContextSet(savedState);

            var view = this.BindingInflate(Resource.Layout.StreamInfoDialog, null);

            var dialog = new AlertDialog.Builder(Activity)
                .SetTitle(Title ?? "Stream Info")
                .SetView(view)
                .SetPositiveButton("OK", (s, e) => Dismiss());

            return dialog.Create();
        }
    }
}
