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
			
			var tabs = new UIViewController[1];
			tabs[0] = TinyIoCContainer.Current.Resolve<AirportsTableViewController>();
			
			ViewControllers = tabs;
		}
	}
}

