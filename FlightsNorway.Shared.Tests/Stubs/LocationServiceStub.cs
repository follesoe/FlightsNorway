using System;
using System.Windows;
using System.Threading;
using FlightsNorway.Shared.Model;
using FlightsNorway.Shared.Services;

namespace FlightsNorway.Shared.Tests.Stubs
{
    public class LocationServiceStub : IGetCurrentLocation
    {
        public event EventHandler<EventArgs<Location>> PositionAvailable;

        public Location LocationToReturn;

        public void GetPositionAsync()
        {
            var t = new Thread(FireEvent);
            t.Start();
        }

        private void FireEvent()
        {
            Thread.Sleep(25);

            Deployment.Current.Dispatcher.BeginInvoke(() =>
                PositionAvailable(this, new EventArgs<Location>(LocationToReturn)));

        }
    }
}
