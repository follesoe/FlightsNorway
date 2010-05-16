using System;
using System.Linq;
using System.Collections.Generic;

using FlightsNorway.Phone.Model;
using FlightsNorway.Phone.FlightDataServices;

namespace FlightsNorway.Phone.Tests.Stubs
{
    public class AirportNamesServiceStub : IGetAirports
    {
        public IEnumerable<Airport> Airports { get; set; }

        public IObservable<IEnumerable<Airport>> GetAirports()
        {
            var allAirports = new List<IEnumerable<Airport>> {Airports};
            return allAirports.ToObservable();
        }
    }
}
