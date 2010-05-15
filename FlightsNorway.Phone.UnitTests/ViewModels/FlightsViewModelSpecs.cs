using System;
using System.Linq;
using GalaSoft.MvvmLight;
using FlightsNorway.Phone.ViewModels;

using NUnit.Framework;

namespace FlightsNorway.Phone.UnitTests.ViewModels
{
    [TestFixture]
    public class FlightsViewModelSpecs
    {
        [Test]
        public void Loads_flights_for_airport_when_airport_is_selected()
        {
            
        }

        [SetUp]
        public void Setup()
        {
            viewModel = new FlightsViewModel();             
        }

        private FlightsViewModel viewModel;

    }
}
