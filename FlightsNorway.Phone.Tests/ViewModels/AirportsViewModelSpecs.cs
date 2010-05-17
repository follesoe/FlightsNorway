using System.Linq;
using FlightsNorway.Phone.Model;
using FlightsNorway.Phone.Services;
using FlightsNorway.Phone.ViewModels;
using FlightsNorway.Phone.FlightDataServices;

using GalaSoft.MvvmLight.Messaging;

using Microsoft.Silverlight.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FlightsNorway.Phone.Tests.ViewModels
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
            objectStore.Delete("selected_airport");
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
        public void Publishes_message_when_user_saves_selected_airport()
        {
            AirportSelectedMessage lastMessage = null;
            Messenger.Default.Register(this, (AirportSelectedMessage message) => lastMessage = message);

            viewModel.SelectedAirport = viewModel.Airports.First();
            viewModel.SaveCommand.Execute(null);

            Assert.IsNotNull(lastMessage);
            Assert.AreEqual(viewModel.SelectedAirport, lastMessage.Content);
        }

        [TestInitialize]
        public void Setup()
        {
            objectStore = new ObjectStore();
            airportsService = new AirportNamesService();
            viewModel = new AirportsViewModel(airportsService);
        }

        private AirportNamesService airportsService;
        private AirportsViewModel viewModel;
        private ObjectStore objectStore;
    }
}
