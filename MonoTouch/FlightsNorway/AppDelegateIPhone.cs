using System;
using System.Collections.Generic;
using System.Linq;
using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace FlightsNorway
{
	public partial class AppDelegateIPhone : UIApplicationDelegate
	{
		private AirportsTableViewController _airportsTvc;
			
		public override bool FinishedLaunching (UIApplication app, NSDictionary options)
		{
			_airportsTvc = new AirportsTableViewController();
				
			window.AddSubview(_airportsTvc.View);
			window.MakeKeyAndVisible ();
	
			return true;
		}
	}
}

