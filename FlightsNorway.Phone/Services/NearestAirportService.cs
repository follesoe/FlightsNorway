using FlightsNorway.Messages;
using FlightsNorway.Model;
using FlightsNorway.FlightDataServices;
using GalaSoft.MvvmLight.Messaging;
using Microsoft.Phone.Reactive;

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
        }

        private void FindNearestAirport()
        {
            var positionAsObservable =
                Observable.FromEvent<EventArgs<Location>>(
                    ev => _locationService.PositionAvailable += ev,
                    ev => _locationService.PositionAvailable -= ev);

            var locations = from e in positionAsObservable
                            select new Location(e.EventArgs.Content.Latitude, 
                                                e.EventArgs.Content.Longitude);

            locations.Subscribe(l => Messenger.Default.Send(new AirportSelectedMessage(_airportsService.GetNearestAirport(l))));
            
            _locationService.GetPositionAsync();
        }
    }
}
