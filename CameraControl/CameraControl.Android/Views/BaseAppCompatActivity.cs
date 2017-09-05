using Android.OS;
using Android.Widget;
using CameraControl.Droid.Utils;
using CameraControl.Portable.Messages;
using CameraControl.Portable.ViewModels;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Droid.Support.V7.AppCompat;
using MvvmCross.Plugins.Messenger;

namespace CameraControl.Droid.Views
{
    public abstract class BaseAppCompatActivity<TViewModel> : MvxAppCompatActivity<TViewModel>
        where TViewModel : BaseViewModel
    {
        private BindableProgress _bindableProgress;
        private MvxSubscriptionToken _toastSubscriptionToken;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            _bindableProgress = new BindableProgress(this);
            this.CreateBindingSet<BaseAppCompatActivity<TViewModel>, TViewModel>()
                .Bind(_bindableProgress).For(p => p.Visible).To(vm => vm.IsBusy)
                .Apply();

            _toastSubscriptionToken = ViewModel.Messenger.SubscribeOnMainThread<ToastMessage>(OnToastMessage, MvxReference.Strong);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            ViewModel.Messenger.Unsubscribe<ToastMessage>(_toastSubscriptionToken);
        }

        private void OnToastMessage(ToastMessage message)
        {
            if (message.Sender != ViewModel)
            {
                return;
            }

            Toast.MakeText(this, message.Description, ToastLength.Short).Show();
        }
    }
}
