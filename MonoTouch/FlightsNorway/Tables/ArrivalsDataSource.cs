using System;
using MonoTouch.UIKit;
using MonoTouch.Foundation;

using FlightsNorway.Lib.Model;
using FlightsNorway.Lib.ViewModels;
using System.Collections.ObjectModel;

namespace FlightsNorway
{
	public class ArrivalsDataSource : UITableViewDataSource
	{
		public UITableView TableView { get; set; }
		
		private readonly FlightsViewModel _viewModel;
		
		private NSString _cellID = new NSString("ArrivalCell");
						
		public ArrivalsDataSource(FlightsViewModel viewModel)
		{
			_viewModel = viewModel;
			
			_viewModel.Arrivals.CollectionChanged += (o, e) => {
				if(TableView != null)
					TableView.ReloadData();
			};
		}
		
		public override int RowsInSection (UITableView tableView, int section)
		{
			return _viewModel.Arrivals.Count;
		}	
		
		public override UITableViewCell GetCell (UITableView tableView, NSIndexPath indexPath)
		{
			var cell = tableView.DequeueReusableCell(_cellID);
            if (cell == null)
            {					
                cell = new UITableViewCell(UITableViewCellStyle.Subtitle, _cellID);
				cell.TextLabel.Font = UIFont.FromName("Georgia", 16f);				
				cell.DetailTextLabel.Font = UIFont.FromName("Georgia", 12f);
            }
        				
			var flight = _viewModel.Arrivals[indexPath.Row];
            cell.TextLabel.Text = flight.Line1();				
			cell.DetailTextLabel.Text = flight.Line2();
            return cell;
		}
	}
}