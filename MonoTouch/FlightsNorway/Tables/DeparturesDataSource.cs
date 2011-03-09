using System;
using MonoTouch.UIKit;
using MonoTouch.Foundation;

using FlightsNorway.Lib.Model;
using FlightsNorway.Lib.ViewModels;

using TinyIoC;

namespace FlightsNorway
{
	public class DeparturesDataSource : UITableViewDataSource
	{
		static NSString CellID = new NSString ("DepartureCell");		
		
		public FlightsViewModel ViewModel { get; private set; }
				
		private DeparturesTableViewController _controller;
		
		public DeparturesTableViewController Controller
		{
			set 
			{
				_controller = value;
				ViewModel.Departures.CollectionChanged += (o, e) => _controller.TableView.ReloadData();
			}
		}
				
		public DeparturesDataSource(FlightsViewModel viewModel)
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
        				
            cell.TextLabel.Text = ViewModel.Departures[indexPath.Row].ToString();				
            return cell;
        }
	}
}

