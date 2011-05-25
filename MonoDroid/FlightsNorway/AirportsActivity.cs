using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using FlightsNorway.Lib.Model;
using FlightsNorway.Lib.ViewModels;

namespace FlightsNorway
{
    [Activity(Label = "FlightsNorway")]
    public class AirportsActivity : ListActivity
    {
        private AirportsViewModel _viewModel;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            using (var stream = Assets.Open("Airports.xml"))
            {
                _viewModel = new AirportsViewModel(stream);
            }

            ListAdapter = new ObservableAdapter<Airport>(this, _viewModel.Airports);
            ListView.ItemClick += (o, e) =>
            _viewModel.SelectedAirport = _viewModel.Airports[e.Position];

        }
    }
}