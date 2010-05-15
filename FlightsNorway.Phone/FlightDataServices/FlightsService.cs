using System;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using System.Collections.Generic;

using FlightsNorway.Phone.Model;

namespace FlightsNorway.Phone.FlightDataServices
{
    public class FlightsService : IGetFlights
    {
        private readonly string _serviceUrl;
        private readonly string _direction;
        private readonly string _lastUpdate;

        public AirlineDictionary Airlines { get; private set; }
        public AirportDictionary Airports { get; private set; }
        public StatusDictionary Statuses { get; private set; }

        private static event EventHandler ReferenceDataLoaded;

        private bool airlinesLoaded, airportsLoaded, statusesLoaded;

        public FlightsService()
        {
            Airlines = new AirlineDictionary();
            Airports = new AirportDictionary();
            Statuses = new StatusDictionary();

            _serviceUrl = "http://flydata.avinor.no/XmlFeed.asp?TimeFrom={0}&TimeTo={1}&airport={2}";
            _lastUpdate = "&lastUpdate=2009-03-10T15:03:00";
            _direction = "&direction={0}";
        }

        public IObservable<IEnumerable<Flight>> GetFlightsFrom(Airport fromAirport)
        {            
            var whenReferenceDataLoaded = Observable.FromEvent(
                                            (EventHandler<EventArgs> ev) => new EventHandler(ev),
                                                ev => ReferenceDataLoaded += ev,
                                                ev => ReferenceDataLoaded -= ev);

            string url = string.Format(_serviceUrl, 1, 7, fromAirport.Code);

            var flights = from isDone in whenReferenceDataLoaded
                          where isDone != null
                          from flight in GetFlightsFrom(url)
                          select flight;

            LoadReferenceData();

            return flights;
        }       

        private void LoadReferenceData()
        {
            var airports = new AirportNamesService().GetAirports();
            var airlines = new AirlineNamesService().GetAirlines();
            var statuses = new StatusService().GetStautses();

            airports.Subscribe(a => Airports.AddRange(a), () => { airportsLoaded = true; OnReferenceDataLoaded(); });
            airlines.Subscribe(a => Airlines.AddRange(a), () => { airlinesLoaded = true; OnReferenceDataLoaded(); });
            statuses.Subscribe(s => Statuses.AddRange(s), () => { statusesLoaded = true; OnReferenceDataLoaded(); });
        }

        private void OnReferenceDataLoaded()
        {
            if(airportsLoaded && airlinesLoaded && statusesLoaded)
            {
                if (ReferenceDataLoaded != null)
                    ReferenceDataLoaded(this, EventArgs.Empty);
            }
        }

        private IObservable<IEnumerable<Flight>> GetFlightsFrom(string url)
        {
            return WebRequestFactory.GetData(new Uri(url), ParseFlightsXml);
        }

        private IEnumerable<Flight> ParseFlightsXml(XmlReader reader)
        {
            var xml = XDocument.Load(reader);

            return from airport in xml.Elements("airport")
                   from flights in airport.Elements("flights")
                   from flight in flights.Elements("flight")
                   select new Flight
                   {
                       Airline = Airlines[flight.ElementValueOrEmpty("airline")],
                       Airport = Airports[flight.ElementValueOrEmpty("airport")],
                       Status = ReadStatus(flight.Element("status")),
                       UniqueId = flight.AttributeValueOrEmpty("uniqueID"),
                       Gate = flight.ElementValueOrEmpty("gate"),
                       Direction = ConvertToDirection(flight.Element("arr_dep")),
                       LastLeg = ConvertToFlightArea(flight.Element("dom_int")),
                       ScheduledTime = flight.ElementAsDateTime("schedule_time")
                   };
        }

        private FlightStatus ReadStatus(XElement element)
        {
            if (element == null) return FlightStatus.Empty;

            var flightStatus = new FlightStatus();
            flightStatus.Status = Statuses[element.AttributeValueOrEmpty("code")];
            flightStatus.StatusTime = element.AttributeAsDateTime("time");
            return flightStatus;
        }

        private static Direction ConvertToDirection(XElement element)
        {
            if (element == null) return Direction.Depature;
            return element.Value == "D" ? Direction.Depature : Direction.Arrival;
        }

        private static FlightArea ConvertToFlightArea(XElement element)
        {
            if (element == null) return FlightArea.Domestic;
            if (element.Value == "S") return FlightArea.Schengen;
            if (element.Value == "I") return FlightArea.International;
            return FlightArea.Domestic;
        }
    }
}
