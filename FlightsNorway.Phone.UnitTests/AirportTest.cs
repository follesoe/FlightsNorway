using FlightsNorway.Phone.Model;
using NUnit.Framework;

namespace FlightsNorway.Phone.UnitTests
{
    [TestFixture]
    public class AirportTest
    {
        [Test]
        public void Can_determine_that_two_airports_are_equal()
        {
            var airport1 = new Airport {Code = "LKL", Name = "Lakselv"};
            var airport2 = new Airport { Code = "LKL", Name = "Lakselv" };

             Assert.AreEqual(airport1, airport2);       
        }
    }
}
