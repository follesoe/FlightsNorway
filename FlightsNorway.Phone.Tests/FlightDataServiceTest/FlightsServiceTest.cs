using System;
using FlightsNorway.Phone.FlightDataServices;

using Microsoft.Silverlight.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FlightsNorway.Phone.Tests.FlightDataServiceTest
{
    [TestClass]
    public class FlightsServiceTest : SilverlightTest
    {
        [TestMethod]
        public void Can_get_flights_from_Lakselv_airport()
        {
            var airport = new Airport {Code = "LKL", Name = "Lakselv"};
            IObservable<Flight> flights = service.GetFlightsFrom(airport);

        }

        [TestInitialize]
        public void Setup()
        {
            service = new FlightsService();
        }

        private FlightsService service;
    }
}
