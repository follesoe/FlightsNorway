using System;
using System.Linq;
using System.Device.Location;

using FlightsNorway.Phone.ViewModels;
using FlightsNorway.Phone.FlightDataServices;

using GalaSoft.MvvmLight.Messaging;

namespace FlightsNorway.Phone.Services
{
    public class NearestAirportService
    {
        private readonly IGetAirports _airportsService;
        private readonly IDoReverseGeocoding _geocodeService;
        private readonly IGetCurrentLocation _locationService;
        
        public NearestAirportService(IGetCurrentLocation locationService, IDoReverseGeocoding geocodeService)
        {
            _locationService = locationService;
            _geocodeService = geocodeService;
            _airportsService = new AirportNamesService();
            Messenger.Default.Register(this, (FindNearestAirportMessage m) => FindNearestAirport());
        }

        public void FindNearestAirport()
        {
            var positionAsObservable =
                Observable.FromEvent<GeoPositionChangedEventArgs<GeoCoordinate>>(
                    ev => _locationService.PositionAvailable += ev,
                    ev => _locationService.PositionAvailable -= ev);

            var locations = from e in positionAsObservable
                            select e.EventArgs.Position.Location;

            var airports = from location in locations
                           from city in _geocodeService.GetNearestCity(location.Latitude, location.Longitude)
                           from airport in _airportsService.GetNorwegianAirports().ToObservable()
                           where airport.Name == city
                           select airport;

            airports.Subscribe(a => Messenger.Default.Send(new AirportSelectedMessage(a)));
            
            _locationService.GetPositionAsync();
        }
    }
}
