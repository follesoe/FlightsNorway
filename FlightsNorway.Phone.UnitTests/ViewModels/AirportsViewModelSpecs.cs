using System;
using System.Collections.Generic;
using FlightsNorway.Phone.Model;
using FlightsNorway.Phone.ViewModels;
using FlightsNorway.Phone.UnitTests.Stubs;

using NUnit.Framework;

namespace FlightsNorway.Phone.UnitTests.ViewModels
{
    [TestFixture]
    public class AirportsViewModelSpecs
    {
        [SetUp]
        public void Setup()
        {
            airportsService = new AirportNamesServiceStub();
            
        }

        private AirportNamesServiceStub airportsService;
        private AirportsViewModel viewModel;
    }
}
