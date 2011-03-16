using System;
using System.Collections.Generic;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using FlightsNorway.Lib.DataServices;
using FlightsNorway.Lib.Model;
using FlightsNorway.Lib.ViewModels;

namespace FlightsNorway
{
    [Activity(Label = "FlightsNorway")]
    public class AirportsActivity : ListActivity
    {
        private AirportsViewModel viewModel;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            viewModel = TinyIoC.TinyIoCContainer.Current.Resolve<AirportsViewModel>();

            ListAdapter = new Adapters.ObservableAdapter<Airport>(this, viewModel.Airports);

            ListView.TextFilterEnabled = true;

            ListView.ItemClick += delegate(object sender, ItemEventArgs args)
            {
                viewModel.SelectedAirport = viewModel.Airports[args.Position];
            };
        }
    }
}

