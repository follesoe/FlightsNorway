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
		
		private AirportNamesService _airportService;		
		
		public override void ViewDidLoad()
		{
			base.ViewDidLoad();
			
			_airportService = new AirportNamesService();
			_airportService.GetAirports(AirportsLoaded);			
		}
		
		private void AirportsLoaded(Result<IEnumerable<Airport>> result)		
		{
			if(result.HasError())
			{
				Console.WriteLine("Exception: {0}", result.Error);
			}
			else
			{
				foreach(var airport in result.Value)
				{
					Console.WriteLine(airport);
				}
			}						
		}
	}
}

