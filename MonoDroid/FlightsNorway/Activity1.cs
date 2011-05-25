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
    public class Activity1 : TabActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.Main);

            TabHost.AddTab(GetTab("airports",
                                   "Airports",
                               typeof(AirportsActivity),
                                   Resource.Drawable.ic_tab_airports));

            TabHost.AddTab(GetTab("arrivals",
                                    "Arrivals",
                                    typeof(ArrivalsActivity),
                                    Resource.Drawable.ic_tab_arrivals));

            TabHost.AddTab(GetTab("departures",
                                    "Departures",
                                    typeof(DeparturesActivity),
                                    Resource.Drawable.ic_tab_departures));
        }

        private TabHost.TabSpec GetTab(string name, string title, Type type, int iconId)
        {
            var intent = new Intent(this, type);
            intent.AddFlags(ActivityFlags.NewTask);


            var spec = TabHost.NewTabSpec(name);
            spec.SetIndicator(title, Resources.GetDrawable(iconId));
            spec.SetContent(intent);

            return spec;
        }

    }

}

