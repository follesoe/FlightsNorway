using System;
using System.Collections.Generic;
using FlightsNorway.Lib.Model;

namespace FlightsNorway.Lib.DataServices
{
    public interface IGetFlights
    {
        AirlineDictionary Airlines { get; }
        AirportDictionary Airports { get; }
        StatusDictionary Statuses { get; }
        void GetFlightsFrom(Action<Result<IEnumerable<Flight>>> callback, Airport fromAirport);
    }
}