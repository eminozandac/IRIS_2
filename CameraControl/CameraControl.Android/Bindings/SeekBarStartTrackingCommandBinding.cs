using System;
using System.Windows.Input;
using Android.Widget;
using MvvmCross.Binding;
using MvvmCross.Binding.Droid.Target;
using MvvmCross.Platform.WeakSubscription;

namespace CameraControl.Droid.Bindings
{
    public class SeekBarStartTrackingCommandBinding : MvxAndroidTargetBinding
    {
        private ICommand _command;
        private IDisposable _stopTrackingSubscription;
        private IDisposable _canExecuteSubscription;
        private readonly EventHandler<EventArgs> _canExecuteEventHandler;

        public SeekBarStartTrackingCommandBinding(SeekBar seekBar) : base(seekBar)
        {
            _canExecuteEventHandler = OnCanExecuteChanged;
            _stopTrackingSubscription = seekBar.WeakSubscribe<SeekBar, SeekBar.StartTrackingTouchEventArgs>(nameof(seekBar.StartTrackingTouch), StartTrackingTouch);
        }

        public override Type TargetType => typeof(ICommand);

        public override MvxBindingMode DefaultMode => MvxBindingMode.OneWay;

        protected SeekBar SeekBar => (SeekBar)Target;

        protected override void SetValueImpl(object target, object value)
        {
            _canExecuteSubscription?.Dispose();
            _canExecuteSubscription = null;

            _command = value as ICommand;
            if (_command != null)
            {
                _canExecuteSubscription = _command.WeakSubscribe(_canExecuteEventHandler);
            }

            RefreshEnabledState();
        }

        protected override void Dispose(bool isDisposing)
        {
            if (isDisposing)
            {
                _stopTrackingSubscription?.Dispose();
                _stopTrackingSubscription = null;

                _canExecuteSubscription?.Dispose();
                _canExecuteSubscription = null;
            }
            base.Dispose(isDisposing);
        }

        private void StartTrackingTouch(object sender, SeekBar.StartTrackingTouchEventArgs e)
        {
            if (_command == null || !_command.CanExecute(null))
            {
                return;
            }

            _command.Execute(null);
        }

        private void OnCanExecuteChanged(object sender, EventArgs e)
        {
            RefreshEnabledState();
        }

        private void RefreshEnabledState()
        {
            if (SeekBar == null)
            {
                return;
            }

            var shouldBeEnabled = false;
            if (_command != null)
            {
                shouldBeEnabled = _command.CanExecute(null);
            }

            SeekBar.Enabled = shouldBeEnabled;
        }
    }
}
