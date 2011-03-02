using System.Linq;
using System.Collections.Generic;
using FlightsNorway.Lib.DataServices;
using FlightsNorway.Lib.Model;
using Microsoft.Silverlight.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FlightsNorway.Tests.FlightDataServiceTest
{
    [TestClass]
    public class AirportNamesServiceSpec : SilverlightTest
    {
        [TestMethod, Asynchronous, Timeout(7500), Tag(Tags.WebService)]
        public void Should_be_able_to_get_airport_names()
        {
            var airportList = new List<Airport>();
            EnqueueCallback(() => service.GetAirports(r => airportList.AddRange(r.Value)));
            EnqueueConditional(() => airportList.Count > 0);
            EnqueueTestComplete();
        }

        [TestMethod, Tag(Tags.Services)]
        public void Should_be_able_to_get_all_Norwegian_airports()
        {
            var airports = service.GetNorwegianAirports();
            Assert.AreEqual(54, airports.Count());
        }

        [TestMethod, Tag(Tags.Services)]
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
