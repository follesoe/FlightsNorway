using FlightsNorway.Lib.Model;
using FlightsNorway.Lib.MVVM;
using FlightsNorway.Messages;
using FlightsNorway.Services;
using FlightsNorway.Tests.Stubs;
using Microsoft.Silverlight.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FlightsNorway.Tests.Services
{
    [TestClass]
    public class NearestAirportServiceSpecs : SilverlightTest
    {
        [TestMethod, Asynchronous, Timeout(5000), Tag(Tags.Services)]
        public void Should_find_nearest_airport_if_user_selects_that_option()
        {
            var home = new Location(63.433281, 10.419294);
            _locationService.LocationToReturn = home;

            Airport nearestAirport = null;
            Messenger.Default.Register(this, (AirportSelectedMessage e) => nearestAirport = e.Content);
            Messenger.Default.Send(new FindNearestAirportMessage());

            EnqueueConditional(() => nearestAirport != null);
            EnqueueCallback(() => Assert.AreEqual("TRD", nearestAirport.Code));
            EnqueueTestComplete();
        }
        
        [TestInitialize]
        public void Setup()
        {
            _locationService = new LocationServiceStub();     
            var service = new NearestAirportService(_locationService);
        }
        private LocationServiceStub _locationService;
    }
}