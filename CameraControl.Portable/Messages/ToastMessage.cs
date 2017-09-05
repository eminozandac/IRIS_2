using System;
using MvvmCross.Plugins.Messenger;

namespace CameraControl.Portable.Messages
{
    public class ToastMessage : MvxMessage
    {
        public ToastMessage(object sender, string description) : base(sender)
        {
            if (description == null)
            {
                throw new ArgumentNullException(nameof(description));
            }

            Description = description;
        }

        public string Description { get; }
    }
}
