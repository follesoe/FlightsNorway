using System;
using System.Linq;
using System.Collections.Generic;

using FlightsNorway.Phone.Model;
using FlightsNorway.Phone.FlightDataServices;


namespace FlightsNorway.Phone.UnitTests.Stubs
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
