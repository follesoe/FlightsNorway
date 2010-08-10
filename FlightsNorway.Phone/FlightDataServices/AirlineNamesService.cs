using System;
using System.Xml;
using System.Xml.Linq;
using System.Linq;
using System.Collections.Generic;
using FlightsNorway.Model;

namespace FlightsNorway.FlightDataServices
{
    public class AirlineNamesService
    {
        private readonly Uri _serviceUrl;

        public AirlineNamesService()
        {
            _serviceUrl = new Uri("http://flydata.avinor.no/airlineNames.asp");
        }        

        public IObservable<IEnumerable<Airline>> GetAirlines()
        {
            return WebRequestFactory.GetData(_serviceUrl, ParseAirlineXml);
        }

        private static IEnumerable<Airline> ParseAirlineXml(XmlReader reader)
        {
            var xml = XDocument.Load(reader);

            return from airlineNames in xml.Elements("airlineNames")
                   from airline in airlineNames.Elements("airlineName")
                   select new Airline
                              {
                                  Code = airline.Attribute("code").Value,
                                  Name = airline.Attribute("name").Value
                              };
        }
    }
}
