using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using FlightsNorway.Lib.Model;

namespace FlightsNorway.Lib.DataServices
{
    public class StatusService : RestService<Status>
    {
        public void GetStautses(ResultCallback<IEnumerable<Status>> callback)
        {
            Get("flightStatuses.asp", callback);
        }

        public override IEnumerable<Status> ParseXml(XmlReader reader)
        {
            var xml = XDocument.Load(reader);

            return from statuses in xml.Elements("flightStatuses")
                   from status in statuses.Elements("flightStatus")
                   select new Status
                   {
                       Code = status.Attribute("code").Value,
                       Name = status.Attribute("statusTextNo").Value
                   };
        }
    }

}
