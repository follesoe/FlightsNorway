using System;
using MonoTouch.UIKit;
using MonoTouch.Foundation;

using FlightsNorway.Lib.Model;
using FlightsNorway.Lib.ViewModels;
using System.Collections.ObjectModel;

namespace FlightsNorway
{
	public class ArrivalsDataSource : ViewModelDataSource<FlightsViewModel, Flight>
	{
		private NSString _cellID = new NSString("ArrivalCell");
		
		public override NSString CellID { get { return _cellID; } }
				
		public override ObservableCollection<Flight> List { get { return ViewModel.Arrivals; } }				
		
		public ArrivalsDataSource(FlightsViewModel viewModel) : base(viewModel)
		{
		}
		
		public override UITableViewCell GetCell (UITableView tableView, NSIndexPath indexPath)
		{
			var cell = tableView.DequeueReusableCell(CellID);
            if (cell == null)
            {					
                cell = new UITableViewCell(UITableViewCellStyle.Subtitle, CellID);
				cell.TextLabel.Font = UIFont.FromName("Georgia", 16f);				
				cell.DetailTextLabel.Font = UIFont.FromName("Georgia", 12f);
            }
        				
			var flight = List[indexPath.Row];
            cell.TextLabel.Text = flight.Line1();				
			cell.DetailTextLabel.Text = flight.Line2();
            return cell;
		}
	}
}