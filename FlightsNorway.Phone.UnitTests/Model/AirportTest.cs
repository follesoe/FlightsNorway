using FlightsNorway.Phone.Model;
using NUnit.Framework;

namespace FlightsNorway.Phone.UnitTests.Model
{
    [TestFixture]
    public class AirportTest
    {
        [Test]
        public void Can_determine_that_two_airports_are_equal()
        {
            var airport1 = new Airport("LKL", "Lakselv");
            var airport2 = new Airport("LKL", "Lakselv");

            Assert.AreEqual(airport1, airport2);       
        }

        [Test]
        public void Can_display_airport_as_string()
        {
            var airport = new Airport("LKL", "Lakselv");

            Assert.AreEqual("LKL - Lakselv", airport.ToString());
        }
    }
}
