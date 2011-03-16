using System;
using FlightsNorway.Lib.Services;
using FlightsNorway.Lib.ViewModels;
using FlightsNorway.Lib.DataServices;
using MonoTouch.Foundation;

namespace FlightsNorway
{
	public class ContainerConfiguration
	{
		public static void Configure(NSObject owner)
		{
			var container = TinyIoC.TinyIoCContainer.Current;
			
			container.Register<AirportsViewModel>().AsSingleton();
			container.Register<FlightsViewModel>().AsSingleton();
			
			container.Register<IStoreObjects, MonoObjectStore>().AsSingleton();
			container.Register<IGetFlights, FlightsService>().AsSingleton();
			container.Register<IGetAirports, AirportNamesService>().AsSingleton();
			
					
			container.Register<MonoMobile.Extensions.IGeolocation>(
                       new PresetLocationService(63.433281, 10.419294, action => owner.InvokeOnMainThread(new NSAction(action))));												
			
			container.Register(new NearestAirportService(container.Resolve<IGeolocation>(), container.Resolve<ITinyMessengerHub>()));			
		}			
	}
}