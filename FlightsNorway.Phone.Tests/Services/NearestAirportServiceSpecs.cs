using FlightsNorway.Shared.Model;
using FlightsNorway.Shared.Messages;
using FlightsNorway.Shared.Services;
using FlightsNorway.Phone.Tests.Stubs;

using GalaSoft.MvvmLight.Messaging;

using Microsoft.Silverlight.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FlightsNorway.Phone.Tests.Services
{
    [TestClass]
    public class NearestAirportServiceSpecs : SilverlightTest
    {
        [TestMethod, Asynchronous, Timeout(5000), Tag(Tags.Services)]
        public void Should_find_nearest_airport_if_user_selects_that_option()
        {
            var home = new Location(63.433281, 10.419294);
            locationService.LocationToReturn = home;

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
            locationService = new LocationServiceStub();     
            var service = new NearestAirportService(locationService);
        }
        private LocationServiceStub locationService;
    }
}