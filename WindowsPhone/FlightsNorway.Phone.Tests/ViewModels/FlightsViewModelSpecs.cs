using System;
using FlightsNorway.Lib.Messages;
using FlightsNorway.Lib.Model;
using FlightsNorway.Lib.ViewModels;
using FlightsNorway.Tests.Builders;
using FlightsNorway.Tests.Stubs;
using Microsoft.Silverlight.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TinyMessenger;

namespace FlightsNorway.Tests.ViewModels
{
    [TestClass]
    public class FlightsViewModelSpecs : SilverlightTest
    {
        [TestMethod, Tag(Tags.ViewModel)]
        public void Loads_selected_airport_if_saved_to_disk()
        {
            _objectStore.Save(_lakselvAirport, Airport.SelectedAirportFilename);

            _viewModel = new FlightsViewModel(_flightsService, _objectStore, _messenger, _dispatcher);

            Assert.AreEqual(_lakselvAirport.Code, _viewModel.SelectedAirport.Code);
        }

        [TestMethod, Asynchronous, Timeout(5000), Tag(Tags.ViewModel)]
        public void Loads_flights_when_airport_is_selected()
        {           
            _messenger.Publish(new AirportSelectedMessage(this, _lakselvAirport));
            
            EnqueueConditional(() => _flightsService.GetFlightsFromWasCalled);
            EnqueueCallback(() => Assert.AreEqual(_lakselvAirport, _flightsService.FromAirport));
            EnqueueTestComplete();
        }

        [TestMethod, Asynchronous, Timeout(5000), Tag(Tags.ViewModel)]
        public void Signals_to_the_user_that_a_service_call_is_happening()
        {            
            EnqueueCallback(() =>
            {
                _messenger.Publish(new AirportSelectedMessage(this, _lakselvAirport));
                Assert.IsTrue(_viewModel.IsBusy);
            });            
            EnqueueTestComplete();
        }

        [TestMethod, Asynchronous, Timeout(5000), Tag(Tags.ViewModel)]
        public void Expose_exception_as_error_message()
        {
            _flightsService.ExceptionToBeThrown = new Exception("Some error.");

            _messenger.Publish(new AirportSelectedMessage(this, _lakselvAirport));

            EnqueueConditional(() => _flightsService.GetFlightsFromWasCalled);
            EnqueueCallback(() => Assert.AreEqual("Some error.", _viewModel.ErrorMessage));
            EnqueueTestComplete();
        }

        [TestMethod, Asynchronous, Timeout(5000), Tag(Tags.ViewModel)]
        public void Adds_arrivals_to_correct_collection_when_loading_flights()
        {
            bool collectionChanged = false;
            _viewModel.Arrivals.CollectionChanged += (o, e) => collectionChanged = true;

            Flight flight = FlightBuilder.Create.Flight("SK123").Arriving();
            _flightsService.FlightsToReturn.Add(flight);

            _messenger.Publish(new AirportSelectedMessage(this, _lakselvAirport));

            EnqueueConditional(() => collectionChanged);                        
            EnqueueCallback(() => Assert.AreEqual(0, _viewModel.Departures.Count));
            EnqueueCallback(() => Assert.AreEqual(1, _viewModel.Arrivals.Count));
            EnqueueCallback(() => Assert.AreEqual("SK123", _viewModel.Arrivals[0].FlightId));
            EnqueueTestComplete();            
        }

        [TestMethod, Asynchronous, Timeout(5000), Tag(Tags.ViewModel)]
        public void Adds_departures_to_correct_collection_when_loading_flights()
        {
            bool collectionChanged = false;
            _viewModel.Departures.CollectionChanged += (o, e) => collectionChanged = true;

            Flight flight = FlightBuilder.Create.Flight("SK123").Departing();
            _flightsService.FlightsToReturn.Add(flight);

            _messenger.Publish(new AirportSelectedMessage(this, _lakselvAirport));

            EnqueueConditional(() => collectionChanged);
            EnqueueCallback(() => Assert.AreEqual(0, _viewModel.Arrivals.Count));
            EnqueueCallback(() => Assert.AreEqual(1, _viewModel.Departures.Count));            
            EnqueueCallback(() => Assert.AreEqual("SK123", _viewModel.Departures[0].FlightId));
            EnqueueTestComplete();
        }

        [TestMethod, Tag(Tags.ViewModel)]
        public void Finds_nearest_airport_if_option_is_selected()
        {
            _objectStore.Save(Airport.Nearest, Airport.SelectedAirportFilename);

            bool findNearestWasPublished = false;
            _messenger.Subscribe<FindNearestAirportMessage>(m => findNearestWasPublished = true);

            _viewModel = new FlightsViewModel(_flightsService, _objectStore, _messenger, _dispatcher);

            Assert.IsTrue(findNearestWasPublished);
        }

        [TestMethod, Asynchronous, Timeout(5000), Tag(Tags.ViewModel)]
        public void Clears_the_lists_if_a_new_airport_is_selected()
        {
            Flight flight = FlightBuilder.Create.Flight("SK123").Departing();
            _flightsService.FlightsToReturn.Add(flight);

            _messenger.Publish(new AirportSelectedMessage(this, _lakselvAirport));

            EnqueueCallback(() => _messenger.Publish(new AirportSelectedMessage(this, _lakselvAirport)));
            EnqueueConditional(() => _viewModel.Departures.Count > 0);
            EnqueueCallback(() => _flightsService.FlightsToReturn.Clear());
            EnqueueCallback(() => _messenger.Publish(new AirportSelectedMessage(this, _trondheimAirport)));
            EnqueueCallback(() => Assert.AreEqual(0, _viewModel.Departures.Count));
            EnqueueTestComplete();
        }

        [TestInitialize]
        public void Setup()
        {
            _lakselvAirport = new Airport("LKL", "Lakselv");
            _trondheimAirport = new Airport("TRD", "Trondheim");
            _flightsService = new FlightsServiceStub();
            _objectStore = new ObjectStoreStub();
            _messenger = new TinyMessengerHub();
            _dispatcher = new DispatchAdapter();
            _viewModel = new FlightsViewModel(_flightsService, _objectStore, _messenger, _dispatcher);
        }

        private Airport _lakselvAirport;
        private Airport _trondheimAirport;
        private FlightsServiceStub _flightsService;
        private FlightsViewModel _viewModel;
        private ObjectStoreStub _objectStore;
        private TinyMessengerHub _messenger;
        private DispatchAdapter _dispatcher;
    }
}
