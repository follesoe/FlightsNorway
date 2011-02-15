using System;
using System.IO;
using System.Xml;
using System.Collections.Generic;
using RestSharp;

namespace FlightsNorway.FlightDataServices
{
    public class RestHelper
    {
        private readonly RestClient _client;

        public RestHelper()
        {
            _client = new RestClient("http://flydata.avinor.no");
        }

        public void Get<T>(string resource, Action<Result<IEnumerable<T>>> callback, Func<XmlReader, IEnumerable<T>> generator)
        {
            var request = new RestRequest(resource, Method.GET);
            _client.ExecuteAsync(request, response =>
            {
                if (response.ErrorException != null)
                {
                    callback(new Result<IEnumerable<T>>(response.ErrorException));
                }
                else
                {
                    callback(new Result<IEnumerable<T>>(ParseResult(response, generator)));
                }
            });
        }

        private static IEnumerable<T> ParseResult<T>(RestResponse response, Func<XmlReader, IEnumerable<T>> generator)
        {
            using (var sr = new StringReader(response.Content))
            using (var xmlReader = XmlReader.Create(sr))
            {
                return generator(xmlReader);
            }
        }
    }
}
