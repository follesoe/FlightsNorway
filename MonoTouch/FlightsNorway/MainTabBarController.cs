using System;
using MonoTouch.UIKit;
using FlightsNorway.Lib.ViewModels;

namespace FlightsNorway
{
	public class MainTabBarController : UITabBarController
	{
		public MainTabBarController ()
		{
		}
		
		public override void ViewDidLoad ()
		{
			base.ViewDidLoad();
					
			var tabs = new UIViewController[3];
					
			var viewModel = new FlightsViewModel();
			tabs[0] = new UINavigationController(new ArrivalsTableViewController(viewModel));
			tabs[1] = new UINavigationController(new DeparturesTableViewController(viewModel));
			tabs[2] = new UINavigationController(new AirportsTableViewController());
					
			tabs[0].TabBarItem = new UITabBarItem("Arrivals", UIImage.FromBundle("Content/Arrivals.png"), 1);			
			tabs[1].TabBarItem = new UITabBarItem("Departures", UIImage.FromBundle("Content/Departures.png"), 1);
			tabs[2].TabBarItem = new UITabBarItem("Airports", UIImage.FromBundle("Content/Airports.png"), 1);
					
			ViewControllers = tabs;
		}

	}
}