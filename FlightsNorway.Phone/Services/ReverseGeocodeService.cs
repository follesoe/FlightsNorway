using System;
using FlightsNorway.Phone.GeocodeService;

namespace FlightsNorway.Phone.Services
{
    public class ReverseGeocodeService : IDoReverseGeocoding
    {
        public IObservable<string> GetNearestCity(double latitude, double longitude)
        {
            var key = "AlAQxDGizgbfv7yU1ACvozNqosoM9O5iTcsP01jqM-oX3c_OJRb1YgrAOMYMRQZB";
            var reverseGeocodeRequest = new ReverseGeocodeRequest();

            reverseGeocodeRequest.Credentials = new Credentials();
            reverseGeocodeRequest.Credentials.ApplicationId = key;

            var point = new Location();
            point.Latitude = latitude;
            point.Longitude = longitude;
            reverseGeocodeRequest.Location = point;

            var geocodeService = new GeocodeServiceClient();

            return null;   
        }
    }
}
