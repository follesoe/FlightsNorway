using System;
using FlightsNorway.Lib.Services;
using FlightsNorway.Lib.ViewModels;
using FlightsNorway.Lib.DataServices;
using MonoTouch.Foundation;
using MonoMobile.Extensions;
using TinyMessenger;

namespace FlightsNorway
{
	public class ContainerConfiguration
	{
		public static void Configure(NSObject owner)
		{
			var container = TinyIoC.TinyIoCContainer.Current;
			
			container.Register<IDispatchOnUIThread>(new Dispatcher(owner));
			container.Register<AirportsViewModel>().AsSingleton();
			container.Register<FlightsViewModel>().AsSingleton();
			
			container.Register<IStoreObjects, MonoObjectStore>().AsSingleton();
			container.Register<IGetFlights, FlightsService>().AsSingleton();
			container.Register<IGetAirports, AirportNamesService>().AsSingleton();			
					
			container.Register<IGeolocation>(new PresetLocationService(63.433281, 10.419294, container.Resolve<IDispatchOnUIThread>()));			
			container.Register(new NearestAirportService(container.Resolve<IGeolocation>(), container.Resolve<ITinyMessengerHub>()));			
		}			
	}
	
	public class Dispatcher : IDispatchOnUIThread
	{
		private readonly NSObject _owner;
		
		public Dispatcher(NSObject owner)
		{
			_owner = owner;
		}
		
		public void Invoke (Action action)
		{
			_owner.BeginInvokeOnMainThread(new NSAction(action));
		}		
	}
}