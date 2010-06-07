using System;
using Microsoft.Phone.Shell;
using Microsoft.Phone.Controls;

namespace FlightsNorway.Phone
{
    public partial class MainPage : PhoneApplicationPage
    {
        public MainPage()
        {
            InitializeComponent();
           
            SupportedOrientations = SupportedPageOrientation.Portrait | SupportedPageOrientation.Landscape;

            InitializeButtons();
        }

        private void InitializeButtons()
        {
            ApplicationBar = new ApplicationBar();
            ApplicationBar.Opacity = 1.0;
            ApplicationBar.IsVisible = true;

            var arrivals = new ApplicationBarIconButton(new Uri("/Icons/arrivals.png", UriKind.Relative));
            var departures = new ApplicationBarIconButton(new Uri("/Icons/departures.png", UriKind.Relative));
            var airports = new ApplicationBarIconButton(new Uri("/Icons/airport.png", UriKind.Relative));

            ApplicationBar.Buttons.Add(arrivals);
            ApplicationBar.Buttons.Add(departures);
            ApplicationBar.Buttons.Add(airports);

            arrivals.Click += (o, e) => _mainPivot.SelectedItem = _arrivalsPivot;
            departures.Click += (o, e) => _mainPivot.SelectedItem = _departuresPivot;
            airports.Click += (o, e) => _mainPivot.SelectedItem = _airportsPivot;

        }
    }
}