using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.OS;
using Android.Widget;
using Android.Content.PM;
using Android.Content;

namespace CameraControl.Droid.Views
{
    //[Activity(Label = "LoginActivity")]
    [Activity(Label = "Iris Mobile", MainLauncher = true, ScreenOrientation = ScreenOrientation.Landscape)]
    public class LoginActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
            //SetContentView(Resource.Layout.Login);
            SetContentView(Resource.Layout.Login);


            TextView connectButton = FindViewById<TextView>(Resource.Id.connect_button);
            connectButton.Click += OnClickedConnect;
        }

        private void OnClickedConnect(object sender, EventArgs e)
        {
            Intent intent = new Intent(this.ApplicationContext, typeof(LiveViewActivity));
            intent.SetFlags(ActivityFlags.NewTask);
            StartActivity(intent);
        }

    }
}