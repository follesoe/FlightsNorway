using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using FlightsNorway.Lib.Extensions;
using FlightsNorway.Lib.Model;

namespace FlightsNorway.Lib.DataServices
{
    public class FlightsService : IGetFlights
    {
        private readonly string _resourceUrl;
        private readonly string _direction;
        private readonly string _lastUpdate;

        private readonly AirportNamesService _airportService;
        private readonly AirlineNamesService _airlineService;
        private readonly StatusService _statusService;

        public AirlineDictionary Airlines { get; private set; }
        public AirportDictionary Airports { get; private set; }
        public StatusDictionary Statuses { get; private set; }

        private readonly RestHelper _rest;
        
        public FlightsService()
        {
            Airlines = new AirlineDictionary();
            Airports = new AirportDictionary();
            Statuses = new StatusDictionary();

            _airlineService = new AirlineNamesService();
            _airportService = new AirportNamesService();
            _statusService = new StatusService();

            _rest = new RestHelper();
            _resourceUrl = "XmlFeed.asp?TimeFrom={0}&TimeTo={1}&airport={2}";                       
            _lastUpdate = "&lastUpdate=2009-03-10T15:03:00";
            _direction = "&direction={0}";
        }

        public void GetFlightsFrom(Action<Result<IEnumerable<Flight>>> callback, Airport fromAirport)
        {
            if(Airports.Count == 0)
            {
                _airportService.GetAirports(r => {
                    if(r.HasError()) callback(new Result<IEnumerable<Flight>>(r.Error));
                    Airports.AddRange(r.Value);
                    GetFlightsIfAllDone(callback, fromAirport);
                });
            }

            if(Airlines.Count == 0)
            {
                _airlineService.GetAirlines(r =>
                {
                    if (r.HasError()) callback(new Result<IEnumerable<Flight>>(r.Error));
                    Airlines.AddRange(r.Value);
                    GetFlightsIfAllDone(callback, fromAirport);
                });                
            }

            if (Statuses.Count == 0)
            {
                _statusService.GetStautses(r =>
                {
                    if (r.HasError()) callback(new Result<IEnumerable<Flight>>(r.Error));
                    Statuses.AddRange(r.Value);
                    GetFlightsIfAllDone(callback, fromAirport);
                });
            }

            GetFlightsIfAllDone(callback, fromAirport);
        }

        private void GetFlightsIfAllDone(Action<Result<IEnumerable<Flight>>> callback, Airport fromAirport)
        {
            if(Airports.Count > 0 && Airlines.Count > 0 && Statuses.Count > 0)
            {
                var resource = string.Format(_resourceUrl, 1, 12, fromAirport.Code);
                _rest.Get(resource, callback, ParseFlightsXml);   
            }
        }

        private IEnumerable<Flight> ParseFlightsXml(XmlReader reader)
        {
            var xml = XDocument.Load(reader);

            return from airport in xml.Elements("airport")
                   from flights in airport.Elements("flights")
                   from flight in flights.Elements("flight")
                   select new Flight
                   {
                       FlightId = flight.ElementValueOrEmpty("flight_id"),
                       Airline = Airlines[flight.ElementValueOrEmpty("airline")],
                       Airport = Airports[flight.ElementValueOrEmpty("airport")],
                       FlightStatus = ReadStatus(flight.Element("status")),
                       UniqueId = flight.AttributeValueOrEmpty("uniqueID"),
                       Gate = flight.ElementValueOrEmpty("gate"),
                       Belt = flight.ElementValueOrEmpty("belt"),
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
