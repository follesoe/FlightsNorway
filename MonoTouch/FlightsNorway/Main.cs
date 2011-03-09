using System;
using System.Linq;
using System.Collections.Generic;

using MonoTouch.UIKit;
using MonoTouch.Foundation;

using FlightsNorway.Lib.Services;
using FlightsNorway.Lib.DataServices;

namespace FlightsNorway
{
	public class Application
	{
		static void Main (string[] args)
		{	
			var container = TinyIoC.TinyIoCContainer.Current;
			container.Register<IStoreObjects, MonoObjectStore>();
			container.Register<IGetAirports, AirportNamesService>();			
								
			UIApplication.Main(args);
		}
	}
}

