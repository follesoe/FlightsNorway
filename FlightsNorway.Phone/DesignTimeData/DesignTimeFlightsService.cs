using System;
using System.Collections.Generic;

using FlightsNorway.Model;
using FlightsNorway.FlightDataServices;

using Microsoft.Phone.Reactive;

namespace FlightsNorway.DesignTimeData
{
    public class DesignTimeFlightsService : IGetFlights
    {
        public AirlineDictionary Airlines { get; set; }
        public AirportDictionary Airports { get; set; }
        public StatusDictionary Statuses { get; set; }

        private static readonly Random Random = new Random(DateTime.Now.Millisecond);

        public DesignTimeFlightsService()
        {
            Airports = new AirportDictionary();
            Airlines = new AirlineDictionary();
            Statuses = new StatusDictionary();
        }

        public IObservable<IEnumerable<Flight>> GetFlightsFrom(Airport fromAirport)
        {
            var allFlights = new List<IEnumerable<Flight>>
                                 {
                                     CreateFlights(6, Direction.Arrival), 
                                     CreateFlights(6, Direction.Depature)
                                 };
            return allFlights.ToObservable();
        }

        public static IEnumerable<Flight> CreateFlights(int number, Direction direction)
        {         
            for(var i = 0; i < number; ++i)
            {
                var flight = new Flight(GetRandomAirline(), GetRandomAirport());
                flight.FlightId = flight.Airline.Code + "12" + i;
                flight.Direction = direction;
                flight.ScheduledTime = new DateTime(2010, 1, 1, 10 + i, 15, 0);
                flight.Gate = GetRandomGate();
                flight.Belt = GetRandomBelt();                
                flight.FlightStatus = direction == Direction.Arrival ? GetRandomArrivalStatus() : GetRandomDepartureStatus();
                
                yield return flight;
            }
        }

        private static FlightStatus GetRandomDepartureStatus()
        {
            var status = new FlightStatus();
            status.StatusTime = DateTime.Now;

            status.Status.StatusTextNorwegian = 
                Random.Next(0, 100) > 50 ? "Boarding" : "Gå til gate";

            return status;
        }

        private static FlightStatus GetRandomArrivalStatus()
        {
            var status = new FlightStatus();
            status.StatusTime = DateTime.Now;

            status.Status.StatusTextNorwegian =
                Random.Next(0, 100) > 50 ? "Landet" : "Ny tid";

            return status;
        }

        private static string GetRandomGate()
        {
            return Random.Next(25, 39).ToString();
        }

        private static string GetRandomBelt()
        {
            return Random.Next(1, 6).ToString();
        }

        private static Airport GetRandomAirport()
        {
            return Random.Next(0, 100) > 50 ? 
                new Airport("TOS", "Tromsø") : 
                new Airport("OSL", "Oslo");
        }

        private static Airline GetRandomAirline()
        {
            return Random.Next(0, 100) > 50 ? 
                new Airline("SK", "SAS") : 
                new Airline("WF", "Wideroe");
        }
    }
}
