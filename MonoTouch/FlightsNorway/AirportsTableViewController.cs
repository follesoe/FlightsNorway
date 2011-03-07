using System;
using System.Collections.Generic;

using MonoTouch.UIKit;

using FlightsNorway.Lib.Model;
using FlightsNorway.Lib.DataServices;

namespace FlightsNorway
{
	public class AirportsTableViewController : UITableViewController
	{
		public AirportsTableViewController ()
		{
		}
		
		class AirportsDataSource : UITableViewDataSource
		{
			private AirportNamesService _service; 
			
			public AirportsDataSource()
			{
				_service = new AirportNamesService();			
			}
		}
				
		public override void ViewDidLoad()
		{
			base.ViewDidLoad();			
		}
	}
}