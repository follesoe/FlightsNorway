using System;
using System.Xml;
using System.Linq;
using System.Xml.Linq;
using System.Collections.Generic;
using FlightsNorway.Lib.Model;

namespace FlightsNorway.Lib.DataServices
{
    public class StatusService : RestService<Status>
    {
        public void GetStautses(Action<Result<IEnumerable<Status>>> callback)
        {
            Get("flightStatuses.asp", callback);
        }

        public override IEnumerable<Status> ParseXml(XmlReader reader)
        {
            var xml = XDocument.Load(reader);

            return xml.Elements("flightStatuses")
                .SelectMany(flightStatuses => flightStatuses.Elements("flightStatus"),
                           (flightStatuses, status) => new Status
                           {
                               Code = status.Attribute("code").Value,
                               StatusTextEnglish = status.Attribute("statusTextEn").Value,
                               StatusTextNorwegian = status.Attribute("statusTextNo").Value
                           });
        }
    }
}
