using FlightsNorway.Services;
using FlightsNorway.ViewModels;
using FlightsNorway.DesignTimeData;
using FlightsNorway.FlightDataServices;
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

        public FlightsViewModel FlightsViewModel
        {
            get { return _container.Get<FlightsViewModel>(); }
        }

        public ClockViewModel ClockViewModel
        {
            get { return _container.Get<ClockViewModel>(); }
        }

        public ViewModelLocator()
        {
            _container = new StandardKernel();
            _container.Bind<ClockViewModel>().ToSelf().InSingletonScope();            
            _container.Bind<FlightsViewModel>().ToSelf().InSingletonScope();
            _container.Bind<AirportsViewModel>().ToSelf().InSingletonScope();
            _container.Bind<IGetAirports>().To<AirportNamesService>().InSingletonScope();
            _container.Bind<IOpenCommunicationChannel>().To<NotificationService>().InSingletonScope();

            _container.Bind<IGetCurrentLocation>().ToConstant(new PresetLocationService(63.433281, 10.419294));
            _container.Bind<NearestAirportService>().ToConstant(new NearestAirportService(_container.Get<IGetCurrentLocation>()));
            
            if(ViewModelBase.IsInDesignModeStatic)
            {
                _container.Bind<IStoreObjects>().To<DesignTimeObjectStore>().InSingletonScope();
                _container.Bind<IGetFlights>().To<DesignTimeFlightsService>().InSingletonScope();
            }
            else
            {
                _container.Bind<IStoreObjects>().To<ObjectStore>().InSingletonScope();
                _container.Bind<IGetFlights>().To<FlightsService>().InSingletonScope();
            }            
        }
    }
}