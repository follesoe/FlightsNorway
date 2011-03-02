using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using FlightsNorway.Lib.Model;

namespace FlightsNorway.Lib.DataServices
{
    public class StatusService
    {
        private readonly RestHelper _rest;

        public StatusService()
        {
            _rest = new RestHelper();
        }

        public void GetStautses(Action<Result<IEnumerable<Status>>> callback)
        {
            _rest.Get("flightStatuses.asp", callback, ParseStatusXml);
        }

        private static IEnumerable<Status> ParseStatusXml(XmlReader reader)
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
