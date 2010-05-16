using FlightsNorway.Phone.Model;
using FlightsNorway.Phone.ViewModels;

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

            if(ViewModelBase.IsInDesignModeStatic)
            {
                _container.RegisterInstance(new AirportsViewModel());
                _container.RegisterInstance(new FlightsViewModel(new DesignTimeFlightsService()));
                _container.Resolve<FlightsViewModel>().SelectedAirport = new Airport("LKL", "Lakselv");
            }
            else
            {
                _container.RegisterInstance(new AirportsViewModel());
                _container.RegisterInstance(new FlightsViewModel());                    
            }            
        }
    }
}