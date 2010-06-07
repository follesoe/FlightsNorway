using System.Diagnostics;
using FlightsNorway.Phone.MonitoringWebService;

namespace FlightsNorway.Phone.FlightDataServices
{
    public class MonitoringService : IMonitorFlights
    {
        private readonly MonitoringWebServiceSoapClient _service;
        
        public MonitoringService()
        {
            _service = new MonitoringWebServiceSoapClient();
        }

        public void MonitorFlight(string callbackUrl, string uniqueId)
        {            
            Debug.WriteLine("Start monitoring " + uniqueId + " - Callback: " + callbackUrl);

            _service.MonitorFlightAsync(callbackUrl, uniqueId);
        }

        public void StopMonitoringFlight(string callbackUrl, string uniqueId)
        {
            Debug.WriteLine("Stop monitoring " + uniqueId + " - Callback: " + callbackUrl);   

            _service.StopMonitoringFlightAsync(callbackUrl, uniqueId);
        }
    }
}
