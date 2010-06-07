using FlightsNorway.Phone.Services;
using FlightsNorway.Phone.ViewModels;
using FlightsNorway.Phone.DesignTimeData;
using FlightsNorway.Phone.FlightDataServices;

using Ninject;
using GalaSoft.MvvmLight;

namespace FlightsNorway.Phone
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
            _container.Bind<IMonitorFlights>().To<MonitoringService>().InSingletonScope();

            _container.Bind<IGetCurrentLocation>().ToConstant(new PresetLocationService(63.433281, 10.419294));
            _container.Bind<NearestAirportService>().ToConstant(new NearestAirportService(_container.Get<IGetCurrentLocation>()));
            _container.Bind<MonitorServiceClient>().ToConstant(new MonitorServiceClient(_container.Get<IOpenCommunicationChannel>(), _container.Get<IMonitorFlights>()));
            
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