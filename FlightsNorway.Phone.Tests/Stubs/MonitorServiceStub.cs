using FlightsNorway.Phone.FlightDataServices;

namespace FlightsNorway.Phone.Tests.Stubs
{
    public class MonitorServiceStub : IMonitorFlights
    {
        public string CallbackUrl;
        public string UniqueFlightId;
        public bool StopMonitoringFlightWasCalled;
        public bool MonitorFlightWasCalled;

        public void MonitorFlight(string callbackUrl, string uniqueId)
        {
            MonitorFlightWasCalled = true;
            CallbackUrl = callbackUrl;
            UniqueFlightId = uniqueId;
        }

        public void StopMonitoringFlight(string callbackUrl, string uniqueId)
        {
            CallbackUrl = callbackUrl;
            UniqueFlightId = uniqueId;
            StopMonitoringFlightWasCalled = true;
        }
    }
}
