using System;

using Android.Views;
using Android.Widget;
using Android.Support.V7.Widget;

namespace CameraControl.Droid.Views
{
    class PlaybackListAdapter : RecyclerView.Adapter
    {
        public event EventHandler<PlaybackListAdapterClickEventArgs> ItemClick;
        public event EventHandler<PlaybackListAdapterClickEventArgs> ItemLongClick;
        string[] items;

        public PlaybackListAdapter(string[] data)
        {
            items = data;
        }

        // Create new views (invoked by the layout manager)
        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {

            //Setup your layout here
            View itemView = null;
            var id = Resource.Layout.PlaybackListItem;
            itemView = LayoutInflater.From(parent.Context).Inflate(id, parent, false);

            var vh = new PlaybackListAdapterViewHolder(itemView, OnClick, OnLongClick);
            return vh;
        }

        // Replace the contents of a view (invoked by the layout manager)
        public override void OnBindViewHolder(RecyclerView.ViewHolder viewHolder, int position)
        {
            //var item = items[position];

            // Replace the contents of the view with that element
            PlaybackListAdapterViewHolder holder = viewHolder as PlaybackListAdapterViewHolder;
            //holder.TextView.Text = items[position];
            if (position == 0)
            {
                holder.CameraName.SetBackgroundResource(Resource.Drawable.button_round_fill_gray);
                holder.DateTime.SetBackgroundResource(Resource.Drawable.button_round_fill_gray);
                holder.Duration.SetBackgroundResource(Resource.Drawable.button_round_fill_gray);
                holder.View.SetBackgroundResource(Resource.Drawable.button_round_fill_gray);
            }
            else
            {
                holder.CameraName.Text = "Driver";
                holder.DateTime.Text = "03/27/2016 06:27:00 PM";
                holder.Duration.Text = "00:02:00";
                holder.View.Text = "View";
            }
                
        }

        //public override int ItemCount => items.Length;
        //public override int ItemCount => 4;
        // Return the number of photos available in the photo album:
        public override int ItemCount
        {
            get { return 4; }
        }

        void OnClick(PlaybackListAdapterClickEventArgs args) => ItemClick?.Invoke(this, args);
        void OnLongClick(PlaybackListAdapterClickEventArgs args) => ItemLongClick?.Invoke(this, args);

    }

    public class PlaybackListAdapterViewHolder : RecyclerView.ViewHolder
    {
        public TextView CameraName { get; set; }
        public TextView DateTime { get; set; }
        public TextView Duration { get; set; }
        public TextView View { get; set; }

        public PlaybackListAdapterViewHolder(View itemView, Action<PlaybackListAdapterClickEventArgs> clickListener,
                            Action<PlaybackListAdapterClickEventArgs> longClickListener) : base(itemView)
        {
            //TextView = v;
            CameraName = itemView.FindViewById<TextView>(Resource.Id.camera_field);
            DateTime = itemView.FindViewById<TextView>(Resource.Id.date_time_field);
            Duration = itemView.FindViewById<TextView>(Resource.Id.duration_field);
            View = itemView.FindViewById<TextView>(Resource.Id.view_field);

            itemView.Click += (sender, e) => clickListener(new PlaybackListAdapterClickEventArgs { View = itemView, Position = AdapterPosition });
            itemView.LongClick += (sender, e) => longClickListener(new PlaybackListAdapterClickEventArgs { View = itemView, Position = AdapterPosition });
        }
    }

    public class PlaybackListAdapterClickEventArgs : EventArgs
    {
        public View View { get; set; }
        public int Position { get; set; }
    }


}