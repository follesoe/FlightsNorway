using FlightsNorway.Model;
using Microsoft.Silverlight.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FlightsNorway.Tests.Model
{
    [TestClass]
    public class LocationSpec
    {
        [TestMethod, Tag(Tags.Model)]
        public void Can_get_the_distance_between_two_positions()
        {
            var posA = new Location(70.0, 24.0);
            var posB = new Location(65, 15);

            double distance = posA.DistanceTo(posB);
            Assert.IsTrue(distance > 670);
        }
    }
}
