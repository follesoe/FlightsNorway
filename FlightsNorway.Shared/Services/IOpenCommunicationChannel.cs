using System;

namespace FlightsNorway.Shared.Services
{
    public interface IOpenCommunicationChannel
    {
        void OpenChannel(Action<string> channelCallback);
    }
}
