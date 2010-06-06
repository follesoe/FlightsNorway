using System;

namespace FlightsNorway.Phone.Services
{
    public interface IDoReverseGeocoding
    {
        IObservable<string> GetNearestCity(double latitude, double longitude);
    }
}
