using FlightsNorway.Phone.FlightDataServices;

using Microsoft.Silverlight.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FlightsNorway.Phone.Tests.FlightDataServiceTest
{
    [TestClass]
    public class FlightsServiceTest : SilverlightTest
    {
        [TestInitialize]
        public void Setup()
        {
            service = new FlightsService();
        }

        private FlightsService service;
    }
}
