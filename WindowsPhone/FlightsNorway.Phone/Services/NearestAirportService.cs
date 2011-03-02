using FlightsNorway.Lib.DataServices;
using FlightsNorway.Lib.Model;
using FlightsNorway.Messages;
using GalaSoft.MvvmLight.Messaging;
using MonoMobile.Extensions;

namespace FlightsNorway.Services
{
    public class NearestAirportService
    {
        private readonly IGetAirports _airportsService;
        private readonly IGeolocation _locationService;
        
        public NearestAirportService(IGeolocation locationService)
        {
            _locationService = locationService;
      
            _airportsService = new AirportNamesService();
            Messenger.Default.Register(this, (FindNearestAirportMessage m) => FindNearestAirport());
        }

        private void FindNearestAirport()
        {
            _locationService.GetCurrentPosition(PositionAvailable);
        }

        private void PositionAvailable(Position position)
        {
            var location = new Location(position.Coords.Latitude, position.Coords.Longitude);
            Messenger.Default.Send(new AirportSelectedMessage(_airportsService.GetNearestAirport(location)));
        }
    }
}