using FlightsNorway.Model;
using FlightsNorway.Messages;
using FlightsNorway.FlightDataServices;
using GalaSoft.MvvmLight.Messaging;

namespace FlightsNorway.Services
{
    public class NearestAirportService
    {
        private readonly IGetAirports _airportsService;
        private readonly IGetCurrentLocation _locationService;
        
        public NearestAirportService(IGetCurrentLocation locationService)
        {
            _locationService = locationService;
      
            _airportsService = new AirportNamesService();
            Messenger.Default.Register(this, (FindNearestAirportMessage m) => FindNearestAirport());
            _locationService.PositionAvailable += PositionAvailable;
        }

        private void FindNearestAirport()
        {            
            _locationService.GetPositionAsync();
        }

        private void PositionAvailable(object sender, EventArgs<Location> e)
        {
            var location = new Location(e.Content.Latitude, e.Content.Longitude);
            Messenger.Default.Send(new AirportSelectedMessage(_airportsService.GetNearestAirport(location)));
        }
    }
}