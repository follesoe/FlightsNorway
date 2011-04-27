using System;
using Android.App;
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