using System;
using System.Drawing;
using MonoTouch.UIKit;
using FlightsNorway.Tables;
using TinyIoC;

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
						
			tabs[0] = new UINavigationController(container.Resolve<ArrivalsTableViewController>());
			tabs[1] = new UINavigationController(container.Resolve<DeparturesTableViewController>());
			tabs[2] = new UINavigationController(container.Resolve<AirportsTableViewController>());
			
			tabs[0].TabBarItem = new UITabBarItem("Arrivals", UIImage.FromBundle("Images/Arrivals.png"), 1);
			tabs[1].TabBarItem = new UITabBarItem("Departures", UIImage.FromBundle("Images/Departures.png"), 2);			
			tabs[2].TabBarItem = new UITabBarItem("Airports", UIImage.FromBundle("Images/Airports.png"), 3);
						
			ViewControllers = tabs;			
		}
	}
}