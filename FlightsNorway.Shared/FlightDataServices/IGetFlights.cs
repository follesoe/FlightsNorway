using System;
using System.Collections.Generic;
using FlightsNorway.Shared.Model;

namespace FlightsNorway.Shared.FlightDataServices
{
    public interface IGetFlights
    {
        AirlineDictionary Airlines { get; }
        AirportDictionary Airports { get; }
        StatusDictionary Statuses { get; }
        IObservable<IEnumerable<Flight>> GetFlightsFrom(Airport fromAirport);
    }
}
