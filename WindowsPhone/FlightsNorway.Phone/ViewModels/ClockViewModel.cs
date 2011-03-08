using System;
using System.Windows.Threading;
using FlightsNorway.Lib.MVVM;

namespace FlightsNorway.ViewModels
{
    public class ClockViewModel : ViewModelBase
    {
        public string CurrentTime
        {
            get { return DateTime.Now.ToString("HH:mm"); }
        }

        public ClockViewModel()
        {
            var timer = new DispatcherTimer();
            timer.Interval = new TimeSpan(0, 0, 0, 1, 0);
            timer.Tick += (o, e) => RaisePropertyChanged("CurrentTime");
            timer.Start();
        }
    }
}