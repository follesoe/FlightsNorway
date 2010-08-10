using System;
using System.Windows;
using GalaSoft.MvvmLight;
using Microsoft.Phone.Reactive;

namespace FlightsNorway.ViewModels
{
    public class ClockViewModel : ViewModelBase
    {
        public string CurrentTime
        {
            get { return DateTime.Now.ToUniversalTime().AddHours(1).ToString("HH:mm"); }
        }

        public ClockViewModel()
        {
            Observable.Interval(new TimeSpan(0, 0, 1))
                .Subscribe(t => Deployment.Current.Dispatcher.BeginInvoke(() => RaisePropertyChanged("CurrentTime")));
        }
    }
}