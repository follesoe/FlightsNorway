using System.Collections.Generic;
using System.Collections.ObjectModel;
using FlightsNorway.Lib.DataServices;
using FlightsNorway.Lib.Messages;
using FlightsNorway.Lib.Model;

namespace FlightsNorway.Lib.ViewModels
{
    public class FlightsViewModel : ViewModelBase
    {
        public ObservableCollection<Flight> Arrivals { get; private set; }
        public ObservableCollection<Flight> Departures { get; private set; }

        private FlightsService _service;

        public FlightsViewModel()
        {
            _service = new FlightsService();

            Arrivals = new ObservableCollection<Flight>();
            Departures = new ObservableCollection<Flight>();

            ServiceLocator.Messenger
                .Subscribe<AirportSelectedMessage>(m => LoadFlightsFrom(m.Content));
        }

        private void LoadFlightsFrom(Airport airport)
        {
            _service.GetFlightsFrom(res =>
            {
                if (!res.HasError())
                {
                    ServiceLocator.Dispatcher.Invoke(() => AddFlights(res.Value));
                }
            }, airport);
        }

        private void AddFlights(IEnumerable<Flight> flights)
        {
            Arrivals.Clear();
            Departures.Clear();

            foreach (var flight in flights)
            {
                if (flight.Direction == Direction.Arrival)
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
