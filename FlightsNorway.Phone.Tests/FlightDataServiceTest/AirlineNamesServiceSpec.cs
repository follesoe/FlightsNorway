using System;
using System.Collections.Generic;

using FlightsNorway.Phone.Model;
using FlightsNorway.Phone.FlightDataServices;

using Microsoft.Silverlight.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FlightsNorway.Phone.Tests.FlightDataServiceTest
{
    [TestClass]
    public class AirlineNamesServiceSpec : SilverlightTest
    {
        [TestMethod, Asynchronous, Timeout(5000), Tag(Tags.WebService)]
        public void Should_be_able_to_get_airline_names()
        {
            var airlineList = new List<Airline>();
            service.GetAirlines().Subscribe(airlineList.AddRange);
            EnqueueConditional(() => airlineList.Count > 0);
            EnqueueTestComplete();
        }

        [TestInitialize]
        public void Setup()
        {
            service = new AirlineNamesService();            
        }

        private AirlineNamesService service;
    }
}
