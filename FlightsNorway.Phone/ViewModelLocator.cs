using FlightsNorway.Phone.ViewModels;

namespace FlightsNorway.Phone
{
    public class ViewModelLocator
    {
        private AirportsViewModel _airportsViewModel;
        private FlightsViewModel _flightsViewModel;

        public AirportsViewModel AirportsViewModel
        {
            get { return _airportsViewModel ?? (_airportsViewModel = new AirportsViewModel()); }
        }

        public FlightsViewModel FlightsViewModel
        {
            get { return _flightsViewModel ?? (_flightsViewModel = new FlightsViewModel()); }
        }
    }
}
