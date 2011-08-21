using System.ComponentModel;
using FlightsNorway.Lib.Model;
using FlightsNorway.Lib.ViewModels;
using System.Collections.ObjectModel;

namespace FlightsNorway.DesignTimeData
{
    public class FlightsDesignTimeViewModel : IFlightsViewModel
    {
        public bool IsBusy { get; set; }

        public ObservableCollection<Flight> Arrivals { get; set; }
        public ObservableCollection<Flight> Departures { get; set; }
        public Airport SelectedAirport { get; set; }
        public Flight SelectedDeparture { get; set; }
        public Flight SelectedArrival { get; set; }
        
        public FlightsDesignTimeViewModel()
        {
            Arrivals = new ObservableCollection<Flight>();
            Departures = new ObservableCollection<Flight>();

            foreach(var arrival in DesignTimeFlightsService.CreateFlights(6, Direction.Arrival))
                Arrivals.Add(arrival);

            foreach(var departure in DesignTimeFlightsService.CreateFlights(6, Direction.Depature))
                Departures.Add(departure);
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
