using System;
using System.Linq;
using System.Collections.Generic;

using FlightsNorway.Phone.Model;
using FlightsNorway.Phone.FlightDataServices;


using Microsoft.Silverlight.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FlightsNorway.Phone.Tests.FlightDataServiceTest
{
    [TestClass]
    public class AirportNamesServiceSpec : SilverlightTest
    {
        [TestMethod, Asynchronous, Timeout(7500), Tag(Tags.WebService)]
        public void Should_be_able_to_get_airport_names()
        {
            var airportList = new List<Airport>();
            service.GetAirports().Subscribe(airportList.AddRange);
            EnqueueConditional(() => airportList.Count > 0);
            EnqueueTestComplete();
        }

        [TestMethod]
        public void Should_be_able_to_get_all_Norwegian_airports()
        {
            var airports = service.GetNorwegianAirports();
            Assert.AreEqual(54, airports.Count());
        }

        [TestInitialize]
        public void Setup()
        {
            service = new AirportNamesService();
        }

        private AirportNamesService service;
    }
}
