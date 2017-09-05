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
using Android.Support.V7.Widget;
using Android.Content.PM;

namespace CameraControl.Droid.Views
{
    [Activity(Label = "PlaybackListActivity", ScreenOrientation = ScreenOrientation.Landscape)]
    public class PlaybackListActivity : Activity
    {
        // RecyclerView instance that displays the photo album:
        RecyclerView mRecyclerView;

        // Layout manager that lays out each card in the RecyclerView:
        RecyclerView.LayoutManager mLayoutManager;

        // Adapter that accesses the data set (a photo album):
        PlaybackListAdapter mAdapter;


        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
            SetContentView(Resource.Layout.ActivityPlayback);
            mRecyclerView = FindViewById<RecyclerView>(Resource.Id.playback_list);

            mLayoutManager = new LinearLayoutManager(this);
            // Plug the layout manager into the RecyclerView:
            mRecyclerView.SetLayoutManager(mLayoutManager);

            mAdapter = new PlaybackListAdapter(null);
            mRecyclerView.SetAdapter(mAdapter);

        }
    }
}