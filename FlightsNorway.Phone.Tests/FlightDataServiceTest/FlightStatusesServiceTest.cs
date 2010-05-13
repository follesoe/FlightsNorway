using System;

using FlightsNorway.Phone.FlightDataServices;

using Microsoft.Silverlight.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FlightsNorway.Phone.Tests.FlightDataServiceTest
{
    [TestClass]
    public class FlightStatusesServiceTest : SilverlightTest
    {
        [TestMethod, Asynchronous, Timeout(5000), Tag("webservice")]
        public void Should_be_able_to_get_airport_names()
        {
            IObservable<FlightStatus> airports = service.GetStautses();

            bool foundNewInfo = false;
            airports.Subscribe(airport => { if (airport.Code == "N") foundNewInfo = true; });

            EnqueueConditional(() => foundNewInfo);
            EnqueueTestComplete();
        }

        [TestInitialize]
        public void Setup()
        {
            service = new FlightStatusesService();
        }

        private FlightStatusesService service;
    }
}
