using System;
using System.Collections.Generic;
using FlightsNorway.FlightDataServices;
using FlightsNorway.Lib.Model;

namespace FlightsNorway.Tests.Stubs
{
    public class FlightsServiceStub : IGetFlights
    {
        public AirlineDictionary Airlines { get; set; }
        public AirportDictionary Airports { get; set; }
        public StatusDictionary Statuses { get; set; }

        public List<Flight> FlightsToReturn { get; set; }

        public bool GetFlightsFromWasCalled;
        public Airport FromAirport;

        public Exception ExceptionToBeThrown;

        public FlightsServiceStub()
        {
            FlightsToReturn = new List<Flight>();
        }

        public void GetFlightsFrom(Action<Result<IEnumerable<Flight>>> callback, Airport fromAirport)
        {
            FromAirport = fromAirport;
            GetFlightsFromWasCalled = true;

            callback(ExceptionToBeThrown == null
                         ? new Result<IEnumerable<Flight>>(GetFlights())
                         : new Result<IEnumerable<Flight>>(ExceptionToBeThrown));
        }

        private IEnumerable<Flight> GetFlights()
        {
            return FlightsToReturn;
        }
    }
}
