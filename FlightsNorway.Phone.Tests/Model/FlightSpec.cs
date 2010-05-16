using System;
using FlightsNorway.Phone.Model;
using Microsoft.Silverlight.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FlightsNorway.Phone.Tests.Model
{
    [TestClass]
    public class FlightSpec
    {
        [TestMethod, Tag(Tags.Model)]
        public void Can_format_a_departure_as_string()
        {
            flight.FlightId = "KL1127";            
            flight.Gate = "46";
            flight.Direction = Direction.Depature;
            flight.Airport = new Airport("AMS", "Amsterdam");
            flight.FlightStatus = new FlightStatus();
            flight.FlightStatus.Status = new Status();
            flight.FlightStatus.Status.StatusTextNorwegian = "Boarding";
            flight.ScheduledTime = new DateTime(2010, 1, 1, 23, 30, 0);
            
            Assert.AreEqual("KL1127 23:30 46 Amsterdam Boarding", flight.ToString());
        }

        [TestMethod, Tag(Tags.Model)]
        public void Can_format_a_arrivals_as_string()
        {
            flight.FlightId = "DY1257";
            flight.Belt = "8";
            flight.Direction = Direction.Arrival;
            flight.Airport = new Airport("AMS", "Amsterdam");            
            flight.FlightStatus = new FlightStatus();
            flight.FlightStatus.Status = new Status();
            flight.FlightStatus.Status.StatusTextNorwegian = "Landet";
            flight.FlightStatus.StatusTime = new DateTime(2010, 1, 1, 23, 8, 0);
            flight.ScheduledTime = new DateTime(2010, 1, 1, 23, 15, 0);

            Assert.AreEqual("DY1257 23:15 8 Amsterdam Landet 23:08", flight.ToString());
        }

        [TestInitialize]
        public void Setup()
        {
            flight = new Flight();
        }

        private Flight flight;
    }
}
