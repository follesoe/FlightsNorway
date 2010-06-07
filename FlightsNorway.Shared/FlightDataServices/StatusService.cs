using System;
using System.Xml;
using System.Xml.Linq;
using System.Collections.Generic;
using System.Linq;
using FlightsNorway.Shared.Model;

namespace FlightsNorway.Shared.FlightDataServices
{
    public class StatusService
    {
        private readonly Uri _serviceUri;

        public StatusService()
        {
            _serviceUri = new Uri("http://flydata.avinor.no/flightStatuses.asp");
        }

        public IObservable<IEnumerable<Status>> GetStautses()
        {
            return WebRequestFactory.GetData(_serviceUri, ParseStatusXml);
        }

        private static IEnumerable<Status> ParseStatusXml(XmlReader reader)
        {
            var xml = XDocument.Load(reader);

            return from flightStatuses in xml.Elements("flightStatuses")
                   from status in flightStatuses.Elements("flightStatus")
                   select new Status
                              {
                                  Code = status.Attribute("code").Value,
                                  StatusTextEnglish = status.Attribute("statusTextEn").Value,
                                  StatusTextNorwegian = status.Attribute("statusTextNo").Value
                              };
        }
    }
}
