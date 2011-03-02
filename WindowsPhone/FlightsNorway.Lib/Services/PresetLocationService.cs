using System;
using System.Threading;
using FlightsNorway.Lib.Model;
using MonoMobile.Extensions;

#if MONOTOUCH
using MonoTouch.Foundation;
#else
using System.Windows;
#endif

namespace FlightsNorway.Lib.Services
{
    public class PresetLocationService : IGeolocation
    {
        private readonly Location _currentLocation;

        public PresetLocationService(double latitude, double longitude)
        {
            _currentLocation = new Location(latitude, longitude);
        }

        public void GetPositionAsync(Action<Position> success)
        {
            var t = new Thread(() => FireEvent(success));
            t.Start();
        }

        private void FireEvent(Action<Position> callback)
        {
            Thread.Sleep(25);
			

            InvokeOnUIThread(() =>
                          {
                              var position = new Position();
                              position.Coords.Latitude = _currentLocation.Latitude;
                              position.Coords.Longitude = _currentLocation.Longitude;
                              callback(position);
                          });
        }
		
		private void InvokeOnUIThread(Action action)
		{
			#if MONOTOUCH			 
			InvokeOnMainThread(action);
			#else
			Deployment.Current.Dispatcher.BeginInvoke(action);
			#endif
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
