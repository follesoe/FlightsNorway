using System;

namespace FlightsNorway.Lib
{
    public interface IDispatchOnUIThread
    {
        void Invoke(Action action);
    }
}
