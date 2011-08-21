using System.Collections.ObjectModel;
using System.ComponentModel;
using FlightsNorway.Lib.Model;

namespace FlightsNorway.Lib.ViewModels
{
    public interface IFlightsViewModel : INotifyPropertyChanged
    {
        bool IsBusy { get; set; }
        ObservableCollection<Flight> Arrivals { get; }
        ObservableCollection<Flight> Departures { get; }
        Airport SelectedAirport { get; set; }
    }
}