using FlightsNorway.Model;
using Microsoft.Silverlight.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FlightsNorway.Tests.Model
{
    [TestClass]
    public class AirportDictionarySpec
    {
        [TestMethod, Tag(Tags.Model)]
        public void Returns_airport_even_if_not_found_in_dictionary()
        {
            var airport = _airports["UNK"];

            Assert.AreEqual("UNK", airport.Code);
            Assert.AreEqual("Unknown", airport.Name);
        }

        [TestInitialize]
        public void Setup()
        {
            _airports = new AirportDictionary();
        }

        private AirportDictionary _airports;
    }
}
