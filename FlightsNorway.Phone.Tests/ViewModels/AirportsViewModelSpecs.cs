using System.Linq;
using FlightsNorway.Phone.ViewModels;
using FlightsNorway.Phone.FlightDataServices;

using Microsoft.Silverlight.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FlightsNorway.Phone.Tests.ViewModels
{
    [TestClass]
    public class AirportsViewModelSpecs : SilverlightTest
    {
        [TestMethod, Tag("viewmodel")]
        public void Should_load_airports_when_viewmodel_is_created()
        {
            Assert.IsTrue(viewModel.Airports.Count > 0);
        }

        [TestMethod, Tag("viewmodel")]
        public void First_airport_in_list_should_be_option_to_select_nearest()
        {
            Assert.AreEqual("Nærmeste flyplass", viewModel.Airports[0].Name);
        }

        [TestMethod, Tag("viewmodel")]
        public void Fires_change_notification_when_selecting_airport()
        {
            var airport = viewModel.Airports.Last();

            string propertyName = string.Empty;
            viewModel.PropertyChanged += (o, e) => propertyName = e.PropertyName;

            viewModel.SelectedAirport = airport;

            Assert.AreEqual("SelectedAirport", propertyName);
        }

        [TestInitialize]
        public void Setup()
        {
            airportsService = new AirportNamesService();
            viewModel = new AirportsViewModel(airportsService);
        }

        private AirportNamesService airportsService;
        private AirportsViewModel viewModel;
    }
}
