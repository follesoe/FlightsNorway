using System.Linq;
using System.Windows.Input;
using System.Collections.ObjectModel;
using FlightsNorway.Lib.DataServices;
using FlightsNorway.Lib.Extensions;
using FlightsNorway.Lib.Messages;
using FlightsNorway.Lib.Model;
using FlightsNorway.Lib.MVVM;
using FlightsNorway.Lib.Services;
using TinyMessenger;

namespace FlightsNorway.Lib.ViewModels
{
    public class AirportsViewModel : ViewModelBase
    {        
        public ObservableCollection<Airport> Airports { get; private set; }
        public ICommand SaveCommand { get; private set; }

        private readonly IStoreObjects _objectStore;
        private readonly ITinyMessengerHub _messenger;

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

        public AirportsViewModel(IGetAirports airportsService, IStoreObjects objectStore, ITinyMessengerHub messenger)
        {
            _objectStore = objectStore;
            _messenger = messenger;
            Airports = new ObservableCollection<Airport>();
            Airports.Add(Airport.Nearest);
            Airports.AddRange(airportsService.GetNorwegianAirports());

            SaveCommand = new RelayCommand(OnSave);
            LoadSelectedAirportFromDisk();
        }

        private void LoadSelectedAirportFromDisk()
        {
            if (!_objectStore.FileExists(Airport.SelectedAirportFilename)) return;

            var savedAirport = _objectStore.Load<Airport>(Airport.SelectedAirportFilename);
            SelectedAirport = Airports.Where(a => a.Code == savedAirport.Code).Single();
        }

        private void OnSave()
        {
            if (SelectedAirport == null) return;
            
            _objectStore.Save(SelectedAirport, Airport.SelectedAirportFilename);

            if (SelectedAirport.Equals(Airport.Nearest))
            {
                _messenger.Publish(new FindNearestAirportMessage(this));
            }
            else
            {
                _messenger.Publish(new AirportSelectedMessage(this, SelectedAirport));
            }
        }
    }
}