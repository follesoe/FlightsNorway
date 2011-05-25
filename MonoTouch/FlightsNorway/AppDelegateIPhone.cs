using System;
using System.Collections.Generic;
using System.Linq;
using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace FlightsNorway
{
	public partial class AppDelegateIPhone : UIApplicationDelegate
	{
		private MainTabBarController _mainTabs;
				
		public override bool FinishedLaunching (UIApplication app, NSDictionary options)
		{
			_mainTabs = new MainTabBarController();
					
			window.AddSubview(_mainTabs.View);
			window.MakeKeyAndVisible ();
		
			return true;
		}
	}
}

