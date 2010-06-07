using System;
using System.Linq;
using System.Device.Location;
using FlightsNorway.Phone.Messages;
using FlightsNorway.Phone.Model;
using FlightsNorway.Phone.ViewModels;
using FlightsNorway.Phone.FlightDataServices;

using GalaSoft.MvvmLight.Messaging;

namespace FlightsNorway.Phone.Services
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
                Observable.FromEvent<GeoPositionChangedEventArgs<GeoCoordinate>>(
                    ev => _locationService.PositionAvailable += ev,
                    ev => _locationService.PositionAvailable -= ev);

            var locations = from e in positionAsObservable
                            select new Location(e.EventArgs.Position.Location.Latitude, 
                                                e.EventArgs.Position.Location.Longitude);

            locations.Subscribe(l => Messenger.Default.Send(new AirportSelectedMessage(_airportsService.GetNearestAirport(l))));
            
            _locationService.GetPositionAsync();
        }
    }
}
