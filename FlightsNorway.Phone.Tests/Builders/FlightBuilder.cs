using System;
using FlightsNorway.Shared.Model;

namespace FlightsNorway.Phone.Tests.Builders
{
    public class FlightBuilder
    {
        private static Flight flight;

        public static FlightBuilder Create
        {
            get { return new FlightBuilder(); }
        }

        public FlightBuilder Flight(string number)
        {
            flight.FlightId = number;
            return this;
        }

        public FlightBuilder Arriving()
        {
            flight.Direction = Direction.Arrival;
            return this;
        }

        public FlightBuilder Departing()
        {
            flight.Direction = Direction.Depature;
            return this;
        }

        public FlightBuilder At(int hours, int minutes)
        {
            flight.ScheduledTime = new DateTime(2010, 1, 1, hours, minutes, 0);
            return this;
        }

        static FlightBuilder()
        {
            flight = new Flight();
            flight.Airport = new Airport();
            flight.Airline = new Airline();
            flight.FlightStatus = new FlightStatus();
        }

        public static implicit operator Flight(FlightBuilder builder)
        {
            return flight;
        }
    }
}
