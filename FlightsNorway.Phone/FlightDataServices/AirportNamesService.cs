using System;
using System.Net;
using System.Xml;
using System.Xml.Linq;
using System.Linq;
using System.Collections.Generic;
using FlightsNorway.Phone.Model;

namespace FlightsNorway.Phone.FlightDataServices
{
    public class AirportNamesService
    {
        private readonly Uri _serviceUrl;

        public AirportNamesService()
        {
            _serviceUrl = new Uri("http://flydata.avinor.no/airportNames.asp");    
        }

        public IObservable<IEnumerable<Airport>> GetAirports()
        {
            return WebRequestFactory.GetData(_serviceUrl, ParseAirportXml);
        }

        private static IEnumerable<Airport> ParseAirportXml(XmlReader reader)
        {
            var xml = XDocument.Load(reader);

            return from airportNames in xml.Elements("airportNames")
                   from airport in airportNames.Elements("airportName")
                   select new Airport
                              {
                                  Code = airport.Attribute("code").Value,
                                  Name = airport.Attribute("name").Value
                              };
        }
    }
}