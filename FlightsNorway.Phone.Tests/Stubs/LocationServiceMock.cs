using System;
using System.Windows;
using System.Threading;
using System.Device.Location;

namespace FlightsNorway.Phone.Tests.Stubs
{
    public class LocationServiceMock : IGetCurrentLocation
    {
        public event EventHandler<GeoPositionChangedEventArgs<GeoCoordinate>> PositionAvailable;

        public GeoCoordinate GeoCoordinateToReturn;

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
                                    new GeoPosition<GeoCoordinate>(DateTimeOffset.Now, GeoCoordinateToReturn))));

        }
    }
}
