using Android.App;
using Android.OS;
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
            ListView.ItemClick += (sender, args) => viewModel.SelectedAirport = viewModel.Airports[args.Position];
        }
    }
}

