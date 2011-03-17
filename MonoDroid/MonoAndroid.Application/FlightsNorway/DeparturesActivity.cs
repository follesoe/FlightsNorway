using Android.App;
using Android.OS;
using FlightsNorway.Lib.ViewModels;

namespace FlightsNorway
{
    [Activity(Label = "FlightsNorway")]
    public class DeparturesActivity : ListActivity
    {
        private FlightsViewModel viewModel;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            viewModel = TinyIoC.TinyIoCContainer.Current.Resolve<FlightsViewModel>();
            ListAdapter = new Adapters.FlightsAdapter(this, viewModel.Departures);
        }
    }
}

