using System.Device.Location;
using FlightsNorway.Phone.Model;
using FlightsNorway.Phone.Services;
using FlightsNorway.Phone.ViewModels;
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
            _doReverseGeocoding.NearestCityToReturn = "Lakselv";
            locationService.GeoCoordinateToReturn = new GeoCoordinate(70.06, 24.97);

            Airport nearestAirport = null;
            Messenger.Default.Register(this, (AirportSelectedMessage e) => nearestAirport = e.Content);
            Messenger.Default.Send(new FindNearestAirportMessage());

            EnqueueConditional(() => nearestAirport != null);
            EnqueueCallback(() => Assert.AreEqual("LKL", nearestAirport.Code));
            EnqueueTestComplete();
        }
        
        [TestInitialize]
        public void Setup()
        {
            _doReverseGeocoding = new DoReverseGeocodingStub();
            locationService = new LocationServiceMock();     
            var service = new NearestAirportService(locationService, _doReverseGeocoding);
        }
        
        private DoReverseGeocodingStub _doReverseGeocoding;
        private LocationServiceMock locationService;
    }
}