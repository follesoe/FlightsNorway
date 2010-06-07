using System.Net;
using System.Text;
using System.Threading;
using System.Web.Services;

namespace FlightsMonitoringService
{
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    public class MonitoringWebService : WebService
    {
        [WebMethod]
        public void MonitorFlight(string callbackUrl, string uniqueId)
        {
            Thread.Sleep(5000);
            string ToastPushXML = "X-WindowsPhone-Target: toast\r\n\r\n" +
                          "<?xml version='1.0' encoding='utf-8'?>" +
                          "<wp:Notification xmlns:wp='WPNotification'>" +
                              "<wp:Toast>" +
                                  "<wp:Text1>{0}</wp:Text1>" +
                                  "<wp:Text2>{1}</wp:Text2>" +
                              "</wp:Toast>" +
                          "</wp:Notification>";


            var sendNotificationRequest = (HttpWebRequest)WebRequest.Create(callbackUrl);

            sendNotificationRequest.Method = "POST";
            sendNotificationRequest.Headers = new WebHeaderCollection();
            sendNotificationRequest.ContentType = "text/xml";

            sendNotificationRequest.Headers.Add("X-NotificationClass", "2");
            string str = string.Format(ToastPushXML, "Go to gate!", "Flight 123 is boarding");
            byte[] strBytes = new UTF8Encoding().GetBytes(str);
            sendNotificationRequest.ContentLength = strBytes.Length;
            using (var requestStream = sendNotificationRequest.GetRequestStream())
            {
                requestStream.Write(strBytes, 0, strBytes.Length);
            }

            var response = (HttpWebResponse)sendNotificationRequest.GetResponse();
        }

        [WebMethod]
        public void StopMonitoringFlight(string callbackUrl, string uniqueId)
        {
        }
    }
}
