using System.Diagnostics;

namespace FlightsNorway.Phone.FlightDataServices
{
    public class MonitoringService : IMonitorFlights
    {
        public void MonitorFlight(string callbackUrl, string uniqueId)
        {
            Debug.WriteLine("Start monitoring " + uniqueId + " - Callback: " + callbackUrl);
        }

        public void StopMonitoringFlight(string callbackUrl, string uniqueId)
        {
            Debug.WriteLine("Stop monitoring " + uniqueId + " - Callback: " + callbackUrl);   
        }
    }
}
