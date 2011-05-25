using System;
using MonoTouch.UIKit;
using System.Collections.Generic;
using FlightsNorway.Lib.Model;
using MonoTouch.Foundation;
using FlightsNorway.Lib.DataServices;
using System.IO;
using FlightsNorway.Lib.ViewModels;

namespace FlightsNorway
{
	public class AirportsTableViewController : UITableViewController
	{
		public AirportsTableViewController ()
		{
		}
			
		private static AirportsViewModel _viewModel;
						
		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
					
			Title = "Airports";		
			using(var stream = new FileStream("Content/Airports.xml", FileMode.Open))
			{
				_viewModel = new AirportsViewModel(stream);
			}			
			TableView.DataSource = new AirportsDataSource(TableView);
			TableView.Delegate = new AirportsDelegate();
		}

		private class AirportsDelegate : UITableViewDelegate
		{
			public override void RowSelected (UITableView tableView, NSIndexPath indexPath)
			{
				_viewModel.SelectedAirport = _viewModel.Airports[indexPath.Row];
			}
		}
			
		private class AirportsDataSource : ObservableDataSource<Airport>		
		{
			private NSString _cellID = new NSString("Airports");			
			public override NSString CellID { get { return _cellID; } }
								
			public AirportsDataSource(UITableView tableView) : 
				base(_viewModel.Airports, tableView)
			{				
			}
		}
	}
}

