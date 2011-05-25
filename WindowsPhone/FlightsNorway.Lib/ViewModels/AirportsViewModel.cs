using System.Collections.ObjectModel;
using System.IO;
using FlightsNorway.Lib.DataServices;
using FlightsNorway.Lib.Messages;
using FlightsNorway.Lib.Model;

namespace FlightsNorway.Lib.ViewModels
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

                _selectedAirport = value;
                RaisePropertyChanged("SelectedAirport");
            }
        }

        public AirportsViewModel(Stream norwegianAirports)
        {
            Airports = new ObservableCollection<Airport>();
            var service = new AirportNamesService();

            foreach (var airport in service.GetNorwegianAirports(norwegianAirports))
            {
                Airports.Add(airport);
            }
        }

        public void SaveSelection()
        {
            ServiceLocator.Messenger.Publish
                (new AirportSelectedMessage(this, SelectedAirport));
        }
    }
}
