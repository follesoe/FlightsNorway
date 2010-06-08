using FlightsNorway.Shared.Model;
using Microsoft.Silverlight.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FlightsNorway.Phone.Tests.Model
{
    [TestClass]
    public class AirportDictionarySpec
    {
        [TestMethod, Tag(Tags.Model)]
        public void Returns_airport_even_if_not_found_in_dictionary()
        {
            var airport = airports["UNK"];

            Assert.AreEqual("UNK", airport.Code);
            Assert.AreEqual("Unknown", airport.Name);
        }

        [TestInitialize]
        public void Setup()
        {
            airports = new AirportDictionary();
        }

        private AirportDictionary airports;
    }
}
