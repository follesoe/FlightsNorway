using System;
using MonoTouch.UIKit;
using FlightsNorway.Lib;
using FlightsNorway.Lib.Messages;
using FlightsNorway.Lib.ViewModels;
using FlightsNorway.Lib.Model;
using MonoTouch.Foundation;

namespace FlightsNorway
{
	public class ArrivalsTableViewController : UITableViewController
	{
		private readonly FlightsViewModel _viewModel;
		
		public ArrivalsTableViewController (FlightsViewModel viewModel)
		{
			_viewModel = viewModel;
			ServiceLocator.Messenger.Subscribe<AirportSelectedMessage>(message => {
				Title = "Arrivals at " + message.Content.Code;
			});
		}
		
		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();			
			TableView.DataSource = new ArrivalsDataSource(_viewModel, TableView);
		}
				
		private class ArrivalsDataSource : ObservableDataSource<Flight>
		{
			private NSString _cellID = new NSString("Arrivals");
					
			public override NSString CellID { get { return _cellID; } }
					
			public ArrivalsDataSource(FlightsViewModel viewModel, UITableView tableView) : 
				base(viewModel.Arrivals, tableView)
			{
			}
		}
	}
}

