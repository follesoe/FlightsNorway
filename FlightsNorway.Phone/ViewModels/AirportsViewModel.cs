using System.Collections.ObjectModel;

using GalaSoft.MvvmLight;

using FlightsNorway.Phone.Model;
using FlightsNorway.Phone.FlightDataServices;

namespace FlightsNorway.Phone.ViewModels
{
    public class AirportsViewModel : ViewModelBase
    {
        public ObservableCollection<Airport> Airports { get; private set; }

        private Airport _selectedAirport;

        public Airport SelectedAirport
        {
            get { return _selectedAirport; }
            set
            {
                if (_selectedAirport == value) return;
                
                RaisePropertyChanged("SelectedAirport", _selectedAirport, value, true);
                _selectedAirport = value;                
            }
        }

        public AirportsViewModel() : this(new AirportNamesService())
        {
            
        }

        public AirportsViewModel(IGetAirports airportsService)
        {
            Airports = new ObservableCollection<Airport>();
            Airports.Add(new NearestAirport());
            Airports.AddRange(airportsService.GetNorwegianAirports());
        }      
    }
}