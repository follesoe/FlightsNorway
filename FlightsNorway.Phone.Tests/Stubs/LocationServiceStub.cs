using System;
using System.Windows;
using System.Threading;
using FlightsNorway.Model;
using MonoMobile.Extensions;

namespace FlightsNorway.Tests.Stubs
{
    public class LocationServiceStub : IGeolocation
    {
        public Location LocationToReturn;

        public void GetPositionAsync(Action<Position> callback)
        {
            var t = new Thread(() => FireEvent(callback));
            t.Start();
        }

        private void FireEvent(Action<Position> callback)
        {
            Thread.Sleep(25);

            Deployment.Current.Dispatcher.BeginInvoke(() =>
            {
                var position = new Position();
                position.Coords.Latitude = LocationToReturn.Latitude;
                position.Coords.Longitude = LocationToReturn.Longitude;
                callback(position);
            });
        }

        public void GetCurrentPosition(Action<Position> success)
        {
            GetPositionAsync(success);
        }

        public void GetCurrentPosition(Action<Position> success, Action<PositionError> error)
        {
            GetPositionAsync(success);
        }

        public void GetCurrentPosition(Action<Position> success, Action<PositionError> error, GeolocationOptions options)
        {
            GetPositionAsync(success);
        }

        public string WatchPosition(Action<Position> success)
        {
            throw new NotImplementedException();
        }

        public string WatchPosition(Action<Position> success, Action<PositionError> error)
        {
            throw new NotImplementedException();
        }

        public string WatchPosition(Action<Position> success, Action<PositionError> error, GeolocationOptions options)
        {
            throw new NotImplementedException();
        }

        public void ClearWatch(string watchID)
        {
            throw new NotImplementedException();
        }
    }
}
