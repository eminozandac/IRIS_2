using System;

namespace CameraControl.Portable.Models
{
    public class PlaybackModel
    {
        public PlaybackModel(string connectionUrl, int ffRate = 1000, long startOffset = 0, bool withRestart = false)
        {
            if (connectionUrl == null)
            {
                throw new ArgumentNullException(nameof(connectionUrl));
            }
            if (ffRate < 1000 || ffRate > 2000)
            {
                throw new ArgumentOutOfRangeException(nameof(ffRate));
            }

            ConnectionUrl = connectionUrl;
            FFRate = ffRate;
            WithRestart = withRestart;
            StartOffset = startOffset;
        }

        public string ConnectionUrl { get; }

        public int FFRate { get; }

        public bool WithRestart { get; }

        public long StartOffset { get; }
    }
}
