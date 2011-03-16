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
using FlightsNorway.Lib.Services;

namespace FlightsNorway
{
    class Dispatcher : IDispatchOnUIThread
    {
        private readonly Activity _owner;

        public Dispatcher(Activity owner)
        {
            _owner = owner;
        }

        public void Invoke(Action action)
        {
            _owner.RunOnUiThread(action);
        }
    }
}