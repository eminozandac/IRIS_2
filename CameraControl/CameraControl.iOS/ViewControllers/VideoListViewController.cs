using System;

using UIKit;
using CoreAnimation;
using Foundation;

namespace CameraControl.iOS.ViewControllers
{
	public partial class VideoListViewController : BasicViewController
    {
		protected VideoListViewController(IntPtr handle) : base(handle)
        {
			// Note: this .ctor should not contain any initialization logic.
		}

		public override void ViewDidLoad()
        {
            base.ViewDidLoad();
			// Perform any additional setup after loading the view, typically from a nib.
			CALayer headerBorder = new CALayer();
			headerBorder.Frame = new CoreGraphics.CGRect(0.0f, 63.0f, UIScreen.MainScreen.Bounds.Width, 1.0f);
			headerBorder.BackgroundColor = UIColor.LightGray.CGColor;
			headerView.Layer.AddSublayer(headerBorder);

            tbVideoList.Source = new TableSource(null, this);
        }

        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();
            // Release any cached data, images, etc that aren't in use.
        }

		partial void OnTouchMenu(Foundation.NSObject sender)
		{
			dropDownMenuView.Hidden = !dropDownMenuView.Hidden;
		}
    }

	public class TableSource : UITableViewSource
	{

		protected string[] tableItems;
		protected string cellIdentifier = "PlayVideoTableViewCell";
		VideoListViewController owner;

		public TableSource(string[] items, VideoListViewController owner)
		{
			tableItems = items;
			this.owner = owner;

		}

		/// <summary>
		/// Called by the TableView to determine how many sections(groups) there are.
		/// </summary>
		public override nint NumberOfSections(UITableView tableView)
		{
			return 1;
		}

		public override nfloat GetHeightForRow(UITableView tableView, NSIndexPath indexPath)
		{
			return 44.0f;
		}

		/// <summary>
		/// Called by the TableView to determine how many cells to create for that particular section.
		/// </summary>
		public override nint RowsInSection(UITableView tableview, nint section)
		{
			//return tableItems.Length;
            return 4;
		}

		/// <summary>
		/// Called when a row is touched
		/// </summary>
		public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
		{
			UIAlertController okAlertController = UIAlertController.Create("Row Selected", tableItems[indexPath.Row], UIAlertControllerStyle.Alert);
			okAlertController.AddAction(UIAlertAction.Create("OK", UIAlertActionStyle.Default, null));
			owner.PresentViewController(okAlertController, true, null);

			tableView.DeselectRow(indexPath, true);
		}

		/// <summary>
		/// Called by the TableView to get the actual UITableViewCell to render for the particular row
		/// </summary>
		public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
		{
            PlayVideoTableViewCell cell = (PlayVideoTableViewCell)tableView.DequeueReusableCell(cellIdentifier);
            //string item = tableItems[indexPath.Row];

            //---- if there are no cells to reuse, create a new one
            //if (cell == null)
            //{ 
            //    cell = new PlayVideoTableViewCell(UITableViewCellStyle.Default, cellIdentifier);
            //}

            //cell.TextLabel.Text = item;
            if (indexPath.Row == 0)
            {
                cell.GetCameraLabel().BackgroundColor = UIColor.LightGray;
                cell.GetCameraLabel().Layer.CornerRadius = 5.0f;
                cell.GetDateTimeLabel().BackgroundColor = UIColor.LightGray;
                cell.GetDateTimeLabel().Layer.CornerRadius = 5.0f;

                cell.GetDurationLabel().BackgroundColor = UIColor.LightGray;
                cell.GetDurationLabel().Layer.CornerRadius = 5.0f;
                cell.GetViewLabel().BackgroundColor = UIColor.LightGray;
				cell.GetViewLabel().Layer.CornerRadius = 5.0f;
            }

			return cell;
		}

	}
}

