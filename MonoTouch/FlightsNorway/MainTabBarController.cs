using System;
using MonoTouch.UIKit;
using TinyIoC;
using FlightsNorway.Tables;

namespace FlightsNorway
{
	public class MainTabBarController : UITabBarController
	{			
		public MainTabBarController()
		{
		}
						
		public override void ViewDidLoad()
		{
			base.ViewDidLoad ();	
			
			var container = TinyIoCContainer.Current;
			
			var tabs = new UIViewController[3];
			tabs[0] = container.Resolve<ArrivalsTableViewController>();
			tabs[1] = container.Resolve<DeparturesTableViewController>();
			tabs[2] = container.Resolve<AirportsTableViewController>();
			
			ViewControllers = tabs;
		}
	}
}

