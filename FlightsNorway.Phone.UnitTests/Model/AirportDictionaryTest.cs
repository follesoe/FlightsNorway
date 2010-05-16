using FlightsNorway.Phone.Model;
using NUnit.Framework;

namespace FlightsNorway.Phone.UnitTests.Model
{
    [TestFixture]
    public class AirportDictionaryTest
    {
        [Test]
        public void Returns_airport_even_if_not_found_in_dictionary()
        {
            var airport = airports["UNK"];

            Assert.AreEqual("UNK", airport.Code);
            Assert.AreEqual("Unknown", airport.Name);
        }

        [SetUp]
        public void Setup()
        {
            airports = new AirportDictionary();
        }

        private AirportDictionary airports;
    }
}
