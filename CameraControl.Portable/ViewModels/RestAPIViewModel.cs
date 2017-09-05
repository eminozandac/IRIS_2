using System;
using CameraControl.Portable.ViewModels;
using MvvmCross.Plugins.Messenger;
using MvvmCross.Platform.Platform;
using System.Collections.Generic;
using CameraControl.Portable.RestAPI;
using MvvmCross.Platform;

namespace CameraControl.Portable.ViewModels
{
	public class RestAPIViewModel : BaseViewModel
	{
		private IIrisRestAPI RestAPIInstance;

		public RestAPIViewModel(IMvxMessenger messenger, IMvxJsonConverter jsonConverter) : base(messenger, jsonConverter)
		{
			RestAPIInstance = Mvx.Resolve<IIrisRestAPI>();
		}

		public void InitRestAPI()
		{ 
			RestAPIInstance.InitCertificate();
			RestAPIInstance.UrlAPI = "https://iris00a.irisbypbe.com:5003";
			RestAPIInstance.Login("admin", "admin");
		}

		public List<PlaybackItem> Sequences
		{
			get
			{
				var list = RestAPIInstance.GetSequences(new PlaybackFilter()
				{
					End = DateTime.UtcNow,
					Start = DateTime.UtcNow.Subtract(TimeSpan.FromDays(7)),
					Camera = CameraName
				});
				return list;

				return new List<PlaybackItem>
				{
					new PlaybackItem() {
						Duration="24 hours",
						Start=DateTime.UtcNow.Subtract(TimeSpan.FromDays(1)),
						End=DateTime.Now },
					new PlaybackItem()
					{
						Duration = "48 hours",
						Start = DateTime.UtcNow.Subtract(TimeSpan.FromDays(2)),
						End = DateTime.Now
					},
					new PlaybackItem()
					{
						Duration = "72 hours",
						Start = DateTime.UtcNow.Subtract(TimeSpan.FromDays(3)),
						End = DateTime.Now
					}

				};
			}
		}

     	public string CameraName { get; set; }

		private PlaybackItem _selectedPlaybackItem;
		public PlaybackItem SelectedPlaybackItem
		{
			get { return _selectedPlaybackItem; }

			set
			{
				_selectedPlaybackItem = value;
				RaisePropertyChanged(() => SelectedPlaybackItem);

				StartTime = SelectedPlaybackItem.Start.ToString("MM/dd HH:mm");
				EndTime = SelectedPlaybackItem.End.ToString("MM/dd HH:mm");

				OnSelectedPlaybackStartsEvent(new PlaybackItemEventArgs(SelectedPlaybackItem.Start));
			}
		}

		public event EventHandler<PlaybackItemEventArgs> SelectedPlaybackStartsEvent;

		private void OnSelectedPlaybackStartsEvent(PlaybackItemEventArgs args)
		{
			var invocator = SelectedPlaybackStartsEvent;
			invocator?.Invoke(this, args);
		}

		private string _startTime;
		public string StartTime
		{
			get { return _startTime; }
			set
			{
				_startTime = value;
				RaisePropertyChanged(() => StartTime);
			}
		}

		private string _endTime;
		public string EndTime
		{
			get { return _endTime; }
			set
			{
				_endTime = value;
				RaisePropertyChanged(() => EndTime);
			}
		}
	}
}
