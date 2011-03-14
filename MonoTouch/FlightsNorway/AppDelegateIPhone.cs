using System;
using System.Linq;
using System.Collections.Generic;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using TinyIoC;

namespace FlightsNorway
{
	public partial class AppDelegateIPhone : UIApplicationDelegate
	{
		private MainTabBarController _controller;
		private UINavigationController _navigationController;		
		
		public override bool FinishedLaunching (UIApplication app, NSDictionary options)
		{
			_navigationController = new UINavigationController();					
			window.AddSubview(_navigationController.View);
			window.MakeKeyAndVisible();			
			
			_controller = TinyIoCContainer.Current.Resolve<MainTabBarController>();		
			_navigationController.PushViewController(_controller, false);					
			
			return true;
		}			
	}
}