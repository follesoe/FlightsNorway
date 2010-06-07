using System.Linq;
using System.Windows.Input;
using System.Collections.ObjectModel;
using FlightsNorway.Shared.Messages;
using FlightsNorway.Shared.Model;
using FlightsNorway.Shared.Services;
using FlightsNorway.Shared.Extensions;
using FlightsNorway.Shared.FlightDataServices;

using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;

namespace FlightsNorway.Shared.ViewModels
{
    public class AirportsViewModel : ViewModelBase
    {        
        public ObservableCollection<Airport> Airports { get; private set; }
        public ICommand SaveCommand { get; private set; }

        private readonly IStoreObjects _objectStore;

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

        public AirportsViewModel(IGetAirports airportsService, IStoreObjects objectStore)
        {
            _objectStore = objectStore;
            Airports = new ObservableCollection<Airport>();
            Airports.Add(Airport.Nearest);
            Airports.AddRange(airportsService.GetNorwegianAirports());

            SaveCommand = new RelayCommand(OnSave);
            LoadSelectedAirportFromDisk();
        }

        private void LoadSelectedAirportFromDisk()
        {
            if (!_objectStore.FileExists(ObjectStore.SelectedAirportFilename)) return;
            var savedAirport = _objectStore.Load<Airport>(ObjectStore.SelectedAirportFilename);
            SelectedAirport = Airports.Where(a => a.Code == savedAirport.Code).Single();
        }

        private void OnSave()
        {
            _objectStore.Save(SelectedAirport, ObjectStore.SelectedAirportFilename);

            if(SelectedAirport.Equals(Airport.Nearest))
            {
                Messenger.Default.Send(new FindNearestAirportMessage());    
            }
            else
            {
                Messenger.Default.Send(new AirportSelectedMessage(SelectedAirport));    
            }            
        }
    }
}