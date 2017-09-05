using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Content.PM;

namespace CameraControl.Droid.Views
{
    [Activity(Label = "LiveViewActivity", ScreenOrientation = ScreenOrientation.Landscape)]
    public class LiveViewActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.ActivityLiveView);

            // Create your application here
            ImageView menuButton = FindViewById<ImageView>(Resource.Id.menu_button);
            menuButton.Click += (s, arg) =>
            {

                PopupMenu menu = new PopupMenu(this, menuButton);

                // with Android 3 need to use MenuInfater to inflate the menu
                //menu.MenuInflater.Inflate (Resource.Menu.popup_menu, menu.Menu);

                // with Android 4 Inflate can be called directly on the menu
                menu.Inflate(Resource.Menu.popup_menu);

                menu.MenuItemClick += (s1, arg1) =>
                {
                    Console.WriteLine("{0} selected", arg1.Item.TitleFormatted);
                    switch (arg1.Item.TitleFormatted.ToString())
                    {
                        case "Playback Videos":
                            //Edit action here
                            Intent intent = new Intent(this.ApplicationContext, typeof(PlaybackListActivity));
                            StartActivity(intent);
                            break;
                        case "Settings":
                            //Edit action here
                            Intent settingIntent = new Intent(this.ApplicationContext, typeof(SettingsActivity));
                            StartActivity(settingIntent);
                            break;
                        case "Status":
                            //Edit action here
                            Intent statusIntent = new Intent(this.ApplicationContext, typeof(StatusActivity));
                            StartActivity(statusIntent);
                            break;
                    }
                };

                // Android 4 now has the DismissEvent
                menu.DismissEvent += (s2, arg2) =>
                {
                    Console.WriteLine("menu dismissed");
                };

                menu.Show();
            };
        }


    }
}