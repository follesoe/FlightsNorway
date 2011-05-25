using System;
using MonoTouch.UIKit;
using FlightsNorway.Lib;
using FlightsNorway.Lib.Messages;
using FlightsNorway.Lib.ViewModels;
using MonoTouch.Foundation;
using FlightsNorway.Lib.Model;

namespace FlightsNorway
{
	public class DeparturesTableViewController : UITableViewController
	{
		private readonly FlightsViewModel _viewModel;
		
		public DeparturesTableViewController (FlightsViewModel viewModel)
		{
			_viewModel = viewModel;
			ServiceLocator.Messenger.Subscribe<AirportSelectedMessage>(message => {
				Title = "Departures from " + message.Content.Code;
			});
		}
	
		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();			
			TableView.DataSource = new ArrivalsDataSource(_viewModel, TableView);
		}
				
		private class ArrivalsDataSource : ObservableDataSource<Flight>
		{
			private NSString _cellID = new NSString("Departures");
					
			public override NSString CellID { get { return _cellID; } }
					
			public ArrivalsDataSource(FlightsViewModel viewModel, UITableView tableView) : 
				base(viewModel.Departures, tableView)
			{
			}
		}		
	}
}

