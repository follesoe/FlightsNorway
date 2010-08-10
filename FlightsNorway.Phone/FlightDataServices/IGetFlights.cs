using System;
using System.Collections.Generic;
using FlightsNorway.Model;

namespace FlightsNorway.FlightDataServices
{
    public interface IGetFlights
    {
        AirlineDictionary Airlines { get; }
        AirportDictionary Airports { get; }
        StatusDictionary Statuses { get; }
        IObservable<IEnumerable<Flight>> GetFlightsFrom(Airport fromAirport);
    }
}
