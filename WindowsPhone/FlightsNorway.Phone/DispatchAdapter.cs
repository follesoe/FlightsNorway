using System;
using System.Windows;
using FlightsNorway.Lib.Services;

namespace FlightsNorway
{
    public class DispatchAdapter : IDispatchOnUIThread
    {
        public void Invoke(Action action)
        {
            Deployment.Current.Dispatcher.BeginInvoke(action);
        }
    }
}