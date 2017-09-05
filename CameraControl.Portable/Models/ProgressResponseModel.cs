namespace CameraControl.Portable.Models
{
    public class ProgressResponseModel
    {
        public ProgressResponseModel(long current, long duration)
        {
            Current = current;
            Duration = duration;
        }

        public long Current { get; }

        public long Duration { get; }
    }
}
