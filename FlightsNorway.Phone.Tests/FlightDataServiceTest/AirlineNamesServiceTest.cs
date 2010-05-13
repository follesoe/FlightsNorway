using System;

using FlightsNorway.Phone.FlightDataServices;

using Microsoft.Silverlight.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FlightsNorway.Phone.Tests.FlightDataServiceTest
{
    [TestClass]
    public class AirlineNamesServiceTest : SilverlightTest
    {
        [TestMethod, Asynchronous, Timeout(5000)]
        public void Should_return_a_dictionary_containing_airline_names()
        {
            IObservable<Airline> airlines = service.GetAirlines();

            bool foundSas = false;
            airlines.Subscribe(a => { if (a.Code == "SK") foundSas = true; });

            EnqueueConditional(() => foundSas);
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
