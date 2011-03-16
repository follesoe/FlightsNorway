using System.Collections.ObjectModel;
using Android.App;
using Android.Views;
using Android.Widget;

namespace FlightsNorway.Adapters
{
    public class ObservableAdapter<T> : BaseAdapter
    {
        private readonly Activity _context;
        private readonly ObservableCollection<T> _items;

        public ObservableAdapter(Activity context, ObservableCollection<T> items)
        {
            _context = context;
            _items = items;

            _items.CollectionChanged += (o,e) => NotifyDataSetChanged();
        }

        public override int Count
        {
            get {return _items.Count; }
        }

        public override Java.Lang.Object GetItem(int position)
        {
            return position;
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var item = _items[position];

            var view = (convertView ??
                        _context.LayoutInflater.Inflate(
                            Resource.Layout.list_item,
                            parent,
                            false)) as LinearLayout;

            var text = view.FindViewById(Resource.Id.text) as TextView;
            text.SetText(item.ToString(), TextView.BufferType.Normal);

            return view;
        }
    }
}