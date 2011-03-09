using System;
using System.Linq;
using System.Collections.Generic;

using MonoTouch.UIKit;
using MonoTouch.Foundation;

using FlightsNorway.Lib.Services;
using FlightsNorway.Lib.DataServices;
using FlightsNorway.Lib.ViewModels;

namespace FlightsNorway
{
	public class Application
	{
		static void Main (string[] args)
		{	
			var container = TinyIoC.TinyIoCContainer.Current;
			container.Register<AirportsViewModel>().AsSingleton();
			container.Register<IStoreObjects, MonoObjectStore>().AsSingleton();
			container.Register<IGetAirports, AirportNamesService>().AsSingleton();			
								
			UIApplication.Main(args);
		}
	}
}

