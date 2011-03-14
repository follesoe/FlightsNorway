using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using FlightsNorway.Lib.DataServices;
using FlightsNorway.Lib.Messages;
using FlightsNorway.Lib.Model;
using FlightsNorway.Lib.MVVM;
using FlightsNorway.Lib.Services;
using TinyMessenger;

namespace FlightsNorway.Lib.ViewModels
{
    public class FlightsViewModel : ViewModelBase, IFlightsViewModel
    {
        private readonly IGetFlights _flightsService;
        private readonly IStoreObjects _objectStore;
        private readonly ITinyMessengerHub _messenger;

        public ObservableCollection<Flight> Arrivals { get; private set; }
        public ObservableCollection<Flight> Departures { get; private set; }
        public string ErrorMessage { get; private set; }

        private Airport _selectedAirport;

        public Airport SelectedAirport
        {
            get { return _selectedAirport; }
            set
            {
                if (_selectedAirport == value) return;

                _selectedAirport = value;
                LoadFlightsFromAirport(_selectedAirport);
                RaisePropertyChanged("SelectedAirport");
            }
        }       

        public FlightsViewModel(IGetFlights flightsService, 
                                IStoreObjects objectStore, 
                                ITinyMessengerHub messenger)
        {            
            Arrivals = new ObservableCollection<Flight>();
            Departures = new ObservableCollection<Flight>();

            _objectStore = objectStore;
            _flightsService = flightsService;
            _messenger = messenger;
            
            _messenger.Subscribe<AirportSelectedMessage>(OnAirportSelected);

            LoadSelectedAirport();
        }


        private void LoadSelectedAirport()
        {
            if (_objectStore.FileExists(ObjectStore.SelectedAirportFilename))
            {
                var airport = _objectStore.Load<Airport>(ObjectStore.SelectedAirportFilename);
                if (airport.Equals(Airport.Nearest))
                {
                    _messenger.Publish(new FindNearestAirportMessage(this));
                }
                else
                {
                    SelectedAirport = airport;
                }
            }
        }

        private void OnAirportSelected(AirportSelectedMessage message)
        {
            SelectedAirport = message.Content;            
        }

        public void LoadFlightsFromAirport(Airport airport)
        {
            Arrivals.Clear();
            Departures.Clear();

            _flightsService.GetFlightsFrom(FlightsLoaded, _selectedAirport);      
        }

        private void FlightsLoaded(Result<IEnumerable<Flight>> result)
        {
            if(result.HasError())
                HandleException(result.Error);
            else
                LoadFlights(result.Value);
        }

        private void LoadFlights(IEnumerable<Flight> flights)
        {
            foreach(var flight in flights)
            {                               
                if(flight.Direction == Direction.Arrival)
                {
                    Arrivals.Add(flight);
                }
                else
                {
                    Departures.Add(flight);                        
                }
            }
        }

        private void HandleException(Exception ex)
        {
            ErrorMessage = ex.Message;
        }
    }
}