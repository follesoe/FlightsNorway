namespace FlightsNorway.Shared.FlightDataServices
{
    public interface IMonitorFlightsService
    {
        void MonitorFlightAsync(string callbackUrl, string uniqueId);
        void StopMonitoringFlightAsync(string callbackUrl, string uniqueId);
    }
}
