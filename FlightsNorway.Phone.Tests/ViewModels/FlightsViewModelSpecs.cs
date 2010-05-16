using System.Collections.Generic;
using FlightsNorway.Phone.Model;
using FlightsNorway.Phone.ViewModels;
using FlightsNorway.Phone.Tests.Stubs;

using GalaSoft.MvvmLight.Messaging;

using Microsoft.Silverlight.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FlightsNorway.Phone.Tests.ViewModels
{
    [TestClass]
    public class FlightsViewModelSpecs : SilverlightTest
    {
        [TestMethod, Asynchronous, Timeout(100000), Tag(Tags.ViewModel)]
        public void Loads_flights_when_airport_is_selected()
        {           
            var airport = new Airport("LKL", "Lakselv");
            Messenger.Default.Send(new AirportSelectedMessage(airport));
            
            EnqueueConditional(() => flightsService.GetFlightsFromWasCalled);
            EnqueueCallback(() => Assert.AreEqual(airport, flightsService.FromAirport));
            EnqueueTestComplete();
        }

        [TestInitialize]
        public void Setup()
        {
            flightsService = new FlightsServiceStub();
            viewModel = new FlightsViewModel(flightsService);
        }

        private FlightsServiceStub flightsService;
        private FlightsViewModel viewModel;
    }
}
