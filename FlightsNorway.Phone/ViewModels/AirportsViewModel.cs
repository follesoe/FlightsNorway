using System;
using System.Linq;
using System.Collections.ObjectModel;
using FlightsNorway.Phone.FlightDataServices;
using FlightsNorway.Phone.Model;

namespace FlightsNorway.Phone.ViewModels
{
    public class AirportsViewModel
    {
        public ObservableCollection<Airport> Airports { get; private set; }

        public AirportsViewModel() : this(new AirportNamesService())
        {
            
        }

        public AirportsViewModel(IGetAirports airportsService)
        {
            Airports = new ObservableCollection<Airport>();
            airportsService.GetAirports().SubscribeOnDispatcher()
                                         .Subscribe(airports => Airports.AddRange(airports));
        }
    }
}
