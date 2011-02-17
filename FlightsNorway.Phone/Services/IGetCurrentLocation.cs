using System;
using FlightsNorway.Model;

namespace FlightsNorway.Services
{
    public interface IGetCurrentLocation
    {
        event EventHandler<EventArgs<Location>> PositionAvailable;
        void GetPositionAsync();
    }
}
