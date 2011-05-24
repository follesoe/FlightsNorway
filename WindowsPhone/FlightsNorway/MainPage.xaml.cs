using System;
using System.Windows;
using FlightsNorway.Lib.DataServices;
using Microsoft.Phone.Controls;

namespace FlightsNorway
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Constructor
        public MainPage()
        {
            InitializeComponent();
            Loaded += MainPage_Loaded;
        }

        private void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            var service = new AirportNamesService();

            var fileUri = new Uri("FlightsNorway;component/Content/Airports.xml",
                                    UriKind.Relative);

            using (var stream = Application.GetResourceStream(fileUri).Stream)
            {
                _airports.ItemsSource = service.GetNorwegianAirports(stream);
            }
        }
    }
}