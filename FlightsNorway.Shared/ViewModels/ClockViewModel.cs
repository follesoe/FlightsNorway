using System;
using System.Linq;
using System.Windows;
using GalaSoft.MvvmLight;

namespace FlightsNorway.Shared.ViewModels
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