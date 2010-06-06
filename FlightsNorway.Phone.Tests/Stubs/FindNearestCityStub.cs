using System;
using System.Linq;
using FlightsNorway.Phone.Services;

namespace FlightsNorway.Phone.Tests.Stubs
{
    public class DoReverseGeocodingStub : IDoReverseGeocoding
    {
        public string NearestCityToReturn;

        public IObservable<string> GetNearestCity(double latitude, double longitude)
        {
            return new[] {NearestCityToReturn}.ToObservable();
        }
    }
}