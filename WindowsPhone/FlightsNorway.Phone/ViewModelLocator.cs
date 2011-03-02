using System.Windows;
using FlightsNorway.Lib.DataServices;
using FlightsNorway.Lib.Services;
using FlightsNorway.Services;
using FlightsNorway.ViewModels;
using FlightsNorway.DesignTimeData;
using MonoMobile.Extensions;
using Ninject;
using GalaSoft.MvvmLight;

namespace FlightsNorway
{
    public class ViewModelLocator
    {       
        private readonly StandardKernel _container;

        public AirportsViewModel AirportsViewModel
        {
            get { return _container.Get<AirportsViewModel>(); }
        }

        public IFlightsViewModel FlightsViewModel
        {
            get
            {
                return _container.Get<IFlightsViewModel>();
            }
        }

        public ClockViewModel ClockViewModel
        {
            get { return _container.Get<ClockViewModel>(); }
        }

        public ViewModelLocator()
        {
            _container = new StandardKernel();
            _container.Bind<ClockViewModel>().ToSelf().InSingletonScope();            
            _container.Bind<AirportsViewModel>().ToSelf().InSingletonScope();
            _container.Bind<IGetAirports>().To<AirportNamesService>().InSingletonScope();
            _container.Bind<IPhoneApplicationService>().ToConstant(new PhoneApplicationServiceAdapter());

            #if DEBUG
            _container.Bind<IGeolocation>().ToConstant(new PresetLocationService(63.433281, 10.419294, action => Deployment.Current.Dispatcher.BeginInvoke(action)));
            #else
            _container.Bind<IGeolocation>().To<MonoMobile.Extensions.Geolocation>();
            #endif
            _container.Bind<NearestAirportService>().ToConstant(new NearestAirportService(_container.Get<IGeolocation>()));
            
            if(ViewModelBase.IsInDesignModeStatic)
            {
                _container.Bind<IStoreObjects>().To<DesignTimeObjectStore>().InSingletonScope();
                _container.Bind<IGetFlights>().To<DesignTimeFlightsService>().InSingletonScope();
                _container.Bind<IFlightsViewModel>().To<FlightsDesignTimeViewModel>().InSingletonScope();                
            }
            else
            {
                _container.Bind<IStoreObjects>().To<ObjectStore>().InSingletonScope();
                _container.Bind<IGetFlights>().To<FlightsService>().InSingletonScope();
                _container.Bind<IFlightsViewModel>().To<FlightsViewModel>().InSingletonScope();
            }
        }
    }
}