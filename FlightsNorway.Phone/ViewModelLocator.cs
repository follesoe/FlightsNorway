using FlightsNorway.Phone.Services;
using FlightsNorway.Phone.ViewModels;
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

        public ViewModelLocator()
        {
            _container = new StandardKernel();
            _container.Bind<AirportsViewModel>().To<AirportsViewModel>().InSingletonScope();
            _container.Bind<FlightsViewModel>().To<FlightsViewModel>().InSingletonScope();
            _container.Bind<IGetAirports>().To<AirportNamesService>().InSingletonScope();
            _container.Bind<IStoreObjects>().To<ObjectStore>().InSingletonScope();            

            _container.Bind<IGetCurrentLocation>().ToConstant(new PresetLocationService(63.433281, 10.419294));
            _container.Bind<NearestAirportService>().ToConstant(
                new NearestAirportService(_container.Get<IGetCurrentLocation>()));

            if(ViewModelBase.IsInDesignModeStatic)
            {
                _container.Bind<IGetFlights>().To<DesignTimeFlightsService>().InSingletonScope();
            }
            else
            {
                _container.Bind<IGetFlights>().To<FlightsService>().InSingletonScope();
            }            
        }
    }
}