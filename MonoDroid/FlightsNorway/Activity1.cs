using System;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using FlightsNorway.Lib.DataServices;
using FlightsNorway.Lib.Model;

namespace FlightsNorway
{
    [Activity(Label = "FlightsNorway", MainLauncher = true, Icon = "@drawable/icon")]
    public class Activity1 : ListActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            var airportsService = new AirportNamesService();
            using (var stream = Assets.Open("Airports.xml"))
            {
                var airports = airportsService.GetNorwegianAirports(stream);
                ListAdapter = new ArrayAdapter<Airport>(this,
                                     Android.Resource.Layout.SimpleListItem1, airports);
            }
        }
    }

}

