using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Xml;

namespace FlightsNorway.Phone.FlightDataServices
{
    public static class WebRequestFactory
    {
        public static IObservable<T> GetData<T>(Uri uri, Func<XmlReader, IEnumerable<T>> generator)
        {
            return (from request in Observable.Return(WebRequestFactory.CreateWebRequest(uri))
                    from response in Observable.FromAsyncPattern<WebResponse>(request.BeginGetResponse, request.EndGetResponse)()
                    from item in generator(XmlReader.Create(response.GetResponseStream())).ToObservable()
                    select item).ObserveOnDispatcher();
        }

        private static WebRequest CreateWebRequest(Uri uri)
        {
            var result = (HttpWebRequest)WebRequest.Create(uri);
            result.AllowReadStreamBuffering = false;
            return result;
        }
    }
}
