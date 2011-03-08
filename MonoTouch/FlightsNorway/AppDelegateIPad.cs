using System;
using System.Linq;
using System.Collections.Generic;

using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace FlightsNorway
{
	public partial class AppDelegateIPad : UIApplicationDelegate
	{
		private MainTabBarController _controller;
		
		public override bool FinishedLaunching (UIApplication app, NSDictionary options)
		{
			// If you have defined a view, add it here:
			// window.AddSubview (navigationController.View);
			
			_controller = new MainTabBarController();			
			window.AddSubview(_controller.View);
			
			
			window.MakeKeyAndVisible ();
			
			return true;
		}
	}
}

