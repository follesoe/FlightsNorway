using System;
using MonoTouch.UIKit;
using FlightsNorway.Lib.DataServices;
using FlightsNorway.Lib.ViewModels;
using MonoTouch.Foundation;
using FlightsNorway.Tables;

namespace FlightsNorway
{
	public class AirportsDataSource : UITableViewDataSource
	{	
		static NSString CellID = new NSString ("AirportCell");		
		private readonly AirportsViewModel _viewModel;		
		private AirportsTableViewController _controller;
		
		public AirportsTableViewController Controller
		{
			set 
			{
				_controller = value;
				_viewModel.Airports.CollectionChanged += (o, e) => _controller.TableView.ReloadData();
			}
		}
							
		public AirportsDataSource(AirportsViewModel viewModel)
		{
			_viewModel = viewModel;			
		}
		
		public override int RowsInSection(UITableView tableView, int section)
		{
			return _viewModel.Airports.Count;
		}	
		  
		public override UITableViewCell GetCell (UITableView tableView, NSIndexPath indexPath)
        {
			var cell = tableView.DequeueReusableCell(CellID);
            if (cell == null)
            {					
                cell = new UITableViewCell(UITableViewCellStyle.Default, CellID);
            }
        				
            cell.TextLabel.Text = _viewModel.Airports[indexPath.Row].ToString();				
            return cell;
        }
	}
}