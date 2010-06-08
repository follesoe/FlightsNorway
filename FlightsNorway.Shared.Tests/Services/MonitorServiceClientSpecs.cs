using FlightsNorway.Shared.Model;
using FlightsNorway.Shared.Messages;
using FlightsNorway.Shared.Services;
using FlightsNorway.Shared.Tests.Stubs;
using GalaSoft.MvvmLight.Messaging;

using Microsoft.Silverlight.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FlightsNorway.Shared.Tests.Services
{
    [TestClass]
    public class MonitorServiceClientSpecs : SilverlightTest
    {
        [TestMethod, Tag(Tags.Services)]
        public void Should_open_push_channel()
        {
            serviceClient = new MonitorServiceClient(notificationService, monitorService);
            Assert.IsTrue(notificationService.OpenChannelWasCalled);
        }

        [TestMethod, Tag(Tags.Services)]
        public void Should_call_cloud_service_when_asked_to_monitor_flight()
        {
            var flight = new Flight();
            flight.UniqueId = "12345";
            serviceClient = new MonitorServiceClient(notificationService, monitorService);

            Messenger.Default.Send(new MonitorFlightMessage(flight));

            Assert.IsTrue(monitorService.MonitorFlightWasCalled);
            Assert.AreEqual(notificationService.ChannelUrl, monitorService.CallbackUrl);
            Assert.AreEqual(flight.UniqueId, monitorService.UniqueFlightId);
        }

        [TestMethod, Tag(Tags.Services)]
        public void Should_call_cloud_service_when_asked_to_stop_monitor_flight()
        {
            serviceClient = new MonitorServiceClient(notificationService, monitorService);

            var flight = new Flight();
            flight.UniqueId = "12345";
            Messenger.Default.Send(new StopMonitorFlightMessage(flight));

            Assert.IsTrue(monitorService.StopMonitoringFlightWasCalled);
            Assert.AreEqual(notificationService.ChannelUrl, monitorService.CallbackUrl);
            Assert.AreEqual(flight.UniqueId, monitorService.UniqueFlightId);
        }

        [TestInitialize]
        public void Setup()
        {
            monitorService = new MonitorServiceStub();
            notificationService = new NotificationServiceStub();            
        }

        private MonitorServiceClient serviceClient;
        private MonitorServiceStub monitorService;
        private NotificationServiceStub notificationService;
    }
}
