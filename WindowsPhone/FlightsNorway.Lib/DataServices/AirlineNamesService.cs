using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using FlightsNorway.Lib.Model;

namespace FlightsNorway.Lib.DataServices
{
    public class AirlineNamesService : RestService<Airline>
    {
        public void GetAirlines(ResultCallback<IEnumerable<Airline>> callback)
        {
            Get("airlineNames.asp", callback);
        }

        public override IEnumerable<Airline> ParseXml(XmlReader reader)
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
