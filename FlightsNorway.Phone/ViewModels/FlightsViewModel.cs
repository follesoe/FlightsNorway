using System;
using System.Linq;

using FlightsNorway.Phone.FlightDataServices;

using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;

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