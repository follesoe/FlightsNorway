using FlightsNorway.Phone.ViewModels;
using GalaSoft.MvvmLight.Messaging;
using Microsoft.Phone.Controls;

namespace FlightsNorway.Phone
{
    public partial class MainPage : PhoneApplicationPage
    {
        public MainPage()
        {
            InitializeComponent();

            SupportedOrientations = SupportedPageOrientation.Portrait | SupportedPageOrientation.Landscape;

            Messenger.Default.Register<AirportSelectedMessage>(this, OnAirportSelected);
        }

        private void OnAirportSelected(AirportSelectedMessage message)
        {
            _mainPivot.GoToMenu(0);
        }
    }
}