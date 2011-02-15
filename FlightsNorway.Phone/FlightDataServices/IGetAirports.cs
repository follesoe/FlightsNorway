using System;
using System.Collections.Generic;
using FlightsNorway.Model;

namespace FlightsNorway.FlightDataServices
{
    public interface IGetAirports
    {
        void GetAirports(Action<Result<IEnumerable<Airport>>> callback);
        IEnumerable<Airport> GetNorwegianAirports();
        Airport GetNearestAirport(Location home);
    }
}
