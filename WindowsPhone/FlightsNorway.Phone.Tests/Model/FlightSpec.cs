using System;
using FlightsNorway.Model;
using Microsoft.Silverlight.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FlightsNorway.Tests.Model
{
    [TestClass]
    public class FlightSpec
    {
        [TestMethod, Tag(Tags.Model)]
        public void Can_format_a_departure_as_string()
        {
            _flight.FlightId = "KL1127";            
            _flight.Gate = "46";
            _flight.Direction = Direction.Depature;
            _flight.Airport = new Airport("AMS", "Amsterdam");
            _flight.FlightStatus = new FlightStatus();
            _flight.FlightStatus.Status = new Status();
            _flight.FlightStatus.Status.StatusTextNorwegian = "Boarding";
            _flight.ScheduledTime = new DateTime(2010, 1, 1, 23, 30, 0);
            
            Assert.AreEqual("KL1127 23:30 46 Amsterdam Boarding", _flight.ToString());
        }

        [TestMethod, Tag(Tags.Model)]
        public void Can_format_a_arrivals_as_string()
        {
            _flight.FlightId = "DY1257";
            _flight.Belt = "8";
            _flight.Direction = Direction.Arrival;
            _flight.Airport = new Airport("AMS", "Amsterdam");            
            _flight.FlightStatus = new FlightStatus();
            _flight.FlightStatus.Status = new Status();
            _flight.FlightStatus.Status.StatusTextNorwegian = "Landet";
            _flight.FlightStatus.StatusTime = new DateTime(2010, 1, 1, 23, 8, 0);
            _flight.ScheduledTime = new DateTime(2010, 1, 1, 23, 15, 0);

            Assert.AreEqual("DY1257 23:15 8 Amsterdam Landet 23:08", _flight.ToString());
        }

        [TestInitialize]
        public void Setup()
        {
            _flight = new Flight();
        }

        private Flight _flight;
    }
}
