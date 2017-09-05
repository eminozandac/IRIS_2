using System.Collections.Generic;
using Android.App;
using Android.Content.PM;
using Android.Views;
using CameraControl.Portable.ViewModels;
using Veg.Mediaplayer.Sdk;
using Toolbar = Android.Support.V7.Widget.Toolbar;

namespace CameraControl.Droid.Views
{
    [Activity(ScreenOrientation = ScreenOrientation.Portrait)]
    public class QuadActivity : BaseAppCompatActivity<QuadViewModel>
    {
        #region Fields

        private readonly List<MediaPlayer> _mediaPlayers = new List<MediaPlayer>();

        #endregion

        #region Viewmodel lifecycle

        protected override void OnViewModelSet()
        {
            base.OnViewModelSet();

            SetContentView(Resource.Layout.Quad);

            var quadToolbar = FindViewById<Toolbar>(Resource.Id.quadToolbar);
            SetSupportActionBar(quadToolbar);

            _mediaPlayers.AddRange(new[]
            {
                FindViewById<MediaPlayer>(Resource.Id.playerOneView),
                FindViewById<MediaPlayer>(Resource.Id.playerTwoView),
                FindViewById<MediaPlayer>(Resource.Id.playerThreeView),
                FindViewById<MediaPlayer>(Resource.Id.playerFourView)
            });
        }

        #endregion

        #region Activity lifecycle

        protected override void OnPause()
        {
            base.OnPause();

            foreach (var mediaPlayer in _mediaPlayers)
            {
                mediaPlayer.OnPause();
            }
        }

        protected override void OnResume()
        {
            base.OnResume();

            foreach (var mediaPlayer in _mediaPlayers)
            {
                mediaPlayer.OnResume();
                foreach (var quadCellViewModel in ViewModel.QuadCellViewModels)
                {
                    if (!quadCellViewModel.MediaPlayerViewModel.RestartCommand.CanExecute())
                    {
                        continue;
                    }

                    quadCellViewModel.MediaPlayerViewModel.RestartCommand.Execute();
                }
            }
        }

        protected override void OnStop()
        {
            base.OnStop();

            foreach (var mediaPlayer in _mediaPlayers)
            {
                mediaPlayer.Close();
            }
        }

        public override void OnWindowFocusChanged(bool hasFocus)
        {
            base.OnWindowFocusChanged(hasFocus);

            foreach (var mediaPlayer in _mediaPlayers)
            {
                mediaPlayer.OnWindowFocusChanged(hasFocus);
            }
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            foreach (var mediaPlayer in _mediaPlayers)
            {
                mediaPlayer.Close();
            }
        }

        #endregion

        #region Options menu

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.Navigation, menu);

            // TODO: Temp solution, need to find out how to switch back to playback view correctly.
            var playbackMenuItem = menu.FindItem(Resource.Id.playbackAction);
            playbackMenuItem.SetEnabled(false);

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
                    return true;
                default:
                    return base.OnOptionsItemSelected(item);
            }
        }

        #endregion
    }
}
