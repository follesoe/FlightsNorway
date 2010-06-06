using System;
using System.Windows;
using System.Threading;
using System.Device.Location;

namespace FlightsNorway.Phone.Services
{
    public class PresetLocationService : IGetCurrentLocation
    {
        public event EventHandler<GeoPositionChangedEventArgs<GeoCoordinate>> PositionAvailable;

        private readonly GeoCoordinate _currentLocation;

        public PresetLocationService(double latitude, double longitude)
        {
            _currentLocation = new GeoCoordinate(latitude, longitude);
        }

        public void GetPositionAsync()
        {
            var t = new Thread(FireEvent);
            t.Start();
        }

        private void FireEvent()
        {
            Thread.Sleep(25);

            Deployment.Current.Dispatcher.BeginInvoke(() =>
                PositionAvailable(this,
                                new GeoPositionChangedEventArgs<GeoCoordinate>(
                                    new GeoPosition<GeoCoordinate>(DateTimeOffset.Now, _currentLocation))));

        }
    }
}
