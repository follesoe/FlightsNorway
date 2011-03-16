using System;
using System.Linq;
using System.Collections.Generic;

using MonoTouch.Foundation;
using MonoTouch.UIKit;
using TinyIoC;

namespace FlightsNorway
{
	public partial class AppDelegateIPad : UIApplicationDelegate
	{
		private MainTabBarController _controller;
		
		public override bool FinishedLaunching (UIApplication app, NSDictionary options)
		{
			ContainerConfiguration.Configure(this);
			
			_controller = TinyIoCContainer.Current.Resolve<MainTabBarController>();		
			window.AddSubview(_controller.View);					
			window.MakeKeyAndVisible();			
			return true;
		}
	}
}