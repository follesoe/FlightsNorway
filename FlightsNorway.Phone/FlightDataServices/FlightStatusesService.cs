using System;
using System.Xml;
using System.Xml.Linq;
using System.Collections.Generic;
using System.Linq;

namespace FlightsNorway.Phone.FlightDataServices
{
    public class FlightStatusesService
    {
        private readonly Uri _serviceUri;

        public FlightStatusesService()
        {
            _serviceUri = new Uri("http://flydata.avinor.no/flightStatuses.asp");
        }

        public IObservable<FlightStatus> GetStautses()
        {
            return WebRequestFactory.GetData(_serviceUri, ParseStatusXml);
        }

        private static IEnumerable<FlightStatus> ParseStatusXml(XmlReader reader)
        {
            var xml = XDocument.Load(reader);

            return from airlineNames in xml.Elements("flightStatuses")
                   from status in airlineNames.Elements("flightStatus")
                   select new FlightStatus
                              {
                                  Code = status.Attribute("code").Value,
                                  StatusTextEnglish = status.Attribute("statusTextEn").Value,
                                  StatusTextNorwegian = status.Attribute("statusTextNo").Value
                              };
        }
    }
}
