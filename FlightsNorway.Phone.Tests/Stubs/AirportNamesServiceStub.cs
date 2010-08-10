using System;
using System.Collections.Generic;
using FlightsNorway.Model;
using Microsoft.Phone.Reactive;
using FlightsNorway.FlightDataServices;

namespace FlightsNorway.Tests.Stubs
{
    public class AirportNamesServiceStub : IGetAirports
    {
        public IEnumerable<Airport> Airports { get; set; }
        public IEnumerable<Airport> NorwegianAirports { get; set; }

        public IObservable<IEnumerable<Airport>> GetAirports()
        {
            var allAirports = new List<IEnumerable<Airport>> {Airports};
            return allAirports.ToObservable();
        }

        public IEnumerable<Airport> GetNorwegianAirports()
        {
            return NorwegianAirports;
        }

        public Airport GetNearestAirport(Location home)
        {
            throw new NotImplementedException();
        }
    }
}
