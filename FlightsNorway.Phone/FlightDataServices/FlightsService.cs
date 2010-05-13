using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace FlightsNorway.Phone.FlightDataServices
{
    public class FlightsService
    {
        private readonly string _serviceUrl;
        private readonly string _direction;
        private readonly string _lastUpdate;

        public FlightsService()
        {
            _serviceUrl = "http://flydata.avinor.no/XmlFeed.asp?TimeFrom={0}&TimeTo={1}&airport={2}";
            _lastUpdate = "&lastUpdate=2009-03-10T15:03:00";
            _direction = "&direction={0}";            
        }

        public IObservable<Flight> GetFlightsFrom(Airport airport)
        {
            return null;
        }
    }
}