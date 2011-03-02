using System;
using System.Collections.Generic;
using FlightsNorway.DataServices;
using FlightsNorway.Lib.DataServices;
using FlightsNorway.Lib.Model;

namespace FlightsNorway.Tests.Stubs
{
    public class AirportNamesServiceStub : IGetAirports
    {
        public IEnumerable<Airport> Airports { get; set; }
        public IEnumerable<Airport> NorwegianAirports { get; set; }
        
        public void GetAirports(Action<Result<IEnumerable<Airport>>> callback)
        {
            callback(new Result<IEnumerable<Airport>>(Airports));
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
