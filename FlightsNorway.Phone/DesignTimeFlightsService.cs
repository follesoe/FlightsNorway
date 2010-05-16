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
            var tromso = new Airport("TOS", "Tromsø");
            var oslo = new Airport("OSL", "Olso");

            var sas = new Airline("SK", "SAS");
            var wideroe = new Airline("WF", "Wideroe");

            for(int i = 0; i < 5; ++i)
            {
                var flight = new Flight(sas, tromso);
                flight.FlightId = "SK12" + i;
                flight.Direction = Direction.Depature;
                flight.ScheduledTime = new DateTime(2010, 1, 1, 10 + i, 15, 0);
                yield return flight;
            }

            for (int i = 0; i < 5; ++i)
            {
                var flight = new Flight(wideroe, oslo);
                flight.FlightId = "WF12" + i;
                flight.Direction = Direction.Arrival;
                flight.ScheduledTime = new DateTime(2010, 1, 1, 10 + i, 15, 0);
                yield return flight;
            }
        }
    }
}