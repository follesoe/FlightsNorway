using System;
using System.Linq;
using FlightsNorway.Phone.ViewModels;

using NUnit.Framework;

namespace FlightsNorway.Phone.UnitTests.ViewModels
{
    [TestFixture]
    public class FlightsViewModelSpecs
    {


        [SetUp]
        public void Setup()
        {
            viewModel = new FlightsViewModel();             
        }

        private FlightsViewModel viewModel;
    }
}
