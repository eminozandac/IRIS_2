using System;
using System.Collections.Generic;

namespace CameraControl.Portable.RestAPI
{

	public class PlaybackItemEventArgs : EventArgs
	{
		public PlaybackItemEventArgs(DateTime startTime)
		{
			StartTime = startTime;
		}

		public DateTime StartTime { get; set; }
	}
}