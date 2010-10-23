using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using FlightsNorway.Model;
using FlightsNorway.Services;
using FlightsNorway.Messages;
using FlightsNorway.FlightDataServices;

using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using Microsoft.Phone.Reactive;

namespace FlightsNorway.ViewModels
{
    public interface IFlightsViewModel
    {
        ObservableCollection<Flight> Arrivals { get; }
        ObservableCollection<Flight> Departures { get; }
        Airport SelectedAirport { get; set; }
    }

    public class FlightsViewModel : ViewModelBase, IFlightsViewModel
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
                RaisePropertyChanged("SelectedAirport");
            }
        }

        public string ErrorMessage { get; set; }

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
            if (!_objectStore.FileExists(ObjectStore.SelectedAirportFilename)) return;

            var airport = _objectStore.Load<Airport>(ObjectStore.SelectedAirportFilename);
            if(airport.Equals(Airport.Nearest))
            {
                Messenger.Default.Send(new FindNearestAirportMessage());
            }
            else
            {
                SelectedAirport = airport;    
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

            var flights = _flightsService.GetFlightsFrom(_selectedAirport);
      
            flights.Subscribe(LoadFlights, HandleException);            
        }

        private void LoadFlights(IEnumerable<Flight> flights)
        {
            foreach(var flight in flights)
            {
                double hoursSince = DateTime.Now.Subtract(flight.ScheduledTime).TotalHours;
                               
                if(flight.Direction == Direction.Arrival)
                {
                    //if (hoursSince > 1) continue;
                    Arrivals.Add(flight);
                }
                else
                {
                    //if (hoursSince > 0.25) continue;                    
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