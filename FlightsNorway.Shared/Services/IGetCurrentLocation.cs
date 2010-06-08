using System;
using FlightsNorway.Shared.Model;

namespace FlightsNorway.Shared.Services
{
    public class EventArgs<T> : EventArgs
    {
        public T Content { get; private set; }

        public EventArgs(T content)
        {
            Content = content;
        }
    }

    public interface IGetCurrentLocation
    {
        event EventHandler<EventArgs<Location>> PositionAvailable;
        void GetPositionAsync();
    }
}
