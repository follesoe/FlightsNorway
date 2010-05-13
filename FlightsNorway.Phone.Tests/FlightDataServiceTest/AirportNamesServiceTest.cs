using System;
using FlightsNorway.Phone.FlightDataServices;

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
            IObservable<Airport> airports = service.GetAirports();

            bool foundLakselv = false;
            airports.Subscribe(airport => { if (airport.Code == "LKL") foundLakselv = true; });

            EnqueueConditional(() => foundLakselv);
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
