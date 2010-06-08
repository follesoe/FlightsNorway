using System.Diagnostics;

namespace FlightsNorway.Shared.FlightDataServices
{
    public class MonitoringService : IScheduleFlightsToMonitor
    {
        private readonly IMonitorFlightsService _service;
        
        public MonitoringService(IMonitorFlightsService service)
        {
            _service = service;
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
