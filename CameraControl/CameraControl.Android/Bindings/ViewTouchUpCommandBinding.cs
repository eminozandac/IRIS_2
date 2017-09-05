using System;
using System.Windows.Input;
using Android.Views;
using MvvmCross.Binding;
using MvvmCross.Binding.Droid.Target;

namespace CameraControl.Droid.Bindings
{
    public class ViewTouchUpCommandBinding : MvxAndroidTargetBinding
    {
        protected View View => (View) Target;

        private ICommand _command;

        public ViewTouchUpCommandBinding(object target) : base(target)
        {
            View.Touch += ViewTouch;
        }

        public override Type TargetType => typeof(ICommand);

        public override MvxBindingMode DefaultMode => MvxBindingMode.OneWay;

        protected override void SetValueImpl(object target, object value)
        {
            _command = (ICommand) value;
        }

        protected override void Dispose(bool isDisposing)
        {
            if (isDisposing)
            {
                View.Touch -= ViewTouch;
            }
            base.Dispose(isDisposing);
        }

        private void ViewTouch(object sender, View.TouchEventArgs e)
        {
            if (e.Event.Action != MotionEventActions.Up)
            {
                return;
            }

            if (_command != null && _command.CanExecute(null))
            {
                _command.Execute(null);
            }
        }
    }
}
