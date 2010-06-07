using System;
using FlightsNorway.Phone.Messages;
using FlightsNorway.Phone.Model;
using FlightsNorway.Phone.ViewModels;
using GalaSoft.MvvmLight.Messaging;
using Microsoft.Phone.Shell;
using Microsoft.Phone.Controls;

namespace FlightsNorway.Phone
{
    public partial class MainPage : PhoneApplicationPage
    {
        private ApplicationBarMenuItem _subscribe;
        private ApplicationBarMenuItem _unsubscribe;
        private Flight _selectedFlight;

        public MainPage()
        {
            InitializeComponent();
           
            SupportedOrientations = SupportedPageOrientation.Portrait | SupportedPageOrientation.Landscape;

            InitializeButtons();

            Messenger.Default.Register(this, (FlightSelectedMessage m) => FlightSelected(m.Content));
        }        

        private void InitializeButtons()
        {
            ApplicationBar = new ApplicationBar();
            ApplicationBar.Opacity = 1.0;
            ApplicationBar.IsVisible = true;
            ApplicationBar.IsMenuEnabled = true;
            
            var arrivals = new ApplicationBarIconButton(new Uri("/Icons/arrivals.png", UriKind.Relative));
            var departures = new ApplicationBarIconButton(new Uri("/Icons/departures.png", UriKind.Relative));
            var airports = new ApplicationBarIconButton(new Uri("/Icons/airport.png", UriKind.Relative));
            
            ApplicationBar.Buttons.Add(arrivals);
            ApplicationBar.Buttons.Add(departures);
            ApplicationBar.Buttons.Add(airports);
            
            arrivals.Click += (o, e) => _mainPivot.SelectedItem = _arrivalsPivot;
            departures.Click += (o, e) => _mainPivot.SelectedItem = _departuresPivot;
            airports.Click += (o, e) => _mainPivot.SelectedItem = _airportsPivot;

            _subscribe = new ApplicationBarMenuItem("monitor flight");
            _unsubscribe = new ApplicationBarMenuItem("stop monitoring");

            ApplicationBar.MenuItems.Add(_subscribe);
            ApplicationBar.MenuItems.Add(_unsubscribe);
        }

        private void FlightSelected(Flight flight)
        {
            _selectedFlight = flight;
            _subscribe.Text = "monitor flight " + flight.FlightId;
        }
    }
}