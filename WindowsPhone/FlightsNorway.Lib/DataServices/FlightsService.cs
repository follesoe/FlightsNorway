using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using FlightsNorway.Lib.Model;

namespace FlightsNorway.Lib.DataServices
{
    public class FlightsService : RestService<Flight>
    {
        private readonly string _resourceUrl;

        private readonly AirportNamesService _airportService;
        private readonly AirlineNamesService _airlineService;
        private readonly StatusService _statusService;

        public CodeNameDictionary<Airline> Airlines { get; private set; }
        public CodeNameDictionary<Airport> Airports { get; private set; }
        public CodeNameDictionary<Status> Statuses { get; private set; }

        public FlightsService()
        {
            Airlines = new CodeNameDictionary<Airline>();
            Airports = new CodeNameDictionary<Airport>();
            Statuses = new CodeNameDictionary<Status>();

            _airlineService = new AirlineNamesService();
            _airportService = new AirportNamesService();
            _statusService = new StatusService();

            _resourceUrl = "XmlFeed.asp?TimeFrom={0}&TimeTo={1}&airport={2}";
        }

        public void GetFlightsFrom(ResultCallback<IEnumerable<Flight>> callback, Airport fromAirport)
        {
            if (Airports.Count == 0)
            {
                _airportService.GetAirports(r =>
                {
                    if (r.HasError()) callback(new Result<IEnumerable<Flight>>(r.Error));
                    Airports.AddRange(r.Value);
                    GetFlightsIfAllDone(callback, fromAirport);
                });
            }

            if (Airlines.Count == 0)
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

        private void GetFlightsIfAllDone(ResultCallback<IEnumerable<Flight>> callback, Airport fromAirport)
        {
            if (Airports.Count > 0 && Airlines.Count > 0 && Statuses.Count > 0)
            {
                var resource = string.Format(_resourceUrl, 1, 12, fromAirport.Code);
                Get(resource, callback);
            }
        }

        public override IEnumerable<Flight> ParseXml(XmlReader reader)
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
                       Gate = flight.ElementValueOrEmpty("gate"),
                       Belt = flight.ElementValueOrEmpty("belt"),
                       Direction = ConvertToDirection(flight.Element("arr_dep")),
                       ScheduledTime = flight.ElementAsDateTime("schedule_time")
                   };
        }

        private static Direction ConvertToDirection(XElement element)
        {
            if (element == null) return Direction.Depature;
            return element.Value == "D" ? Direction.Depature : Direction.Arrival;
        }

        private FlightStatus ReadStatus(XElement element)
        {
            if (element == null) return new FlightStatus();

            var flightStatus = new FlightStatus();
            flightStatus.Status = Statuses[element.AttributeValueOrEmpty("code")];
            flightStatus.StatusTime = element.AttributeAsDateTime("time");
            return flightStatus;
        }
    }
}
