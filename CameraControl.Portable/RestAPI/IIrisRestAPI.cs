using System;
using System.Collections.Generic;

namespace CameraControl.Portable.RestAPI
{
	public interface IIrisRestAPI
	{
		String UrlAPI { get; set; }
		TimeSpan Timeout { get; }
		string Token { get; }
		void InitCertificate();
		bool Login(String userName, String password);
		List<Camera> GetCamerasList();
		List<PlaybackItem> GetSequences(PlaybackFilter filter);
	}

	
}