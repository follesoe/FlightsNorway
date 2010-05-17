using FlightsNorway.Phone.ViewModels;
using FlightsNorway.Phone.FlightDataServices;

using GalaSoft.MvvmLight;

namespace FlightsNorway.Phone
{
    public class ViewModelLocator
    {
        private readonly MicroContainer _container;

        public AirportsViewModel AirportsViewModel
        {
            get { return _container.Resolve<AirportsViewModel>(); }
        }

        public FlightsViewModel FlightsViewModel
        {
            get { return _container.Resolve<FlightsViewModel>(); }
        }

        public ViewModelLocator()
        {
            _container = new MicroContainer();

            _container.Register<AirportsViewModel, AirportsViewModel>();
            _container.Register<FlightsViewModel, FlightsViewModel>();
            _container.Register<IGetAirports, AirportNamesService>();

            if(ViewModelBase.IsInDesignModeStatic)
            {
                _container.Register<IGetFlights, DesignTimeFlightsService>();
            }
            else
            {
                _container.Register<IGetFlights, FlightsService>();
            }            
        }
    }
}