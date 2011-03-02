using System;
using System.IO;
using System.Xml;
using System.Text;
using System.Collections.Generic;
using RestSharp;

namespace FlightsNorway.Lib.DataServices
{
    public abstract class RestService<T> where T : class
    {
        private readonly RestClient _client;

        protected RestService()
        {
            _client = new RestClient("http://flydata.avinor.no");
        }
 
        protected void Get(string resource, Action<Result<IEnumerable<T>>> callback)
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
                    callback(new Result<IEnumerable<T>>(ParseResult(response)));
                }
            });
        }

        public abstract IEnumerable<T> ParseXml(XmlReader reader);

        private IEnumerable<T> ParseResult(RestResponse response)
        {                        
            var encoding = Encoding.GetEncoding("iso-8859-1");            
            using (var ms = new MemoryStream(response.RawBytes))
            using (var sr = new StreamReader(ms, encoding))
            using (var xmlReader = XmlReader.Create(sr))
            {
                return ParseXml(xmlReader);
            }
        }
    }
}
