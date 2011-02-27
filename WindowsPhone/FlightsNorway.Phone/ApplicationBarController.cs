using System;
using FlightsNorway.Messages;
using FlightsNorway.Model;
using Microsoft.Phone.Shell;
using GalaSoft.MvvmLight.Messaging;

namespace FlightsNorway
{
    public class ApplicationBarController
    {        
        private readonly IApplicationBar _applicationBar;
        private ApplicationBarMenuItem _subscribe;
        private ApplicationBarMenuItem _unsubscribe;
        private Flight _selectedFlight;

        public ApplicationBarController(IApplicationBar applicationBar)
        {
            _applicationBar = applicationBar;
            InitializeButtons();
            Messenger.Default.Register(this, (FlightSelectedMessage m) => FlightSelected(m.Content));
        }

        private void FlightSelected(Flight flight)
        {
            _selectedFlight = flight;
            _subscribe.Text = "monitor flight " + flight.FlightId;
        }

        private void InitializeButtons()
        {
            _applicationBar.Opacity = 1.0;
            _applicationBar.IsVisible = true;
            _applicationBar.IsMenuEnabled = true;

            var arrivals = new ApplicationBarIconButton(new Uri("/Icons/arrivals.png", UriKind.Relative));
            var departures = new ApplicationBarIconButton(new Uri("/Icons/departures.png", UriKind.Relative));
            var airports = new ApplicationBarIconButton(new Uri("/Icons/airport.png", UriKind.Relative));

            arrivals.Text = "arrivals";
            departures.Text = "departures";
            airports.Text = "airports";

            _applicationBar.Buttons.Add(arrivals);
            _applicationBar.Buttons.Add(departures);
            _applicationBar.Buttons.Add(airports);

            _subscribe = new ApplicationBarMenuItem("monitor flight");
            _unsubscribe = new ApplicationBarMenuItem("stop monitoring");

            _applicationBar.MenuItems.Add(_subscribe);
            _applicationBar.MenuItems.Add(_unsubscribe);

            _subscribe.Click += (o, e) =>
            {
                if (_selectedFlight != null)
                {
                    Messenger.Default.Send(new MonitorFlightMessage(_selectedFlight));
                }
            };

            _unsubscribe.Click += (o, e) =>
            {
                if (_selectedFlight != null)
                {
                    Messenger.Default.Send(new StopMonitorFlightMessage(_selectedFlight));
                }
            };
        }
    }
}
