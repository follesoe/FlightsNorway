using System.Linq;
using FlightsNorway.Lib.Model;
using FlightsNorway.Messages;
using FlightsNorway.Services;
using FlightsNorway.Tests.Stubs;
using FlightsNorway.ViewModels;
using FlightsNorway.FlightDataServices;
using GalaSoft.MvvmLight.Messaging;
using Microsoft.Silverlight.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FlightsNorway.Tests.ViewModels
{
    [TestClass]
    public class AirportsViewModelSpecs : SilverlightTest
    {
        [TestMethod, Tag(Tags.ViewModel)]
        public void Should_load_airports_when_viewmodel_is_created()
        {
            Assert.IsTrue(_viewModel.Airports.Count > 0);
        }

        [TestMethod, Tag(Tags.ViewModel)]
        public void First_airport_in_list_should_be_option_to_select_nearest()
        {
            Assert.AreEqual("Nærmeste flyplass", _viewModel.Airports[0].Name);
        }

        [TestMethod, Tag(Tags.ViewModel)]
        public void Saves_selected_airport()
        {
            _objectStore.Delete(ObjectStore.SelectedAirportFilename);
            _viewModel.SelectedAirport = _viewModel.Airports.Last();
            _viewModel.SaveCommand.Execute(null);

            var airport = _objectStore.Load<Airport>("selected_airport");

            Assert.AreEqual(_viewModel.SelectedAirport, airport);
        }

        [TestMethod, Tag(Tags.ViewModel)]
        public void Fires_change_notification_when_selecting_airport()
        {
            var airport = _viewModel.Airports.Last();

            string propertyName = string.Empty;
            _viewModel.PropertyChanged += (o, e) => propertyName = e.PropertyName;

            _viewModel.SelectedAirport = airport;

            Assert.AreEqual("SelectedAirport", propertyName);
        }
        
        [TestMethod, Tag(Tags.ViewModel)]
        public void Publishes_message_when_user_selects_an_airport()
        {
            AirportSelectedMessage lastMessage = null;
            Messenger.Default.Register(this, (AirportSelectedMessage message) => lastMessage = message);

            _viewModel.SelectedAirport = _viewModel.Airports.Last();
            _viewModel.SaveCommand.Execute(null);

            Assert.IsNotNull(lastMessage);
            Assert.AreEqual(_viewModel.SelectedAirport, lastMessage.Content);
        }

        [TestMethod, Tag(Tags.ViewModel)]
        public void Publishes_message_when_user_selects_nearest_airport()
        {
            FindNearestAirportMessage lastMessage = null;
            Messenger.Default.Register(this, (FindNearestAirportMessage message) => lastMessage = message);

            _viewModel.SelectedAirport = _viewModel.Airports.Where(a => a.Code == Airport.Nearest.Code).Single();
            _viewModel.SaveCommand.Execute(null);

            Assert.IsNotNull(lastMessage);
        }

        [TestInitialize]
        public void Setup()
        {
            _objectStore = new ObjectStoreStub();
            _airportsService = new AirportNamesService();
            _viewModel = new AirportsViewModel(_airportsService, _objectStore);
        }

        private AirportNamesService _airportsService;
        private AirportsViewModel _viewModel;
        private ObjectStoreStub _objectStore;
    }
}
