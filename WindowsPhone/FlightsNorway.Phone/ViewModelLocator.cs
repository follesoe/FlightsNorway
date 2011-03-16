using System.Windows;
using System.ComponentModel;

using FlightsNorway.Lib.Services;
using FlightsNorway.Lib.ViewModels;
using FlightsNorway.Lib.DataServices;
using FlightsNorway.ViewModels;
using FlightsNorway.DesignTimeData;

using TinyIoC;
using TinyMessenger;
using MonoMobile.Extensions;

namespace FlightsNorway
{
    public class ViewModelLocator
    {       
        private readonly TinyIoCContainer _container;

        public AirportsViewModel AirportsViewModel
        {
            get { return _container.Resolve<AirportsViewModel>(); }
        }

        public IFlightsViewModel FlightsViewModel
        {
            get
            {
                return _container.Resolve<IFlightsViewModel>();
            }
        }

        public ClockViewModel ClockViewModel
        {
            get { return _container.Resolve<ClockViewModel>(); }
        }

        public ViewModelLocator()
        {

            _container = TinyIoCContainer.Current;
            _container.Register<ClockViewModel>().AsSingleton();
            _container.Register<AirportsViewModel>().AsSingleton();
            _container.Register<IGetAirports, AirportNamesService>().AsSingleton();
            
            #if DEBUG
            _container.Register<IGeolocation>(new PresetLocationService(63.433281, 10.419294, action => Deployment.Current.Dispatcher.BeginInvoke(action)));
            #else
            _container.Register<IGeolocation, MonoMobile.Extensions.Geolocation>();
            #endif
            _container.Register(new NearestAirportService(_container.Resolve<IGeolocation>(), _container.Resolve<ITinyMessengerHub>()));

            if (DesignerProperties.IsInDesignTool)
            {
                _container.Register<IStoreObjects, DesignTimeObjectStore>().AsSingleton();
                _container.Register<IGetFlights, DesignTimeFlightsService>().AsSingleton();
                _container.Register<IFlightsViewModel, FlightsDesignTimeViewModel>().AsSingleton();                
            }
            else
            {
                _container.Register<IStoreObjects, ObjectStore>().AsSingleton();
                _container.Register<IGetFlights, FlightsService>().AsSingleton();
                _container.Register<IFlightsViewModel, FlightsViewModel>().AsSingleton();
            }
        }
    }
}