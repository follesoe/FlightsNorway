using System;
using MonoTouch.UIKit;
using MonoTouch.Foundation;

using FlightsNorway.Lib.Model;
using FlightsNorway.Lib.ViewModels;

namespace FlightsNorway
{
	public class ArrivalsDataSource : UITableViewDataSource
	{
		static NSString CellID = new NSString ("AirportCell");		
		
		public FlightsViewModel ViewModel { get; private set; }
		
		
		private ArrivalsTableViewController _controller;
		
		public ArrivalsTableViewController Controller
		{
			set 
			{
				_controller = value;
				ViewModel.Arrivals.CollectionChanged += (o, e) => _controller.TableView.ReloadData();
			}
		}
		
		public ArrivalsDataSource(FlightsViewModel viewModel)
		{
			ViewModel = viewModel;
		}
		
		public override int RowsInSection (UITableView tableView, int section)
		{
			return ViewModel.Arrivals.Count;
		}
		
		public override UITableViewCell GetCell (UITableView tableView, NSIndexPath indexPath)
        {
			var cell = tableView.DequeueReusableCell(CellID);
            if (cell == null)
            {					
                cell = new UITableViewCell(UITableViewCellStyle.Default, CellID);
            }
        				
            cell.TextLabel.Text = ViewModel.Arrivals[indexPath.Row].ToString();				
            return cell;
        }
	}
}