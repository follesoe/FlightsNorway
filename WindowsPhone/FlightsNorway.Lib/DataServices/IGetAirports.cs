using System;
using System.Collections.Generic;
using FlightsNorway.Lib.Model;

namespace FlightsNorway.Lib.DataServices
{
    public interface IGetAirports
    {
        void GetAirports(Action<Result<IEnumerable<Airport>>> callback);
        IEnumerable<Airport> GetNorwegianAirports();
        Airport GetNearestAirport(Location home);
    }
}