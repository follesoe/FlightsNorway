using System;
using MonoTouch.UIKit;
using System.Collections.Generic;
using FlightsNorway.Lib.Model;
using MonoTouch.Foundation;
using FlightsNorway.Lib.DataServices;
using System.IO;

namespace FlightsNorway
{
	public class AirportsTableViewController : UITableViewController
	{
		public AirportsTableViewController ()
		{
		}
			
		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
					
			TableView.DataSource = new AirportsDataSource();
		}

			
		private class AirportsDataSource : UITableViewDataSource		
		{	
			private NSString CellID = new NSString("Airports");
			private List<Airport> _airports;
			
			public AirportsDataSource()
			{
				var service = new AirportNamesService();
				using(var stream = new FileStream("Content/Airports.xml", FileMode.Open))
				{
					_airports = service.GetNorwegianAirports(stream);
				}
			}
						
			public override int RowsInSection (UITableView tableView, int section)
			{
				return _airports.Count;
			}
			
			public override UITableViewCell GetCell (UITableView tableView, NSIndexPath indexPath)
			{
				var cell = tableView.DequeueReusableCell(CellID);
				if (cell == null)
				{					
				    cell = new UITableViewCell(UITableViewCellStyle.Default, CellID);
				}
				        				
				cell.TextLabel.Text = _airports[indexPath.Row].ToString();				
				return cell;
			}
			
		}		
	}
}

