using System;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;
using FlightsNorway.Lib.DataServices;
using FlightsNorway.Lib.Services;
using FlightsNorway.Lib.ViewModels;
using MonoMobile.Extensions;
using TinyMessenger;

namespace FlightsNorway
{
    [Activity(Label = "FlightsNorway", MainLauncher = true, Theme = "@android:style/Theme.NoTitleBar")]
    public class MainActivity : TabActivity
    {
        private string currentTabId;

        public MainActivity()
        {
            InitContainer();
        }

        private void InitContainer()
        {
            Android.Util.Log.Info("MainActivity.cs", "Initializing");
            var container = TinyIoC.TinyIoCContainer.Current;
            container.Register<IDispatchOnUIThread>(new Dispatcher(this));
            container.Register<AirportsViewModel>().AsSingleton();
            container.Register<FlightsViewModel>().AsSingleton();
            container.Register<IStoreObjects, NoStorage>().AsSingleton();
            container.Register<IGetFlights, FlightsService>().AsSingleton();
            container.Register<IGetAirports, AirportNamesService>().AsSingleton();
            container.Register<IGeolocation>(new PresetLocationService(63.433, 10.419, container.Resolve<IDispatchOnUIThread>()));
            container.Register(new NearestAirportService(container.Resolve<IGeolocation>(), container.Resolve<ITinyMessengerHub>()));
        }

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
                                  typeof (DeparturesActivity),
                                  Resource.Drawable.ic_tab_departures));

            TabHost.TabChanged += TabHost_TabChanged;
        }

        void TabHost_TabChanged(object sender, TabHost.TabChangeEventArgs e)
        {
            if (currentTabId == null || currentTabId == "airports")
                TinyIoC.TinyIoCContainer.Current.Resolve<AirportsViewModel>().SaveCommand.Execute(null);

            currentTabId = e.TabId;
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