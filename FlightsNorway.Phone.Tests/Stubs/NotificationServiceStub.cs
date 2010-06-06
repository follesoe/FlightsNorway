using System;
using FlightsNorway.Phone.Services;

namespace FlightsNorway.Phone.Tests.Stubs
{
    public class NotificationServiceStub : IOpenCommunicationChannel
    {
        public void OpenChannel(Action<string> channelCallback)
        {
            channelCallback("http://push.notifications.com");
        }
    }
}
