using System;
using System.Device.Location;

namespace FlightsNorway.Phone
{
    public interface IGetCurrentLocation
    {
        event EventHandler<GeoPositionChangedEventArgs<GeoCoordinate>> PositionAvailable;
        void GetPositionAsync();
    }
}
