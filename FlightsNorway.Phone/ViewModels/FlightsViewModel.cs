using System;
using System.Collections.Generic;
using System.Linq;

using FlightsNorway.Phone.FlightDataServices;
using FlightsNorway.Phone.Model;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;

namespace FlightsNorway.Phone.ViewModels
{
    public class FlightsViewModel : ViewModelBase
    {
        private readonly IGetFlights _flightsService;
        private Airport _selectedAirport;

        public FlightsViewModel(): this(new FlightsService())
        {            
        }

        public FlightsViewModel(IGetFlights flightsService)
        {
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
            System.Diagnostics.Debug.WriteLine(flights);
        }
    }
}