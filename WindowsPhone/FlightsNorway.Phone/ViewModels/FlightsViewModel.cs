using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using FlightsNorway.Lib.DataServices;
using FlightsNorway.Lib.Extensions;
using FlightsNorway.Lib.Messages;
using FlightsNorway.Lib.Model;
using FlightsNorway.Lib.MVVM;
using FlightsNorway.Lib.Services;
using FlightsNorway.Services;
using Microsoft.Phone.Shell;
using TinyMessenger;

namespace FlightsNorway.ViewModels
{
    public class FlightsViewModel : ViewModelBase, IFlightsViewModel
    {
        private readonly IPhoneApplicationService _appService;
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
                                IPhoneApplicationService appService,
                                ITinyMessengerHub messenger)
        {            
            Arrivals = new ObservableCollection<Flight>();
            Departures = new ObservableCollection<Flight>();

            _objectStore = objectStore;
            _flightsService = flightsService;
            _messenger = messenger;
            
            _appService = appService;
            _appService.Deactivated += OnDeactivated;

            _messenger.Subscribe<AirportSelectedMessage>(OnAirportSelected);

            LoadSelectedAirport();
            LoadFlightsFromAppState();
        }

        private void LoadFlightsFromAppState()
        {
            if(_appService.State.ContainsKey("Arrivals") && _appService.State.ContainsKey("Departures"))
            {
                Arrivals.AddRange(_appService.State.Get<ObservableCollection<Flight>>("Arrivals"));
                Departures.AddRange(_appService.State.Get<ObservableCollection<Flight>>("Departures"));
            }
        }

        private void OnDeactivated(object sender, DeactivatedEventArgs e)
        {
            _appService.State.AddOrReplace("Arrivals", Arrivals);
            _appService.State.AddOrReplace("Departures", Departures);
            _appService.State.AddOrReplace("SelectedAirport", SelectedAirport);
        }

        private void LoadSelectedAirport()
        {
            if(_appService.State.ContainsKey("SelectedAirport"))
            {
                _selectedAirport = _appService.State.Get<Airport>("SelectedAirport");
            }
            else if (_objectStore.FileExists(ObjectStore.SelectedAirportFilename))
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