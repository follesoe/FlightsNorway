using System;

namespace FlightsNorway.Phone.Services
{
    public class LocationLookupService : IFindNearestCity
    {
        public IObservable<string> GetNearestCity(double latitude, double longitude)
        {
            return null;   
        }
    }
}
