using System;
using System.Linq;
using System.Net;
using System.Xml;

namespace FlightsNorway.Phone.FlightDataServices
{
    public static class WebRequestFactory
    {       
        public static IObservable<T> GetData<T>(Uri uri, Func<XmlReader, T> generator)
        {
            return (from request in Observable.Return(CreateWebRequest(uri))
                    from response in Observable.FromAsyncPattern<WebResponse>(request.BeginGetResponse, request.EndGetResponse)()
                    select generator(XmlReader.Create(response.GetResponseStream()))).ObserveOnDispatcher();
        }

        private static WebRequest CreateWebRequest(Uri uri)
        {
            var result = (HttpWebRequest)WebRequest.Create(uri);
            result.AllowReadStreamBuffering = false;
            return result;
        }
    }
}
