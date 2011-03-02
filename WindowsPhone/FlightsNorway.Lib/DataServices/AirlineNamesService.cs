using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using FlightsNorway.Lib.Model;

namespace FlightsNorway.Lib.DataServices
{
    public class AirlineNamesService
    {
        private readonly RestHelper _rest;
        
        public AirlineNamesService()
        {
            _rest = new RestHelper();
        }

        public void GetAirlines(Action<Result<IEnumerable<Airline>>> callback)
        {
            _rest.Get("airlineNames.asp", callback, ParseAirlineXml);
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