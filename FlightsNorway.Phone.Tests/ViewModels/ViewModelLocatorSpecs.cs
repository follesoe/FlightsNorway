using Microsoft.Silverlight.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FlightsNorway.Phone.Tests.ViewModels
{
    [TestClass]
    public class ViewModelLocatorSpecs
    {
        [TestMethod, Tag(Tags.ViewModel)]
        public void Airports_ViewModel_is_singleton()
        {
            var airports1 = locator.AirportsViewModel;
            var airports2 = locator.AirportsViewModel;
            Assert.AreSame(airports1, airports2);
        }

        [TestMethod, Tag(Tags.ViewModel)]
        public void Flights_ViewModel_is_singleton()
        {
            var flights1 = locator.FlightsViewModel;
            var flights2 = locator.FlightsViewModel;
            Assert.AreSame(flights1, flights2);
        }

        [TestInitialize]
        public void Setup()
        {
            locator = new ViewModelLocator();
        }

        private ViewModelLocator locator;
    }
}
