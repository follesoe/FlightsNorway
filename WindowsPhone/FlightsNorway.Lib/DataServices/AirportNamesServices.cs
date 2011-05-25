using System;
using System.IO;
using System.Linq;
using System.Net;
#if MONOTOUCH || MONODROID
using System.Web;
#endif
using System.Xml;
using System.Xml.Linq;
using System.Collections.Generic;

using FlightsNorway.Lib.Model;

namespace FlightsNorway.Lib.DataServices
{
    public class AirportNamesService : RestService<Airport>
    {
        public void GetAirports(ResultCallback<IEnumerable<Airport>> callback)
        {
            Get("airportNames.asp", callback);
        }

        public override IEnumerable<Airport> ParseXml(XmlReader reader)
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


        public List<Airport> GetNorwegianAirports(Stream stream)
        {
            var xml = XDocument.Load(stream);

            return (from a in xml.Root.Descendants("Airport")
                    select new Airport
                    {
                        Code = HttpUtility.UrlDecode(a.Attribute("Code").Value),
                        Name = HttpUtility.UrlDecode(a.Attribute("Name").Value),
                        Location = new Location
                        {
                            Latitude = Convert.ToDouble(a.Attribute("Lat").Value),
                            Longitude = Convert.ToDouble(a.Attribute("Lon").Value)
                        }
                    }).ToList();
        }
    }
}
