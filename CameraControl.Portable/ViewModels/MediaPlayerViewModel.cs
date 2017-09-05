using System;
using System.Linq;
using CameraControl.Portable.Messages;
using CameraControl.Portable.Models;
using CameraControl.Portable.Models.MediaPlayer;
using MvvmCross.Core.ViewModels;
using MvvmCross.Plugins.Messenger;

namespace CameraControl.Portable.ViewModels
{
    public class MediaPlayerViewModel : MvxViewModel
    {
        #region Events

        public event EventHandler<PlaybackModel> PlayRequest;
        public event EventHandler StopRequest;
        public event EventHandler DroppedFrameRequest;

        #endregion

        #region Fields

        private MvxCommand<int> _playCommand;
        private MvxCommand _stopCommand;
        private MvxCommand _restartCommand;
        private MvxCommand _startSeekCommand;
        private MvxCommand<int> _progressCommand;
        private MvxCommand _showInfoCommand;

        private bool _isProgressPaused;
        private StreamInfo _streamInfo;
        private bool _isPlaying;
        private ProgressResponseModel _progressResponse;

        #endregion

        #region Constructor

        public MediaPlayerViewModel(IMvxMessenger messenger, bool canReportProgress = false)
        {
            Messenger = messenger;
            CanReportProgress = canReportProgress;
        }

        #endregion

        #region Properties

        public IMvxMessenger Messenger { get; }

        public bool CanReportProgress { get; }

        public string ConnectionUrl { get; set; }

        public int DroppedFrame { get; set; }

        public string StreamName { get; set; }

        public long StartOffset { get; private set; }

        public long Duration { get; private set; }

        public int Progress { get; private set; }

        public bool IsPlaying
        {
            get { return _isPlaying; }
            set
            {
                if (_isPlaying == value)
                {
                    return;
                }

                _isPlaying = value;

                RaisePropertyChanged(() => IsPlaying);
                PlayCommand.RaiseCanExecuteChanged();
                StopCommand.RaiseCanExecuteChanged();
                RestartCommand.RaiseCanExecuteChanged();
                StartSeekCommand.RaiseCanExecuteChanged();
                StopSeekCommand.RaiseCanExecuteChanged();
            }
        }

        public ProgressResponseModel ProgressResponse
        {
            get { return _progressResponse; }
            set
            {
                _progressResponse = value;

                OnProgressResponseChanged(_progressResponse);
            }
        }

        public StreamInfo StreamInfo
        {
            get { return _streamInfo; }
            set
            {
                _streamInfo = value;
                
                RaisePropertyChanged(() => StreamInfo);
                ShowInfoCommand.RaiseCanExecuteChanged();
            }
        }

        #endregion

        #region Commands

        public MvxCommand<int> PlayCommand => _playCommand ?? (_playCommand = new MvxCommand<int>(SetPlay, CanSetPlay));

        public MvxCommand StopCommand => _stopCommand ?? (_stopCommand = new MvxCommand(SetStop, CanSetStop));

        public MvxCommand RestartCommand => _restartCommand ?? (_restartCommand = new MvxCommand(SetRestart, CanSetRestart));

        public MvxCommand StartSeekCommand => _startSeekCommand ?? (_startSeekCommand = new MvxCommand(StartSeek, CanStartSeek));

        public MvxCommand<int> StopSeekCommand => _progressCommand ?? (_progressCommand = new MvxCommand<int>(SetProgress, CanSetProgress));

        public MvxCommand ShowInfoCommand => _showInfoCommand ?? (_showInfoCommand = new MvxCommand(ShowInfo, CanShowInfo));

		public void SetStartPlayTime(DateTime startTime)
		{
			var span = DateTime.UtcNow.Subtract(startTime);
			if (Duration != 0 && StopSeekCommand.CanExecute())
			{
				var progress = ((double)(Duration - (long)span.TotalMilliseconds) / Duration) * 100;
				if (progress < 0 || progress > 100)
					return;
				Progress = (int)Math.Truncate(progress);

				StopSeekCommand.Execute(progress);
			}
		}

        #endregion

        #region Protected methods

        protected virtual void OnPlayRequest(PlaybackModel e)
        {
            PlayRequest?.Invoke(this, e);
        }

        protected virtual void OnStopRequest()
        {
            StopRequest?.Invoke(this, EventArgs.Empty);
        }

        protected virtual void OnDroppedFrameRequest()
        {
            DroppedFrameRequest?.Invoke(this, EventArgs.Empty);
        }

        #endregion

        #region Private methods

        private void SetPlay(int ffRate = 1000)
        {
            var playbackModel = new PlaybackModel(ConnectionUrl, ffRate);
            OnPlayRequest(playbackModel);
        }

        private bool CanSetPlay(int ffRate) => ConnectionUrl != null;

        private void SetStop()
        {
            OnStopRequest();
        }

        private bool CanSetStop() => IsPlaying;

        private void SetRestart()
        {
            var playbackModel = new PlaybackModel(ConnectionUrl, withRestart: true);
            OnPlayRequest(playbackModel);
        }

        private bool CanSetRestart() => ConnectionUrl != null;

        private void StartSeek()
        {
            _isProgressPaused = true;
        }

        private bool CanStartSeek() => IsPlaying;

		private void SetProgress(int progress)
        {
            StartOffset = (long) (Duration * ((double) progress / 100));

            var playbackModel = new PlaybackModel(ConnectionUrl, startOffset: StartOffset, withRestart: true);
            OnPlayRequest(playbackModel);

            _isProgressPaused = false;
        }

        private bool CanSetProgress(int progress) => IsPlaying && Duration != 0;

        private void ShowInfo()
        {
            var videoStream = StreamInfo.Stream.VideoStreams.First();
            OnDroppedFrameRequest();

            var streamInfoModel = new StreamInfoModel(videoStream)
            {
                DroppedFrame = DroppedFrame
            };

            var infoMessage = new MediaPlayerInfoMessage(this, streamInfoModel);
            Messenger.Publish(infoMessage);
        }

        private bool CanShowInfo() => StreamInfo?.Stream?.VideoStreams?.FirstOrDefault() != null;

        private void OnProgressResponseChanged(ProgressResponseModel progressResponseModel)
        {
            if (_isProgressPaused)
            {
                return;
            }

            Progress = (int) (100 * ((double) progressResponseModel.Current / progressResponseModel.Duration));
            Duration = progressResponseModel.Duration;

            // TODO: Crutch for possible 0 current position value on MediaPlayer GetLiveStreamPosition call.
            if (progressResponseModel.Current != 0)
            {
                RaisePropertyChanged(() => Progress);
            }
            StopSeekCommand.RaiseCanExecuteChanged();
        }

        #endregion
    }
}
