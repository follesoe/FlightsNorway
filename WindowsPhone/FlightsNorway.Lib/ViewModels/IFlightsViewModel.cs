using System.Collections.ObjectModel;
using FlightsNorway.Lib.Model;

namespace FlightsNorway.Lib.ViewModels
{
    public interface IFlightsViewModel
    {
        ObservableCollection<Flight> Arrivals { get; }
        ObservableCollection<Flight> Departures { get; }
        Airport SelectedAirport { get; set; }
    }
}