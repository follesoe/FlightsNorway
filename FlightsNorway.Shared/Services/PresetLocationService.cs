using System;
using System.Windows;
using System.Threading;
using FlightsNorway.Shared.Model;

namespace FlightsNorway.Shared.Services
{
    public class PresetLocationService : IGetCurrentLocation
    {
        public event EventHandler<EventArgs<Location>> PositionAvailable;

        private readonly Location _currentLocation;

        public PresetLocationService(double latitude, double longitude)
        {
            _currentLocation = new Location(latitude, longitude);
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
                PositionAvailable(this, new EventArgs<Location>(_currentLocation)));

        }
    }
}
