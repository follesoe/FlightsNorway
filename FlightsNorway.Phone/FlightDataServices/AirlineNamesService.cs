using System;
using System.Linq;
using System.Net;
using System.Xml;
using System.Xml.Linq;

using System.Collections.Generic;

namespace FlightsNorway.Phone.FlightDataServices
{
    public class AirlineNamesService
    {
        private readonly Uri _serviceUrl;

        public AirlineNamesService()
        {
            _serviceUrl = new Uri("http://flydata.avinor.no/airlineNames.asp");
        }

        public IObservable<Airline> GetAirlines()
        {
            return (from request in Observable.Return(CreateWebRequest(_serviceUrl))
                    from response in Observable.FromAsyncPattern<WebResponse>(request.BeginGetResponse, request.EndGetResponse)()
                    from item in GetAirlines(XmlReader.Create(response.GetResponseStream())).ToObservable()
                    select item).ObserveOnDispatcher();                            
        }

        private static IEnumerable<Airline> GetAirlines(XmlReader reader)
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

        private static WebRequest CreateWebRequest(Uri uri)
        {
            var result = (HttpWebRequest) WebRequest.Create(uri);
            result.AllowReadStreamBuffering = false;
            return result;
        }
    }
}
