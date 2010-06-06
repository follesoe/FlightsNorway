using System;
using System.Collections.Generic;
using FlightsNorway.Phone.Model;

namespace FlightsNorway.Phone.FlightDataServices
{
    public interface IGetAirports
    {
        IObservable<IEnumerable<Airport>> GetAirports();
        IEnumerable<Airport> GetNorwegianAirports();
        Airport GetNearestAirport(Location home);
    }
}
