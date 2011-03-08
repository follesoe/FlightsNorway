using System;
using System.Collections.Generic;

using MonoTouch.UIKit;

using FlightsNorway.Lib.Model;
using FlightsNorway.Lib.ViewModels;
using FlightsNorway.Lib.Services;
using FlightsNorway.Lib.DataServices;
using MonoTouch.Foundation;

namespace FlightsNorway
{
	public class AirportsTableViewController : UITableViewController
	{
		static NSString CellID = new NSString ("AirportCell");
		
		public AirportsTableViewController ()
		{			
		}
		
		class AirportsDataSource : UITableViewDataSource
		{
			private readonly AirportNamesService _service; 
			private readonly MonoObjectStore _objectStore;
			private readonly AirportsViewModel _viewModel;
			private readonly AirportsTableViewController _tvc;
			
			public AirportsDataSource(AirportsTableViewController tvc)
			{
				_tvc = tvc;
				_service = new AirportNamesService();
				_objectStore = new MonoObjectStore();				
				_viewModel = new AirportsViewModel(_service, _objectStore);
				
				_viewModel.Airports.CollectionChanged += (o, e) => _tvc.TableView.ReloadData();
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
				
		public override void ViewDidLoad()
		{
			base.ViewDidLoad();				
			TableView.DataSource = new AirportsDataSource(this);
		}
	}
}