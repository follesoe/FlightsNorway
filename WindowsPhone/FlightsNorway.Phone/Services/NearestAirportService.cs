using FlightsNorway.Lib.DataServices;
using FlightsNorway.Lib.Messages;
using FlightsNorway.Lib.Model;
using MonoMobile.Extensions;
using TinyMessenger;

namespace FlightsNorway.Services
{
    public class NearestAirportService
    {
        private readonly IGetAirports _airportsService;
        private readonly IGeolocation _locationService;
        private readonly ITinyMessengerHub _messenger;

        public NearestAirportService(IGeolocation locationService, ITinyMessengerHub messenger)
        {
            _locationService = locationService;
            _messenger = messenger;

            _airportsService = new AirportNamesService();
            _messenger.Subscribe<FindNearestAirportMessage>(m => FindNearestAirport());
        }

        private void FindNearestAirport()
        {
            _locationService.GetCurrentPosition(PositionAvailable);
        }

        private void PositionAvailable(Position position)
        {
            var location = new Location(position.Coords.Latitude, position.Coords.Longitude);
            _messenger.Publish(new AirportSelectedMessage(this, _airportsService.GetNearestAirport(location)));
        }
    }
}