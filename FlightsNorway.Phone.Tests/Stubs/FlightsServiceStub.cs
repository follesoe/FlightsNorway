using System;
using System.Linq;
using System.Collections.Generic;

using FlightsNorway.Phone.Model;
using FlightsNorway.Phone.FlightDataServices;

namespace FlightsNorway.Phone.Tests.Stubs
{
    public class FlightsServiceStub : IGetFlights
    {
        public AirlineDictionary Airlines { get; set; }
        public AirportDictionary Airports { get; set; }
        public StatusDictionary Statuses { get; set; }

        public List<Flight> FlightsToReturn { get; set; }

        public IObservable<IEnumerable<Flight>> GetFlightsFrom(Airport fromAirport)
        {
            var allFlights = new List<IEnumerable<Flight>> { FlightsToReturn };
            return allFlights.ToObservable();
        }
    }
}
