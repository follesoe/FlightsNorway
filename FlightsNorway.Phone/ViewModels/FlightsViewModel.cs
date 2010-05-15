using System;

using FlightsNorway.Phone.FlightDataServices;

using GalaSoft.MvvmLight;

namespace FlightsNorway.Phone.ViewModels
{
    public class FlightsViewModel : ViewModelBase
    {
        private readonly IGetFlights _flightsService;

        public FlightsViewModel(): this(new FlightsService())
        {
            
        }

        public FlightsViewModel(IGetFlights flightsService)
        {
            _flightsService = flightsService;
        }
    }
}