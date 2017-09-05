using System;
using CameraControl.Portable.Models.MediaPlayer;

namespace CameraControl.Portable.Models
{
    public class StreamInfoModel
    {
        public StreamInfoModel(VideoStream videoStream)
        {
            if (videoStream == null)
            {
                throw new ArgumentNullException(nameof(videoStream));
            }

            Codec = videoStream.Format?.Value;

            if (videoStream.Height != null && videoStream.Width != null)
            {
                Resolution = $"{videoStream.Width.Value}x{videoStream.Height.Value}";
            }

            Fps = videoStream.Fps?.Value.ToString();
        }

        public string Codec { get; }

        public string Resolution { get; }

        public string Fps { get; }

        public string Name { get; set; }

        public int DroppedFrame { get; set; }
    }
}
