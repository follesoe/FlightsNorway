using System;
using System.Collections.ObjectModel;

using MonoTouch.UIKit;
using MonoTouch.Foundation;

using FlightsNorway.Lib.Model;
using FlightsNorway.Lib.ViewModels;

using TinyIoC;

namespace FlightsNorway
{
	public class DeparturesDataSource : ViewModelDataSource<FlightsViewModel, Flight>
	{
		private NSString _cellID = new NSString("DepartureCell");
		
		public override NSString CellID { get { return _cellID; } }
				
		public override ObservableCollection<Flight> List { get { return ViewModel.Departures; } }				
		
		public DeparturesDataSource(FlightsViewModel viewModel) : base(viewModel)
		{
		}	
		
		public override UITableViewCell GetCell (UITableView tableView, NSIndexPath indexPath)
		{
			var cell = tableView.DequeueReusableCell(CellID);
            if (cell == null)
            {					
                cell = new UITableViewCell(UITableViewCellStyle.Subtitle, CellID);
				cell.ContentView.BackgroundColor = UIColor.Black;
				cell.TextLabel.BackgroundColor = UIColor.Black;
				cell.TextLabel.TextColor = UIColor.Yellow;
				cell.TextLabel.Font = UIFont.FromName("Georgia", 18f);
            }
        				
			var flight = List[indexPath.Row];
            cell.TextLabel.Text = flight.Line1();				
			cell.DetailTextLabel.Text = flight.Line2();
            return cell;
		}
	}
}