using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

using FlightsNorway.Phone.Model;
using FlightsNorway.Phone.Services;
using FlightsNorway.Phone.FlightDataServices;

using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;

namespace FlightsNorway.Phone.ViewModels
{
    public class FlightsViewModel : ViewModelBase
    {
        private readonly IGetFlights _flightsService;
        private readonly IStoreObjects _objectStore;

        public ObservableCollection<Flight> Arrivals { get; private set; }
        public ObservableCollection<Flight> Departures { get; private set; }

        private Airport _selectedAirport;

        public Airport SelectedAirport
        {
            get { return _selectedAirport; }
            set
            {
                if (_selectedAirport == value) return;

                _selectedAirport = value;
                AirportSelected(_selectedAirport);
            }
        }

        public FlightsViewModel(): this(new FlightsService(), new ObjectStore())
        {            
        }

        public FlightsViewModel(IGetFlights flightsService, IStoreObjects objectStore)
        {
            Arrivals = new ObservableCollection<Flight>();
            Departures = new ObservableCollection<Flight>();

            _objectStore = objectStore;
            _flightsService = flightsService;

            Messenger.Default.Register<AirportSelectedMessage>(this, OnAirportSelected);

            LoadSelectedAirportFromDisk();
        }

        private void LoadSelectedAirportFromDisk()
        {
            if(_objectStore.FileExists(ObjectStore.SelectedAirportFilename))
            {
                SelectedAirport = _objectStore.Load<Airport>(ObjectStore.SelectedAirportFilename);
            }
        }

        private void OnAirportSelected(AirportSelectedMessage message)
        {
            SelectedAirport = message.Content;            
        }

        public void AirportSelected(Airport airport)
        {
            Arrivals.Clear();
            Departures.Clear();

            _flightsService.GetFlightsFrom(_selectedAirport).Subscribe(LoadFlights);            
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
    }
}