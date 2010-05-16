using FlightsNorway.Phone.Model;
using FlightsNorway.Phone.ViewModels;
using FlightsNorway.Phone.Tests.Stubs;
using FlightsNorway.Phone.Tests.Builders;

using GalaSoft.MvvmLight.Messaging;

using Microsoft.Silverlight.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FlightsNorway.Phone.Tests.ViewModels
{
    [TestClass]
    public class FlightsViewModelSpecs : SilverlightTest
    {
        [TestMethod, Asynchronous, Timeout(5000), Tag(Tags.ViewModel)]
        public void Loads_flights_when_airport_is_selected()
        {           
            var airport = new Airport("LKL", "Lakselv");
            Messenger.Default.Send(new AirportSelectedMessage(airport));
            
            EnqueueConditional(() => flightsService.GetFlightsFromWasCalled);
            EnqueueCallback(() => Assert.AreEqual(airport, flightsService.FromAirport));
            EnqueueTestComplete();
        }

        [TestMethod, Asynchronous, Timeout(5000), Tag(Tags.ViewModel)]
        public void Adds_arrivals_to_correct_collection_when_loading_flights()
        {
            bool collectionChanged = false;
            viewModel.Arrivals.CollectionChanged += (o, e) => collectionChanged = true;

            Flight flight = FlightBuilder.Create.Flight("SK123").Arriving();
            flightsService.FlightsToReturn.Add(flight);

            var airport = new Airport("LKL", "Lakselv");
            Messenger.Default.Send(new AirportSelectedMessage(airport));

            EnqueueConditional(() => collectionChanged);                        
            EnqueueCallback(() => Assert.AreEqual(0, viewModel.Departures.Count));
            EnqueueCallback(() => Assert.AreEqual(1, viewModel.Arrivals.Count));
            EnqueueCallback(() => Assert.AreEqual("SK123", viewModel.Arrivals[0].FlightId));
            EnqueueTestComplete();            
        }

        [TestMethod, Asynchronous, Timeout(5000), Tag(Tags.ViewModel)]
        public void Adds_departures_to_correct_collection_when_loading_flights()
        {
            bool collectionChanged = false;
            viewModel.Departures.CollectionChanged += (o, e) => collectionChanged = true;

            Flight flight = FlightBuilder.Create.Flight("SK123").Departing();
            flightsService.FlightsToReturn.Add(flight);

            var airport = new Airport("LKL", "Lakselv");
            Messenger.Default.Send(new AirportSelectedMessage(airport));

            EnqueueConditional(() => collectionChanged);
            EnqueueCallback(() => Assert.AreEqual(0, viewModel.Arrivals.Count));
            EnqueueCallback(() => Assert.AreEqual(1, viewModel.Departures.Count));            
            EnqueueCallback(() => Assert.AreEqual("SK123", viewModel.Departures[0].FlightId));
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
