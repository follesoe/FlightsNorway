using System;
using System.Collections.Generic;
using System.Linq;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using FlightsNorway.Lib;

namespace FlightsNorway
{
	public partial class AppDelegateIPhone : UIApplicationDelegate
	{
		private MainTabBarController _mainTabs;
				
		public override bool FinishedLaunching (UIApplication app, NSDictionary options)
		{
			ServiceLocator.Dispatcher = new DispatchAdapter(this);
			_mainTabs = new MainTabBarController();
					
			window.AddSubview(_mainTabs.View);
			window.MakeKeyAndVisible ();
		
			return true;
		}
	}
}

