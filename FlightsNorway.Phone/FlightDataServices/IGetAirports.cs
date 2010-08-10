using System;
using System.Collections.Generic;
using FlightsNorway.Model;

namespace FlightsNorway.FlightDataServices
{
    public interface IGetAirports
    {
        IObservable<IEnumerable<Airport>> GetAirports();
        IEnumerable<Airport> GetNorwegianAirports();
        Airport GetNearestAirport(Location home);
    }
}
