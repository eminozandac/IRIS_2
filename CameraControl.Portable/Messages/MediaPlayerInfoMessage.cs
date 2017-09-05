using System;
using CameraControl.Portable.Models;
using MvvmCross.Plugins.Messenger;

namespace CameraControl.Portable.Messages
{
    public class MediaPlayerInfoMessage : MvxMessage
    {
        public MediaPlayerInfoMessage(object sender, StreamInfoModel streamInfoModel) : base(sender)
        {
            if (streamInfoModel == null)
            {
                throw new ArgumentNullException(nameof(streamInfoModel));
            }

            StreamInfoModel = streamInfoModel;
        }

        public StreamInfoModel StreamInfoModel { get; }
    }
}
