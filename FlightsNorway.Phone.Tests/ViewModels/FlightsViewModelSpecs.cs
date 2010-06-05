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
            Messenger.Default.Send(new AirportSelectedMessage(lakselvAirport));
            
            EnqueueConditional(() => flightsService.GetFlightsFromWasCalled);
            EnqueueCallback(() => Assert.AreEqual(lakselvAirport, flightsService.FromAirport));
            EnqueueTestComplete();
        }

        [TestMethod, Asynchronous, Timeout(5000), Tag(Tags.ViewModel)]
        public void Adds_arrivals_to_correct_collection_when_loading_flights()
        {
            bool collectionChanged = false;
            viewModel.Arrivals.CollectionChanged += (o, e) => collectionChanged = true;

            Flight flight = FlightBuilder.Create.Flight("SK123").Arriving();
            flightsService.FlightsToReturn.Add(flight);

            Messenger.Default.Send(new AirportSelectedMessage(lakselvAirport));

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

            Messenger.Default.Send(new AirportSelectedMessage(lakselvAirport));

            EnqueueConditional(() => collectionChanged);
            EnqueueCallback(() => Assert.AreEqual(0, viewModel.Arrivals.Count));
            EnqueueCallback(() => Assert.AreEqual(1, viewModel.Departures.Count));            
            EnqueueCallback(() => Assert.AreEqual("SK123", viewModel.Departures[0].FlightId));
            EnqueueTestComplete();
        }

        [TestMethod, Tag(Tags.ViewModel)]
        public void Loads_selected_airport_if_saved_to_disk()
        {
            objectStore.FileExistsShouldReturn = true;
            objectStore.LoadShouldReturn = lakselvAirport;

            viewModel = new FlightsViewModel(flightsService, objectStore);
            
            Assert.AreEqual(lakselvAirport.Code, viewModel.SelectedAirport.Code);
        }

        [TestMethod, Asynchronous, Timeout(5000), Tag(Tags.ViewModel)]
        public void Clears_the_lists_if_a_new_airport_is_selected()
        {
            Flight flight = FlightBuilder.Create.Flight("SK123").Departing();
            flightsService.FlightsToReturn.Add(flight);

            Messenger.Default.Send(new AirportSelectedMessage(lakselvAirport));

            EnqueueCallback(() => Messenger.Default.Send(new AirportSelectedMessage(lakselvAirport)));
            EnqueueConditional(() => viewModel.Departures.Count > 0);
            EnqueueCallback(() => flightsService.FlightsToReturn.Clear());
            EnqueueCallback(() => Messenger.Default.Send(new AirportSelectedMessage(trondheimAirport)));
            EnqueueCallback(() => Assert.AreEqual(0, viewModel.Departures.Count));
            EnqueueTestComplete();
        }

        [TestInitialize]
        public void Setup()
        {
            lakselvAirport = new Airport("LKL", "Lakselv");
            trondheimAirport = new Airport("TRD", "Trondheim");
            flightsService = new FlightsServiceStub();
            objectStore = new ObjectStoreMock();
            viewModel = new FlightsViewModel(flightsService, objectStore);
        }

        private Airport lakselvAirport;
        private Airport trondheimAirport;
        private FlightsServiceStub flightsService;
        private FlightsViewModel viewModel;
        private ObjectStoreMock objectStore;
        
    }
}
