using System;

namespace FlightsNorway.Services
{
    public interface IOpenCommunicationChannel
    {
        void OpenChannel(Action<string> channelCallback);
    }
}
