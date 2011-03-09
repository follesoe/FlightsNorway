using System;
using System.Collections.Generic;

using MonoTouch.UIKit;

using MonoTouch.Foundation;
using TinyIoC;
using FlightsNorway.Lib.ViewModels;

namespace FlightsNorway.Tables
{
	public class AirportsTableViewController : UITableViewController
	{				
		public AirportsTableViewController()
		{		
			
		}	

		private class AirportsDelegate : UITableViewDelegate
		{
			private readonly AirportsViewModel _viewModel;
			
			public AirportsDelegate(AirportsViewModel viewModel)
			{
				_viewModel = viewModel;
			}
			
			public override void RowSelected (UITableView tableView, MonoTouch.Foundation.NSIndexPath indexPath)
			{
				_viewModel.SelectedAirport = _viewModel.Airports[indexPath.Row];			
				Console.WriteLine("{0} was selected...", _viewModel.SelectedAirport);
			}
		}		
				
		public override void ViewDidLoad()
		{
			base.ViewDidLoad();		
			
			var dataSource = TinyIoCContainer.Current.Resolve<AirportsDataSource>();
			dataSource.Controller = this;
			
			TableView.DataSource = dataSource;
			TableView.Delegate = new AirportsDelegate(dataSource.ViewModel);
		}
	}
}