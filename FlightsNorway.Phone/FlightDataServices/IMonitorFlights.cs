namespace FlightsNorway.Phone.FlightDataServices
{
    public interface IMonitorFlights
    {
        void MonitorFlight(string callbackUrl, string uniqueId);
        void StopMonitoringFlight(string callbackUrl, string uniqueId);
    }
}