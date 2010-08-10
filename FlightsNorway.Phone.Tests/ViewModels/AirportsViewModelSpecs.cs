using System.Linq;
using FlightsNorway.Messages;
using FlightsNorway.Model;
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
            Assert.IsTrue(viewModel.Airports.Count > 0);
        }

        [TestMethod, Tag(Tags.ViewModel)]
        public void First_airport_in_list_should_be_option_to_select_nearest()
        {
            Assert.AreEqual("Nærmeste flyplass", viewModel.Airports[0].Name);
        }

        [TestMethod, Tag(Tags.ViewModel)]
        public void Saves_selected_airport()
        {
            objectStore.Delete(ObjectStore.SelectedAirportFilename);
            viewModel.SelectedAirport = viewModel.Airports.Last();
            viewModel.SaveCommand.Execute(null);

            var airport = objectStore.Load<Airport>("selected_airport");

            Assert.AreEqual(viewModel.SelectedAirport, airport);
        }

        [TestMethod, Tag(Tags.ViewModel)]
        public void Fires_change_notification_when_selecting_airport()
        {
            var airport = viewModel.Airports.Last();

            string propertyName = string.Empty;
            viewModel.PropertyChanged += (o, e) => propertyName = e.PropertyName;

            viewModel.SelectedAirport = airport;

            Assert.AreEqual("SelectedAirport", propertyName);
        }
        
        [TestMethod, Tag(Tags.ViewModel)]
        public void Publishes_message_when_user_selects_an_airport()
        {
            AirportSelectedMessage lastMessage = null;
            Messenger.Default.Register(this, (AirportSelectedMessage message) => lastMessage = message);

            viewModel.SelectedAirport = viewModel.Airports.Last();
            viewModel.SaveCommand.Execute(null);

            Assert.IsNotNull(lastMessage);
            Assert.AreEqual(viewModel.SelectedAirport, lastMessage.Content);
        }

        [TestMethod, Tag(Tags.ViewModel)]
        public void Publishes_message_when_user_selects_nearest_airport()
        {
            FindNearestAirportMessage lastMessage = null;
            Messenger.Default.Register(this, (FindNearestAirportMessage message) => lastMessage = message);

            viewModel.SelectedAirport = viewModel.Airports.Where(a => a.Code == Airport.Nearest.Code).Single();
            viewModel.SaveCommand.Execute(null);

            Assert.IsNotNull(lastMessage);
        }

        [TestInitialize]
        public void Setup()
        {
            objectStore = new ObjectStoreStub();
            airportsService = new AirportNamesService();
            viewModel = new AirportsViewModel(airportsService, objectStore);
        }

        private AirportNamesService airportsService;
        private AirportsViewModel viewModel;
        private ObjectStoreStub objectStore;
    }
}
