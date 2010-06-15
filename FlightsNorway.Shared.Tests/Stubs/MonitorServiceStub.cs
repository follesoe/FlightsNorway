using FlightsNorway.Shared.FlightDataServices;

namespace FlightsNorway.Shared.Tests.Stubs
{
    public class MonitorServiceStub : IMonitorFlightsService
    {
        public string CallbackUrl;
        public string UniqueFlightId;
        public bool StopMonitoringFlightWasCalled;
        public bool MonitorFlightWasCalled;

        public void MonitorFlightAsync(string callbackUrl, string uniqueId)
        {
            MonitorFlightWasCalled = true;
            CallbackUrl = callbackUrl;
            UniqueFlightId = uniqueId;
        }

        public void StopMonitoringFlightAsync(string callbackUrl, string uniqueId)
        {
            CallbackUrl = callbackUrl;
            UniqueFlightId = uniqueId;
            StopMonitoringFlightWasCalled = true;
        }
    }
}
