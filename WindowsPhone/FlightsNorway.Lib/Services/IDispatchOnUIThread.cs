using System;

namespace FlightsNorway.Lib.Services
{
    public interface IDispatchOnUIThread
    {
        void Invoke(Action action);
    }
}
