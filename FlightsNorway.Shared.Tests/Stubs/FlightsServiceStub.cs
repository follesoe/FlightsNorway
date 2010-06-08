using System;
using System.Linq;
using System.Collections.Generic;

using FlightsNorway.Shared.FlightDataServices;
using FlightsNorway.Shared.Model;

namespace FlightsNorway.Shared.Tests.Stubs
{
    public class FlightsServiceStub : IGetFlights
    {
        public AirlineDictionary Airlines { get; set; }
        public AirportDictionary Airports { get; set; }
        public StatusDictionary Statuses { get; set; }

        public List<Flight> FlightsToReturn { get; set; }

        public bool GetFlightsFromWasCalled;
        public Airport FromAirport;

        public FlightsServiceStub()
        {
            FlightsToReturn = new List<Flight>();
        }

        public IObservable<IEnumerable<Flight>> GetFlightsFrom(Airport fromAirport)
        {
            FromAirport = fromAirport;
            GetFlightsFromWasCalled = true;

            var allFlights = new List<IEnumerable<Flight>> { FlightsToReturn };
            return allFlights.ToObservable();
        }
    }
}
