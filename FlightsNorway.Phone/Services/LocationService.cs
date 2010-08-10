using System;
using System.Windows;
using System.Device.Location;
using FlightsNorway.Model;

namespace FlightsNorway.Services
{
    public class LocationService : IGetCurrentLocation
    {
        private readonly GeoCoordinateWatcher _geoWatcher;

        public event EventHandler<EventArgs<Location>> PositionAvailable;

        public LocationService()
        {
            _geoWatcher = new GeoCoordinateWatcher(GeoPositionAccuracy.Default);
            
            _geoWatcher.StatusChanged += OnStatusChanged;           
        }

        public void GetPositionAsync()
        {
            if(_geoWatcher.Status == GeoPositionStatus.Disabled)
            {
                _geoWatcher.Start();
            }
        }

        private void OnStatusChanged(object sender, GeoPositionStatusChangedEventArgs e)
        {
            switch(e.Status)
            {
                case GeoPositionStatus.NoData:
                    OnPositionAvailable(new GeoPosition<GeoCoordinate>(DateTime.Now, GeoCoordinate.Unknown));
                    _geoWatcher.Stop();
                    break;
                case GeoPositionStatus.Ready:
                    OnPositionAvailable(_geoWatcher.Position);
                    break;
                default:
                    break;
            }
            _geoWatcher.Stop();
        }

        private void OnPositionAvailable(GeoPosition<GeoCoordinate> position)
        {
            if (PositionAvailable != null)
            {
                Deployment.Current.Dispatcher.BeginInvoke(() =>
                        PositionAvailable(this, new EventArgs<Location>(new Location(position.Location.Latitude, position.Location.Longitude))));
            }
        }
    }
}