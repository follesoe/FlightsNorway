using System.Collections.Generic;

using FlightsNorway.Phone.Messages;
using FlightsNorway.Phone.FlightDataServices;

using GalaSoft.MvvmLight.Messaging;

namespace FlightsNorway.Phone.Services
{
    public class MonitorServiceClient
    {
        private readonly IOpenCommunicationChannel _notificationService;
        private readonly IMonitorFlights _monitorService;

        private string _callbackUrl;

        private readonly Queue<string> _flightsToMonitor;
        private readonly Queue<string> _flightsToStopMonitoring;

        public MonitorServiceClient(IOpenCommunicationChannel notificationService, IMonitorFlights monitorService)
        {
            _flightsToMonitor = new Queue<string>();
            _flightsToStopMonitoring = new Queue<string>();

            _monitorService = monitorService;
            _notificationService = notificationService;           
            _notificationService.OpenChannel(ChanelOpened);

            Messenger.Default.Register(this, (MonitorFlightMessage m) => StartMonitoringFlight(m));
            Messenger.Default.Register(this, (StopMonitorFlightMessage m) => StopMonitoringFlight(m));
        }

        private void ChanelOpened(string callbackUrl)
        {
            _callbackUrl = callbackUrl;
            ProcessQueues();
        }

        private void StartMonitoringFlight(MonitorFlightMessage message)
        {
            _flightsToMonitor.Enqueue(message.Content.UniqueId);
            ProcessQueues();
        }

        private void StopMonitoringFlight(StopMonitorFlightMessage message)
        {
            _flightsToStopMonitoring.Enqueue(message.Content.UniqueId);
            ProcessQueues();
        }

        private void ProcessQueues()
        {
            if (string.IsNullOrEmpty(_callbackUrl)) return;

            while(_flightsToMonitor.Count > 0)
            {
                _monitorService.MonitorFlight(_callbackUrl, _flightsToMonitor.Dequeue());
            }
            
            while(_flightsToStopMonitoring.Count > 0)
            {
                _monitorService.StopMonitoringFlight(_callbackUrl, _flightsToStopMonitoring.Dequeue());
            }
        }
    }
}
