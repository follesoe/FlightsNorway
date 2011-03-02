using System;

namespace FlightsNorway.Lib.Services
{
    public class EventArgs<T> : EventArgs
    {
        public T Content { get; private set; }

        public EventArgs(T content)
        {
            Content = content;
        }
    }
}