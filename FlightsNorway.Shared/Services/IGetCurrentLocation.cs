using System;

namespace FlightsNorway.Shared.Services
{
    public interface IGetCurrentLocation
    {
        //event EventHandler<GeoPositionChangedEventArgs<GeoCoordinate>> PositionAvailable;
        void GetPositionAsync();
    }
}
