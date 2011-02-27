using System;
using System.Collections.Generic;
using FlightsNorway.Lib.Model;

namespace FlightsNorway.FlightDataServices
{
    public interface IGetFlights
    {
        AirlineDictionary Airlines { get; }
        AirportDictionary Airports { get; }
        StatusDictionary Statuses { get; }
        void GetFlightsFrom(Action<Result<IEnumerable<Flight>>> callback, Airport fromAirport);
    }
}
