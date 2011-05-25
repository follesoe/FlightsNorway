using System;
using MonoTouch.UIKit;
using FlightsNorway.Lib;
using FlightsNorway.Lib.Messages;

namespace FlightsNorway
{
	public class ArrivalsTableViewController : UITableViewController
	{
		public ArrivalsTableViewController ()
		{
			ServiceLocator.Messenger.Subscribe<AirportSelectedMessage>(message => {
				Title = "Arrivals at " + message.Content.Code;
			});
		}
	}
}

