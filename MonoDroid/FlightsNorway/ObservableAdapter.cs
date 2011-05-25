using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace FlightsNorway
{
    public class ObservableAdapter<T> : BaseAdapter<T>
    {
        private readonly Activity _context;
        private readonly ObservableCollection<T> _collection;

        public ObservableAdapter(Activity context, ObservableCollection<T> collection)
        {
            _context = context;
            _collection = collection;
            _collection.CollectionChanged += (o, e) => NotifyDataSetChanged();
        }

        public override int Count
        {
            get { return _collection.Count; }
        }

        public override T this[int position]
        {
            get { return _collection[position]; }
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var item = _collection[position];
            var view = (convertView ??
                _context.LayoutInflater.Inflate(Resource.Layout.list_item, parent, false))
                    as LinearLayout;

            var textView = (TextView)view.FindViewById(Resource.Id.text);
            textView.SetText(item.ToString(), TextView.BufferType.Normal);

            return view;
        }
    }
}