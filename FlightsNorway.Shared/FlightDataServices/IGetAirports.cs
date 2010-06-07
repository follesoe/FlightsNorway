using System;
using System.Collections.Generic;
using FlightsNorway.Shared.Model;

namespace FlightsNorway.Shared.FlightDataServices
{
    public interface IGetAirports
    {
        IObservable<IEnumerable<Airport>> GetAirports();
        IEnumerable<Airport> GetNorwegianAirports();
        Airport GetNearestAirport(Location home);
    }
}
