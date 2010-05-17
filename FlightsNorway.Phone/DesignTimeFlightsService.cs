using System;
using System.Linq;
using System.Collections.Generic;

using FlightsNorway.Phone.Model;
using FlightsNorway.Phone.FlightDataServices;

namespace FlightsNorway.Phone
{
    public class DesignTimeFlightsService : IGetFlights
    {
        public AirlineDictionary Airlines { get; set; }
        public AirportDictionary Airports { get; set; }
        public StatusDictionary Statuses { get; set; }

        private static readonly Random _random = new Random(DateTime.Now.Millisecond);

        public DesignTimeFlightsService()
        {
            Airports = new AirportDictionary();
            Airlines = new AirlineDictionary();
            Statuses = new StatusDictionary();
        }

        public IObservable<IEnumerable<Flight>> GetFlightsFrom(Airport fromAirport)
        {
            var allFlights = new List<IEnumerable<Flight>> { CreateDepartures() };
            return allFlights.ToObservable();
        }

        private static IEnumerable<Flight> CreateDepartures()
        {         
            for(int i = 0; i < 5; ++i)
            {
                var flight = new Flight(GetRandomAirline(), GetRandomAirport());
                flight.FlightId = flight.Airline.Code + "12" + i;
                flight.Direction = Direction.Depature;
                flight.ScheduledTime = new DateTime(2010, 1, 1, 10 + i, 15, 0);
                yield return flight;
            }

            for (int i = 0; i < 5; ++i)
            {
                var flight = new Flight(GetRandomAirline(), GetRandomAirport());
                flight.FlightId = flight.Airline.Code + "12" + i;
                flight.Direction = Direction.Arrival;
                flight.ScheduledTime = new DateTime(2010, 1, 1, 10 + i, 15, 0);
                yield return flight;
            }
        }

        private static Airport GetRandomAirport()
        {
            return _random.Next(0, 100) > 50 ? 
                new Airport("TOS", "Tromsø") : 
                new Airport("OSL", "Olso");
        }

        private static Airline GetRandomAirline()
        {
            return _random.Next(0, 100) > 50 ? 
                new Airline("SK", "SAS") : 
                new Airline("WF", "Wideroe");
        }
    }
}
