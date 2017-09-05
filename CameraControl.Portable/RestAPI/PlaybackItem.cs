using System;
namespace CameraControl.Portable.RestAPI
{
	public class PlaybackItem
	{
		public DateTime Start { get; set; }
		public DateTime End { get; set; }
		public String Duration { get; set; }
	}
}
