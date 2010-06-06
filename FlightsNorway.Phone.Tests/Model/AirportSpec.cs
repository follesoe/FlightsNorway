using FlightsNorway.Phone.Model;
using Microsoft.Silverlight.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FlightsNorway.Phone.Tests.Model
{
    [TestClass]
    public class AirportSpec
    {
        [TestMethod, Tag(Tags.Model)]
        public void Can_determine_that_two_airports_are_equal()
        {
            var airport1 = new Airport("LKL", "Lakselv");
            var airport2 = new Airport("LKL", "Lakselv");

            Assert.AreEqual(airport1, airport2);       
        }

        [TestMethod, Tag(Tags.Model)]
        public void Can_display_airport_as_string()
        {
            var airport = new Airport("LKL", "Lakselv");

            Assert.AreEqual("LKL - Lakselv", airport.ToString());
        }

        [TestMethod, Tag(Tags.Model)]
        public void Can_get_distance_to_airport()
        {
            var altaAirport = new Airport("ALF", "Alta", 69.9792675, 23.3570997);
            var home = new Location(63.433281, 10.419294);
            var distance = altaAirport.DistanceFrom(home);

            Assert.IsTrue(distance > 670);
        }
    }
}
