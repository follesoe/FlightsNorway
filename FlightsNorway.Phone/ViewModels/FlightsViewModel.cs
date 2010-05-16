using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

using FlightsNorway.Phone.Model;
using FlightsNorway.Phone.FlightDataServices;

using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;

namespace FlightsNorway.Phone.ViewModels
{
    public class FlightsViewModel : ViewModelBase
    {
        private readonly IGetFlights _flightsService;

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

        public ObservableCollection<Flight> Arrivals { get; private set; }
        public ObservableCollection<Flight> Departures { get; private set; }

        public FlightsViewModel(): this(new FlightsService())
        {            
        }

        public FlightsViewModel(IGetFlights flightsService)
        {
            Arrivals = new ObservableCollection<Flight>();
            Departures = new ObservableCollection<Flight>();

            _flightsService = flightsService;
            Messenger.Default.Register<AirportSelectedMessage>(this, OnAirportSelected);
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