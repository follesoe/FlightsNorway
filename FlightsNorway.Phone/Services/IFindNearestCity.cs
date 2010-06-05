using System;

namespace FlightsNorway.Phone.Services
{
    public interface IFindNearestCity
    {
        IObservable<string> GetNearestCity(double latitude, double longitude);
    }
}
