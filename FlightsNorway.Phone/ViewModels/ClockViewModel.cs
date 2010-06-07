using System;
using System.Linq;
using GalaSoft.MvvmLight;

namespace FlightsNorway.Phone.ViewModels
{
    public class ClockViewModel : ViewModelBase
    {
        public string CurrentTime
        {
            get { return DateTime.Now.ToString("HH:mm"); }
        }

        public ClockViewModel()
        {
            Observable.Interval(new TimeSpan(0, 0, 1)).Subscribe(t => RaisePropertyChanged("CurrentTime"));
        }
    }
}
