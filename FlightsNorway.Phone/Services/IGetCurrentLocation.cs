using System;
using System.Device.Location;

namespace FlightsNorway.Phone.Services
{
    public interface IGetCurrentLocation
    {
        event EventHandler<GeoPositionChangedEventArgs<GeoCoordinate>> PositionAvailable;
        void GetPositionAsync();
    }
}
