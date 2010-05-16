using System;
using System.Collections.Generic;
using FlightsNorway.Phone.FlightDataServices;
using FlightsNorway.Phone.Model;
using Microsoft.Silverlight.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FlightsNorway.Phone.Tests.FlightDataServiceTest
{
    [TestClass]
    public class AirportNamesServiceTest : SilverlightTest
    {
        [TestMethod, Asynchronous, Timeout(5000), Tag("webservice")]
        public void Should_be_able_to_get_airport_names()
        {
            var airportList = new List<Airport>();
            service.GetAirports().Subscribe(airportList.AddRange);
            EnqueueConditional(() => airportList.Count > 0);
            EnqueueTestComplete();
        }

        [TestInitialize]
        public void Setup()
        {
            service = new AirportNamesService();
        }

        private AirportNamesService service;
    }
}
