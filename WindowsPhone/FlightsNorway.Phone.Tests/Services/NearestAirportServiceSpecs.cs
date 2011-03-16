using FlightsNorway.Lib.Messages;
using FlightsNorway.Lib.Model;
using FlightsNorway.Lib.Services;
using FlightsNorway.Tests.Stubs;
using Microsoft.Silverlight.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TinyMessenger;

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
            _messenger.Subscribe<AirportSelectedMessage>(m => nearestAirport = m.Content);            
            _messenger.Publish(new FindNearestAirportMessage(this));
            
            EnqueueConditional(() => nearestAirport != null);
            EnqueueCallback(() => Assert.AreEqual("TRD", nearestAirport.Code));
            EnqueueTestComplete();
        }
        
        [TestInitialize]
        public void Setup()
        {
            _locationService = new LocationServiceStub();
            _messenger = new TinyMessengerHub();
            var service = new NearestAirportService(_locationService, _messenger);
        }
        
        private LocationServiceStub _locationService;
        private TinyMessengerHub _messenger;
    }
}