﻿using Android.Util;
using CameraControl.Portable.Models;
using CameraControl.Portable.Models.MediaPlayer;
using CameraControl.Portable.Utils;
using CameraControl.Portable.ViewModels;
using Java.Nio;
using MvvmCross.Binding;
using MvvmCross.Binding.Droid.Target;
using MvvmCross.Platform.WeakSubscription;
using System;
using System.Threading;
using Veg.Mediaplayer.Sdk;
using Object = Java.Lang.Object;

namespace CameraControl.Droid.Bindings
{
    public class MediaPlayerViewModelBinding : MvxAndroidTargetBinding
    {
        private const string MediaPlayerCallbackTag = "MediaPlayerCallback";
        private const int ProgressTimerStep = 1500;

        private IDisposable _droppedFrameRequestSubscription;
        private IDisposable _playRequestSubscription;
        private IDisposable _stopRequestSubscription;
        private Timer _progressTimer;
        private bool _isPlaying;

        public MediaPlayerViewModelBinding(MediaPlayer mediaPlayer) : base(mediaPlayer)
        {
        }

        public MediaPlayerViewModel MediaPlayerViewModel { get; private set; }

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
                OnIsPlayingChanged(IsPlaying);
            }
        }

        public override Type TargetType => typeof(MediaPlayerViewModel);

        public override MvxBindingMode DefaultMode => MvxBindingMode.OneTime;

        protected MediaPlayer MediaPlayer => (MediaPlayer)Target;

        protected override void SetValueImpl(object target, object value)
        {
            _droppedFrameRequestSubscription?.Dispose();
            _droppedFrameRequestSubscription = null;

            _playRequestSubscription?.Dispose();
            _playRequestSubscription = null;

            _stopRequestSubscription?.Dispose();
            _stopRequestSubscription = null;

            _progressTimer?.Dispose();
            _progressTimer = null;

            MediaPlayerViewModel = value as MediaPlayerViewModel;
            if (MediaPlayerViewModel == null)
            {
                return;
            }

            if (MediaPlayerViewModel.CanReportProgress)
            {
                _progressTimer = new Timer(OnProgressTimer, null, ProgressTimerStep, Timeout.Infinite);
            }

            _stopRequestSubscription = MediaPlayerViewModel.WeakSubscribe(nameof(MediaPlayerViewModel.StopRequest), StopRequestHandler);
            _playRequestSubscription = MediaPlayerViewModel.WeakSubscribe<MediaPlayerViewModel, PlaybackModel>(nameof(MediaPlayerViewModel.PlayRequest), PlayRequestHandler);
            _droppedFrameRequestSubscription = MediaPlayerViewModel.WeakSubscribe(nameof(MediaPlayerViewModel.DroppedFrameRequest), DroppedFrameRequestHandler);
        }


        protected override void Dispose(bool isDisposing)
        {
            if (isDisposing)
            {
                _droppedFrameRequestSubscription?.Dispose();
                _droppedFrameRequestSubscription = null;

                _playRequestSubscription?.Dispose();
                _playRequestSubscription = null;

                _stopRequestSubscription?.Dispose();
                _stopRequestSubscription = null;

                _progressTimer?.Dispose();
                _progressTimer = null;
            }
            base.Dispose(isDisposing);
        }

        private void PlayRequestHandler(object sender, PlaybackModel playbackModel)
        {
            if (playbackModel.WithRestart)
            {
                MediaPlayer.Close();
            }

            MediaPlayer.Open(new MediaPlayerConfig
            {
                ConnectionUrl = playbackModel.ConnectionUrl,
                ConnectionNetworkProtocol = -1,
                ConnectionBufferingTime = 100,
                DecodingType = 1,
                StartOffest = playbackModel.StartOffset
            }, new MediaPlayerCallback(this));

            // TODO: Check if this works.
            MediaPlayer.SetFFRate(playbackModel.FFRate);
        }

        private void StopRequestHandler(object sender, EventArgs eventArgs)
        {
            MediaPlayer.Close();
        }

        private void DroppedFrameRequestHandler(object sender, EventArgs eventArgs)
        {
            MediaPlayerViewModel.DroppedFrame = MediaPlayer?.DroppedFrame ?? 0;
        }

        private void OnProgressTimer(object state)
        {
            if (IsPlaying && MediaPlayer.LiveStreamPosition != null)
            {
                var position = MediaPlayer.LiveStreamPosition;

                var progressResponseModel = new ProgressResponseModel(position.Current, position.Duration);
                MediaPlayerViewModel.ProgressResponse = progressResponseModel;
            }

            _progressTimer.Change(ProgressTimerStep, Timeout.Infinite); 
        }

        private void OnIsPlayingChanged(bool isPlaying)
        {
            MediaPlayerViewModel.IsPlaying = isPlaying;

            if (isPlaying)
            {
                var streamInfoXml = MediaPlayer.StreamInfo;
                try
                {
                    var streamInfo = streamInfoXml.FromXml<StreamInfo>();
                    MediaPlayerViewModel.StreamInfo = streamInfo;
                }
                catch (Exception ex)
                {
                    // TODO: Log this exception.
                }
            }
            else
            {
                MediaPlayerViewModel.StreamInfo = null;
            }
        }

        private class MediaPlayerCallback : Object, MediaPlayer.IMediaPlayerCallback
        {
            private readonly MediaPlayerViewModelBinding _viewModelBinding;

            public MediaPlayerCallback(MediaPlayerViewModelBinding viewModelBinding)
            {
                _viewModelBinding = viewModelBinding;
            }

            public int OnReceiveData(ByteBuffer p0, int p1, long p2)
            {
                return 0;
            }

            public int Status(int status)
            {
                // TODO: IsBusy property processing.
                // TODO: Reset position on ptz action.

                if (status == MediaPlayer.PlayerNotifyCodes.CpConnectStarting.Ordinal())
                {
                    Log.Debug(MediaPlayerCallbackTag, "CpConnectStarting");
                }
                else if (status == MediaPlayer.PlayerNotifyCodes.PlpBuildSuccessful.Ordinal())
                {
                    var text = _viewModelBinding.MediaPlayer.GetPropString(MediaPlayer.PlayerProperties.PpPropertyPlpResponseText);
                    var code = _viewModelBinding.MediaPlayer.GetPropString(MediaPlayer.PlayerProperties.PpPropertyPlpResponseCode);
                    Log.Debug(MediaPlayerCallbackTag, $"PlpBuildSuccessful: Response sText={text} sCode={code}");
                }
                else if (status == MediaPlayer.PlayerNotifyCodes.VrpNeedSurface.Ordinal())
                {
                    Log.Debug(MediaPlayerCallbackTag, "VrpNeedSurface");
                }
                else if (status == MediaPlayer.PlayerNotifyCodes.PlpPlaySuccessful.Ordinal())
                {
                    _viewModelBinding.IsPlaying = true;
                    Log.Debug(MediaPlayerCallbackTag, "PlpPlaySuccessful");
                }
                else if (status == MediaPlayer.PlayerNotifyCodes.PlpCloseStarting.Ordinal())
                {
                    Log.Debug(MediaPlayerCallbackTag, "PlpCloseStarting");
                }
                else if (status == MediaPlayer.PlayerNotifyCodes.PlpCloseSuccessful.Ordinal())
                {
                    _viewModelBinding.IsPlaying = false;
                    Log.Debug(MediaPlayerCallbackTag, "PlpCloseSuccessful");
                }
                else if (status == MediaPlayer.PlayerNotifyCodes.PlpCloseFailed.Ordinal())
                {
                    Log.Debug(MediaPlayerCallbackTag, "PlpCloseFailed");
                }
                else if (status == MediaPlayer.PlayerNotifyCodes.CpConnectFailed.Ordinal())
                {
                    Log.Debug(MediaPlayerCallbackTag, "CpConnectFailed");
                }
                else if (status == MediaPlayer.PlayerNotifyCodes.PlpBuildFailed.Ordinal())
                {
                    var text = _viewModelBinding.MediaPlayer.GetPropString(MediaPlayer.PlayerProperties.PpPropertyPlpResponseText);
                    var code = _viewModelBinding.MediaPlayer.GetPropString(MediaPlayer.PlayerProperties.PpPropertyPlpResponseCode);
                    Log.Debug(MediaPlayerCallbackTag, $"PlpBuildFailed: Response sText={text} sCode={code}");
                }
                else if (status == MediaPlayer.PlayerNotifyCodes.PlpPlayFailed.Ordinal())
                {
                    Log.Debug(MediaPlayerCallbackTag, "PlpPlayFailed");
                }
                else if (status == MediaPlayer.PlayerNotifyCodes.PlpError.Ordinal())
                {
                    Log.Debug(MediaPlayerCallbackTag, "PlpError");
                }
                else if (status == MediaPlayer.PlayerNotifyCodes.CpInterrupted.Ordinal())
                {
                    Log.Debug(MediaPlayerCallbackTag, "CpInterrupted");
                }
                else if (status == MediaPlayer.PlayerNotifyCodes.CpInterrupted.Ordinal())
                {
                    Log.Debug(MediaPlayerCallbackTag, "CpInterrupted");
                }
                else if (status == MediaPlayer.PlayerNotifyCodes.CpStopped.Ordinal() ||
                         status == MediaPlayer.PlayerNotifyCodes.VdpStopped.Ordinal() ||
                         status == MediaPlayer.PlayerNotifyCodes.VrpStopped.Ordinal() ||
                         status == MediaPlayer.PlayerNotifyCodes.AdpStopped.Ordinal() ||
                         status == MediaPlayer.PlayerNotifyCodes.ArpStopped.Ordinal())

                {
                    _viewModelBinding.MediaPlayer.Stop();
                    _viewModelBinding.IsPlaying = false;
                    Log.Debug(MediaPlayerCallbackTag, "Stopped");
                }
                else if (status == MediaPlayer.PlayerNotifyCodes.CpErrorNodataTimeout.Ordinal())
                {
                    Log.Debug(MediaPlayerCallbackTag, "CpErrorNodataTimeout");
                }
                else if (status == MediaPlayer.PlayerNotifyCodes.PlpTrialVersion.Ordinal())
                {
                    _viewModelBinding.MediaPlayer.Stop();
                    _viewModelBinding.IsPlaying = false;
                    Log.Debug(MediaPlayerCallbackTag, "PlpTrialVersion");
                }
                else if (status == MediaPlayer.PlayerNotifyCodes.PlpEos.Ordinal())
                {
                    _viewModelBinding.MediaPlayer.Stop();
                    _viewModelBinding.IsPlaying = false;
                    Log.Debug(MediaPlayerCallbackTag, "PlpEos");
                }
                else if (status == MediaPlayer.PlayerNotifyCodes.CpErrorDisconnected.Ordinal())

                {
                    _viewModelBinding.MediaPlayer.Stop();
                    _viewModelBinding.IsPlaying = false;
                    Log.Debug(MediaPlayerCallbackTag, "CpErrorDisconnected");
                }

                return 0;
            }
        }
    }
}
