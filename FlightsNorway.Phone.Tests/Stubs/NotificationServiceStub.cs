using System;
using FlightsNorway.Shared.Services;

namespace FlightsNorway.Phone.Tests.Stubs
{
    public class NotificationServiceStub : IOpenCommunicationChannel
    {
        public bool OpenChannelWasCalled;
        public string ChannelUrl = "http://push.notifications.com";

        public void OpenChannel(Action<string> channelCallback)
        {
            OpenChannelWasCalled = true;
            channelCallback(ChannelUrl);
        }
    }
}
