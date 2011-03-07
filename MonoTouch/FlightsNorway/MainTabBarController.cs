using System;
using MonoTouch.UIKit;

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
			
			var tabs = new UIViewController[1];
			tabs[0] = new AirportsTableViewController();
			
			ViewControllers = tabs;
		}
	}
}

