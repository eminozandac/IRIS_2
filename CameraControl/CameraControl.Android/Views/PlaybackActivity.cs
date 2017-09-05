using Android.App;
using Android.Content.PM;
using Android.Views;
using CameraControl.Portable.ViewModels;
using Veg.Mediaplayer.Sdk;
using Toolbar = Android.Support.V7.Widget.Toolbar;

namespace CameraControl.Droid.Views
{
    [Activity(ScreenOrientation = ScreenOrientation.Portrait)]
    public class PlaybackActivity : BaseAppCompatActivity<PlaybackViewModel>
    {
        #region Fields

        private MediaPlayer _mediaPlayer;

        #endregion

        #region Viewmodel lifecycle

        protected override void OnViewModelSet()
        {
            base.OnViewModelSet();

            SetContentView(Resource.Layout.Playback);

            var playbackToolbar = FindViewById<Toolbar>(Resource.Id.playbackToolbar);
            SetSupportActionBar(playbackToolbar);

            _mediaPlayer = FindViewById<MediaPlayer>(Resource.Id.playerView);
        }

        #endregion

        #region Activity lifecycle

        protected override void OnPause()
        {
            base.OnPause();

            _mediaPlayer?.OnPause();
        }

        protected override void OnResume()
        {
            base.OnResume();

            if (_mediaPlayer == null)
            {
                return;
            }

            _mediaPlayer.OnResume();
            if (ViewModel.MediaPlayerViewModel.RestartCommand.CanExecute())
            {
                ViewModel.MediaPlayerViewModel.RestartCommand.Execute();
            }
        }

        protected override void OnStop()
        {
            base.OnStop();

            _mediaPlayer?.Close();
        }

        public override void OnWindowFocusChanged(bool hasFocus)
        {
            base.OnWindowFocusChanged(hasFocus);

            _mediaPlayer?.OnWindowFocusChanged(hasFocus);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            _mediaPlayer?.Close();
        }

        #endregion

        #region Options menu

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.Navigation, menu);
            return base.OnCreateOptionsMenu(menu);
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Resource.Id.liveAction:
                    if (ViewModel.ShowLiveCommand.CanExecute())
                    {
                        ViewModel.ShowLiveCommand.Execute();
                    }
                    return true;
                case Resource.Id.playbackAction:
                    return true;
                case Resource.Id.quadAction:
                    if (ViewModel.ShowQuadCommand.CanExecute())
                    {
                        ViewModel.ShowQuadCommand.Execute();
                    }
                    return true;
                default:
                    return base.OnOptionsItemSelected(item);
            }
        }

        #endregion
    }
}
