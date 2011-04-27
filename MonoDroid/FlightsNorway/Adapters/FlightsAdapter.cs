using System.Collections.ObjectModel;
using Android.App;
using Android.Views;
using Android.Widget;
using FlightsNorway.Lib.Model;

namespace FlightsNorway.Adapters
{
    public class FlightsAdapter : ObservableAdapter<Flight>
    {
        public FlightsAdapter(Activity context, ObservableCollection<Flight> items) : base(context, items) {}

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var item = _items[position];
            var view = (convertView ?? _context.LayoutInflater.Inflate(Resource.Layout.list_flight, parent, false)) as LinearLayout;

            SetTextView(view, Resource.Id.rute, item.FlightId);
            SetTextView(view, Resource.Id.tid, item.ScheduledTime.ToString("HH:mm"));
            SetTextView(view, Resource.Id.gate, item.Gate.Length == 0 ? item.Belt : item.Gate);
            SetTextView(view, Resource.Id.fra, item.Airport.Name);
            SetTextView(view, Resource.Id.merknad, item.FlightStatus.Status.StatusTextEnglish);
            return view;
        }
    }
}