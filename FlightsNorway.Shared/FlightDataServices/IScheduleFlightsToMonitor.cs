namespace FlightsNorway.Shared.FlightDataServices
{
    public interface IScheduleFlightsToMonitor
    {
        void MonitorFlight(string callbackUrl, string uniqueId);
        void StopMonitoringFlight(string callbackUrl, string uniqueId);
    }
}