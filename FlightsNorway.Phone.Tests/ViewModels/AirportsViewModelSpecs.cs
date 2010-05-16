using System.Collections.Generic;
using FlightsNorway.Phone.Model;
using FlightsNorway.Phone.Tests.Stubs;
using FlightsNorway.Phone.ViewModels;
using Microsoft.Silverlight.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FlightsNorway.Phone.Tests.ViewModels
{
    [TestClass]
    public class AirportsViewModelSpecs : SilverlightTest
    {
        [TestMethod, Asynchronous, Tag("viewmodel")]
        public void Should_load_airports_when_viewmodel_is_created()
        {
            var airport = new Airport {Code = "LKL", Name = "Lakselv"};
            airportsService.Airports = new List<Airport> {airport};
            viewModel = new AirportsViewModel(airportsService);

            EnqueueConditional(() => viewModel.Airports.Count > 0);
            EnqueueCallback(() => Assert.AreEqual(airport, viewModel.Airports[0]));
            EnqueueTestComplete();
        }

        [TestInitialize]
        public void Setup()
        {
            airportsService = new AirportNamesServiceStub();
            
        }

        private AirportNamesServiceStub airportsService;
        private AirportsViewModel viewModel;
    }
}
