using System;
using System.Xml;
using System.Xml.Linq;
using System.Linq;
using System.Collections.Generic;

namespace FlightsNorway.Phone.FlightDataServices
{
    public class AirportNamesService
    {
        private readonly Uri _serviceUrl;

        public AirportNamesService()
        {
            _serviceUrl = new Uri("http://flydata.avinor.no/airportNames.asp");    
        }

        public IObservable<Airport> GetAirports()
        {
            return WebRequestFactory.GetData(_serviceUrl, ParseAirportXml);
        }

        private static IEnumerable<Airport> ParseAirportXml(XmlReader reader)
        {
            var xml = XDocument.Load(reader);

            return from airlineNames in xml.Elements("airportNames")
                   from airline in airlineNames.Elements("airportName")
                   select new Airport
                              {
                                  Code = airline.Attribute("code").Value,
                                  Name = airline.Attribute("name").Value
                              };
        }
    }
}