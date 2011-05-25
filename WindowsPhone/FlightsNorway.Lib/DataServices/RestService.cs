using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Xml;

namespace FlightsNorway.Lib.DataServices
{
    public abstract class RestService<T> where T : class
    {
        private string _baseUrl = "http://flydata.avinor.no/";

        public abstract IEnumerable<T> ParseXml(XmlReader reader);

        protected void Get(string resource, ResultCallback<IEnumerable<T>> callback)
        {
            var webRequest = (HttpWebRequest)WebRequest.Create(_baseUrl + resource);

            webRequest.BeginGetResponse(responseResult =>
            {
                try
                {
                    var response = webRequest.EndGetResponse(responseResult);
                    if (response != null)
                    {
                        var result = ParseResult(response);
                        response.Close();
                        callback(new Result<IEnumerable<T>>(result));
                    }
                }
                catch (Exception ex)
                {
                    callback(new Result<IEnumerable<T>>(ex));
                }

            }, webRequest);
        }

        private IEnumerable<T> ParseResult(WebResponse response)
        {
            var encoding = Encoding.GetEncoding("iso-8859-1");
            using (var sr = new StreamReader(response.GetResponseStream(), encoding))
            using (var xmlReader = XmlReader.Create(sr))
            {
                return ParseXml(xmlReader);
            }
        }
    }

}
