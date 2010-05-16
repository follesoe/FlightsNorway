using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Collections.Generic;

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
            if (_selectedAirport == message.Content) return;

            _selectedAirport = message.Content;
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