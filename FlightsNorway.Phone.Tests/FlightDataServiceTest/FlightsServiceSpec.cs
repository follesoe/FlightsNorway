using System;
using System.Collections.Generic;

using FlightsNorway.Phone.Model;
using FlightsNorway.Phone.FlightDataServices;

using Microsoft.Silverlight.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FlightsNorway.Phone.Tests.FlightDataServiceTest
{
    [TestClass]
    public class FlightsServiceSpec : SilverlightTest
    {
        [TestMethod, Asynchronous, Timeout(10000), Tag(Tags.WebService)]
        public void Can_get_flights_from_Lakselv_airport()
        {
            var flightsList = new List<Flight>();
            
            EnqueueCallback(() => service.GetFlightsFrom(new Airport("TRD", "Trondheim")).Subscribe(flightsList.AddRange));
            EnqueueConditional(() => flightsList.Count > 0);
            EnqueueCallback(() => AssertFlightsAreLoaded(flightsList));
            EnqueueCallback(() => Assert.IsTrue(service.Airports.Count > 0));
            EnqueueCallback(() => Assert.IsTrue(service.Airlines.Count > 0));
            EnqueueCallback(() => Assert.IsTrue(service.Statuses.Count > 0));

            EnqueueTestComplete();
        }

        private static void AssertFlightsAreLoaded(List<Flight> flights)
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
            service = new FlightsService();
        }

        private FlightsService service;
    }
}