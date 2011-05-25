using System;
using MonoTouch.UIKit;
using FlightsNorway.Lib;
using FlightsNorway.Lib.Messages;

namespace FlightsNorway
{
	public class DeparturesTableViewController : UITableViewController
	{
		public DeparturesTableViewController ()
		{
			ServiceLocator.Messenger.Subscribe<AirportSelectedMessage>(message => {
				Title = "Departures from " + message.Content.Code;
			});
		}
	}
}

