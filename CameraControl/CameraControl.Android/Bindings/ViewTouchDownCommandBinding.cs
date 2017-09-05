using System;
using System.Windows.Input;
using Android.Views;
using MvvmCross.Binding;
using MvvmCross.Binding.Droid.Target;

namespace CameraControl.Droid.Bindings
{
    public class ViewTouchDownCommandBinding : MvxAndroidTargetBinding
    {
        protected View View => (View) Target;

        private ICommand _command;

        public ViewTouchDownCommandBinding(object target) : base(target)
        {
            View.Touch += ButtonTouch;
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
                View.Touch -= ButtonTouch;
            }
            base.Dispose(isDisposing);
        }

        private void ButtonTouch(object sender, View.TouchEventArgs e)
        {
            if (e.Event.Action != MotionEventActions.Down)
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
