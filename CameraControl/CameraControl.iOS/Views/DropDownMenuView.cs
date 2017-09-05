using System;
using System.Drawing;
using ObjCRuntime;
using CoreGraphics;
using Foundation;
using UIKit;
using System.Collections.Generic;
using System.Linq;

namespace CameraControl.iOS.Views
{
    [Register("DropDownMenuView")]
    public partial class DropDownMenuView : UIView
    {
        
        UITableView menuTable;

        public DropDownMenuView()
        {

        }

        public DropDownMenuView(RectangleF bounds) : base(bounds)
        {
        }

        public DropDownMenuView(BasicViewController theOwner, List<string> theMenuItems)
        {

            CGRect ownerFrame = UIScreen.MainScreen.Bounds;
           
            Frame = new CGRect(ownerFrame.Width - 201.0f, 44.0f, 200.0f, 44.0f * theMenuItems.Count);

            menuTable = new UITableView();
            menuTable.Frame = new CGRect(0, 0, Frame.Width, Frame.Height);
            menuTable.BackgroundColor = UIColor.Black;


            menuTable.Source = new TableSource(theMenuItems.ToArray(), theOwner);
            //dropDownMenu.owner = theOwner;
            //dropDownMenu.menuItems = theMenuItems;

            AddSubview(menuTable);
            theOwner.View.AddSubview(this);

        }

    }

	public class TableSource : UITableViewSource
	{
		protected string[] tableItems;
		protected string cellIdentifier = "TableCell";
		BasicViewController owner;

        public TableSource(string[] items, BasicViewController owner)
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
			return tableItems.Length;
		}

		/// <summary>
		/// Called when a row is touched
		/// </summary>
		public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
		{
            if (indexPath.Row == tableItems.Count() - 1)
                return;
            
            BasicViewController goViewController = owner.viewControllerManager.viewControllers.ElementAt(indexPath.Row);
            
            if (goViewController != owner) {
				if (goViewController == owner.viewControllerManager.rootViewController)
					owner.viewControllerManager.rootViewController.DismissViewController(false, null);
                else if (owner != owner.viewControllerManager.rootViewController)
                    owner.DismissViewController(false, null);

                
                owner.PresentViewController(goViewController, true, null);
            }
                
            
			tableView.DeselectRow(indexPath, true);
		}

		/// <summary>
		/// Called by the TableView to get the actual UITableViewCell to render for the particular row
		/// </summary>
		public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
		{
			UITableViewCell cell = tableView.DequeueReusableCell(cellIdentifier);
			string item = tableItems[indexPath.Row];

			//---- if there are no cells to reuse, create a new one
			if (cell == null)
			{ cell = new UITableViewCell(UITableViewCellStyle.Default, cellIdentifier); }

			cell.TextLabel.Text = item;
            cell.TextLabel.TextColor = UIColor.White;
            cell.BackgroundColor = UIColor.Black;

			return cell;
		}
	}
}
