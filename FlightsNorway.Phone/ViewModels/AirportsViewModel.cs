using System.Windows.Input;
using System.Collections.ObjectModel;

using FlightsNorway.Phone.Model;
using FlightsNorway.Phone.Services;
using FlightsNorway.Phone.Extensions;
using FlightsNorway.Phone.FlightDataServices;

using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;

namespace FlightsNorway.Phone.ViewModels
{
    public class AirportsViewModel : ViewModelBase
    {
        public ObservableCollection<Airport> Airports { get; private set; }
        public ICommand SaveCommand { get; private set; }

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

        public AirportsViewModel() : this(new AirportNamesService())
        {
            
        }

        public AirportsViewModel(IGetAirports airportsService)
        {
            Airports = new ObservableCollection<Airport>();
            Airports.Add(new Airport("NEAREST", "Nærmeste flyplass"));
            Airports.AddRange(airportsService.GetNorwegianAirports());

            SaveCommand = new RelayCommand(OnSave);
        }      

        private void OnSave()
        {
            var objectStore = new ObjectStore();
            objectStore.Save(SelectedAirport, ObjectStore.SelectedAirportFilename);

            Messenger.Default.Send(new AirportSelectedMessage(SelectedAirport));
        }
    }
}