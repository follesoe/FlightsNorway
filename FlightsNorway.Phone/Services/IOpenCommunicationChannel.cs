using System;

namespace FlightsNorway.Phone.Services
{
    public interface IOpenCommunicationChannel
    {
        void OpenChannel(Action<string> channelCallback);
    }
}
