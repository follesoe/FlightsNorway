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

namespace FlightsNorway
{
    [Activity(Label = "FlightsNorway")]
    public class ArrivalsActivity : ListActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            ListAdapter = new ObservableAdapter<Flight>
                (this, Activity1.FlightsViewModel.Departures);
        }
    }

}