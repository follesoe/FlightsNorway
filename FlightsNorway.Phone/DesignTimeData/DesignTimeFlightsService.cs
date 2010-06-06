using System;
using System.Linq;
using System.Collections.Generic;
using FlightsNorway.Phone.Model;
using FlightsNorway.Phone.FlightDataServices;

namespace FlightsNorway.Phone.DesignTimeData
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
            for(int i = 0; i < 6; ++i)
            {
                var flight = new Flight(GetRandomAirline(), GetRandomAirport());
                flight.FlightId = flight.Airline.Code + "12" + i;
                flight.Direction = Direction.Depature;
                flight.ScheduledTime = new DateTime(2010, 1, 1, 10 + i, 15, 0);
                flight.Gate = GetRandomGate();
                flight.Belt = GetRandomBelt();
                flight.FlightStatus = GetRandomDepartureStatus();
                yield return flight;
            }

            for (int i = 0; i < 6; ++i)
            {
                var flight = new Flight(GetRandomAirline(), GetRandomAirport());
                flight.FlightId = flight.Airline.Code + "12" + i;
                flight.Direction = Direction.Arrival;
                flight.ScheduledTime = new DateTime(2010, 1, 1, 10 + i, 15, 0);
                flight.Gate = GetRandomGate();
                flight.Belt = GetRandomBelt();
                flight.FlightStatus = GetRandomArrivalStatus();                
                yield return flight;
            }
        }

        private static FlightStatus GetRandomDepartureStatus()
        {
            var status = new FlightStatus();
            status.StatusTime = DateTime.Now;

            status.Status.StatusTextNorwegian = 
                _random.Next(0, 100) > 50 ? "Boarding" : "Gå til gate";

            return status;
        }

        private static FlightStatus GetRandomArrivalStatus()
        {
            var status = new FlightStatus();
            status.StatusTime = DateTime.Now;

            status.Status.StatusTextNorwegian =
                _random.Next(0, 100) > 50 ? "Landet" : "Ny tid";

            return status;
        }

        private static string GetRandomGate()
        {
            return _random.Next(25, 39).ToString();
        }

        private static string GetRandomBelt()
        {
            return _random.Next(1, 6).ToString();
        }

        private static Airport GetRandomAirport()
        {
            return _random.Next(0, 100) > 50 ? 
                new Airport("TOS", "Tromsø") : 
                new Airport("OSL", "Oslo");
        }

        private static Airline GetRandomAirline()
        {
            return _random.Next(0, 100) > 50 ? 
                new Airline("SK", "SAS") : 
                new Airline("WF", "Wideroe");
        }
    }
}
