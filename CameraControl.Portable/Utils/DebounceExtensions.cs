using System;
using System.Threading;
using System.Threading.Tasks;

namespace CameraControl.Portable.Utils
{
    public static class DebounceExtensions
    {
        public static Action Debounce(this Action action, int delay = 300)
        {
            var last = 0;

            return () =>
            {
                var current = Interlocked.Increment(ref last);
                Task.Delay(delay).ContinueWith(task =>
                {
                    if (current == last)
                    {
                        action();
                    }
                });
            };
        }
    }
}
