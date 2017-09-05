using System;
namespace CameraControl.Portable.RestAPI
{
	public class PlaybackFilter
	{
		public string Camera { get; set; }
		public DateTime Start { get; set; }
		public DateTime End { get; set; }
	}
}
