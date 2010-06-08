using System;
using System.Linq;
using System.Collections.Generic;

using FlightsNorway.Shared.Model;
using FlightsNorway.Shared.FlightDataServices;

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

        [TestMethod, Tag(Tags.WebService)]
        public void Should_be_able_to_get_all_Norwegian_airports()
        {
            var airports = service.GetNorwegianAirports();
            Assert.AreEqual(54, airports.Count());
        }

        [TestMethod, Tag("yo")]
        public void Should_be_able_to_get_nearest_Norwegian_airport()
        {
            var home = new Location(63.433281, 10.419294);
            Airport airport = service.GetNearestAirport(home);

            Assert.AreEqual("TRD", airport.Code);
        }

        [TestInitialize]
        public void Setup()
        {
            service = new AirportNamesService();
        }

        private AirportNamesService service;
    }
}
