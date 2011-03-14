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
						
			ViewControllers = tabs;			
		}
	}
}

