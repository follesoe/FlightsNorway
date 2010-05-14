using System;
using System.Linq;
using System.Collections.Generic;

using FlightsNorway.Phone.Model;
using FlightsNorway.Phone.FlightDataServices;

using Microsoft.Silverlight.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FlightsNorway.Phone.Tests.FlightDataServiceTest
{
    [TestClass]
    public class FlightsServiceTest : SilverlightTest
    {
        [TestMethod, Asynchronous, Timeout(10000), Tag("webservice")]
        public void Can_get_flights_from_Lakselv_airport()
        {
            var flightsList = new List<Flight>();
            
            EnqueueConditional(() => airlines.Count > 0 && airports.Count > 0);
            EnqueueCallback(() => service.GetFlightsFrom(new Airport {Code = "LKL"}).Subscribe(flightsList.AddRange));
            EnqueueConditional(() => flightsList.Count() > 0);
            EnqueueTestComplete();
        }

        [TestInitialize]
        public void Setup()
        {
            statuses = new StatusDictionary();
            airports = new AirportDictionary();
            airlines = new AirlineDictionary();
            service = new FlightsService(airlines, airports, statuses);

            var airportService = new AirportNamesService();
            airportService.GetAirports().Subscribe(airportsEnum => { foreach (var airport in airportsEnum) airports.Add(airport); });

            var airlineService = new AirlineNamesService();
            airlineService.GetAirlines().Subscribe(airlinesEnum => {  foreach(var airline in airlinesEnum) airlines.Add(airline);});
        }

        private FlightsService service;
        private AirportDictionary airports;
        private AirlineDictionary airlines;
        private StatusDictionary statuses;
    }
}