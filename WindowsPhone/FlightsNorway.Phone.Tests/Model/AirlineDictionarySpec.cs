using FlightsNorway.Lib.Model;
using Microsoft.Silverlight.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FlightsNorway.Tests.Model
{
    [TestClass]
    public class AirlineDictionaryTest
    {
        [TestMethod, Tag(Tags.Model)]
        public void Returns_airline_even_if_not_found_in_dictionary()
        {
            var airline = _airlines["UNK"];

            Assert.AreEqual("UNK", airline.Code);
            Assert.AreEqual("Unknown", airline.Name);
        }

        [TestInitialize]
        public void Setup()
        {
            _airlines = new AirlineDictionary();
        }

        private AirlineDictionary _airlines;
    }
}
