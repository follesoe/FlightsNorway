using System.Collections.Generic;
using FlightsNorway.FlightDataServices;
using FlightsNorway.Lib.Model;
using Microsoft.Silverlight.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FlightsNorway.Tests.FlightDataServiceTest
{
    [TestClass]
    public class FlightsServiceSpec : SilverlightTest
    {
        [TestMethod, Asynchronous, Timeout(10000), Tag(Tags.WebService)]
        public void Can_get_flights_from_Oslo_airport()
        {
            var flightsList = new List<Flight>();
            var osl = new Airport("OSL", "Oslo");

            EnqueueCallback(() => _service.GetFlightsFrom(r => flightsList.AddRange(r.Value), osl));
            EnqueueConditional(() => flightsList.Count > 0);
            EnqueueCallback(() => AssertThatFlightsAreLoaded(flightsList));
            EnqueueCallback(() => Assert.IsTrue(_service.Airports.Count > 0));
            EnqueueCallback(() => Assert.IsTrue(_service.Airlines.Count > 0));
            EnqueueCallback(() => Assert.IsTrue(_service.Statuses.Count > 0));

            EnqueueTestComplete();
        }

        private static void AssertThatFlightsAreLoaded(List<Flight> flights)
        {
            Assert.IsTrue(flights.Count > 0);

            foreach(var flight in flights)
            {
                Assert.IsNotNull(flight.Airline);
                Assert.IsNotNull(flight.Airport);
                Assert.IsNotNull(flight.FlightStatus);
            }
        }

        [TestInitialize]
        public void Setup()
        {
            _service = new FlightsService();
        }

        private FlightsService _service;
    }
}