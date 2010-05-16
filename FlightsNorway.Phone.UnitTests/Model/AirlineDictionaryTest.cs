using FlightsNorway.Phone.Model;
using NUnit.Framework;

namespace FlightsNorway.Phone.UnitTests.Model
{
    [TestFixture]
    public class AirlineDictionaryTest
    {
        [Test]
        public void Returns_airline_even_if_not_found_in_dictionary()
        {
            var airline = airlines["UNK"];

            Assert.AreEqual("UNK", airline.Code);
            Assert.AreEqual("Unknown", airline.Name);
        }

        [SetUp]
        public void Setup()
        {
            airlines = new AirlineDictionary();
        }

        private AirlineDictionary airlines;
    }
}
