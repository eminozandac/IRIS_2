using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Views;
using CameraControl.Portable.Messages;
using CameraControl.Portable.ViewModels;
using MvvmCross.Plugins.Messenger;
using Veg.Mediaplayer.Sdk;
using Toolbar = Android.Support.V7.Widget.Toolbar;

namespace CameraControl.Droid.Views
{
    [Activity(ScreenOrientation = ScreenOrientation.Portrait)]
    public class LiveActivity : BaseAppCompatActivity<LiveViewModel>
    {
        #region Fields

        private MediaPlayer _mediaPlayer;
        private MvxSubscriptionToken _infoSubscriptionToken;

        #endregion

        #region ViewModel lifecycle

        protected override void OnViewModelSet()
        {
            base.OnViewModelSet();

            SetContentView(Resource.Layout.Live);

            var liveToolbar = FindViewById<Toolbar>(Resource.Id.liveToolbar);
            SetSupportActionBar(liveToolbar);

            _mediaPlayer = FindViewById<MediaPlayer>(Resource.Id.playerView);
        }

        #endregion

        #region Activity lifecycle

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            _infoSubscriptionToken = ViewModel.Messenger.SubscribeOnMainThread<MediaPlayerInfoMessage>(OnMediaPlayerInfoMessage, MvxReference.Strong);
        }

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
            ViewModel.Messenger.Unsubscribe<MediaPlayerInfoMessage>(_infoSubscriptionToken);
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
                    return true;
                case Resource.Id.playbackAction:
                    if (ViewModel.ShowPlaybackCommand.CanExecute())
                    {
                        ViewModel.ShowPlaybackCommand.Execute();
                    }
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

        #region Private methods

        private void OnMediaPlayerInfoMessage(MediaPlayerInfoMessage infoMessage)
        {
            if (infoMessage.Sender != ViewModel.MediaPlayerViewModel)
            {
                return;
            }

            var dialogFragment = new StreamInfoFragment
            {
                Title = ViewModel.SelectedProfileModel?.Name,
                DataContext = infoMessage.StreamInfoModel
            };
            dialogFragment.Show(SupportFragmentManager, "info");
        }

        #endregion
    }
}
