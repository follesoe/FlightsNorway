using System;
using System.Collections.Generic;
using Microsoft.Phone.Shell;

namespace FlightsNorway.Services
{
    public interface IPhoneApplicationService
    {
        IDictionary<string, object> State { get; }
        event EventHandler<LaunchingEventArgs> Launching;
        event EventHandler<ClosingEventArgs> Closing;
        event EventHandler<ActivatedEventArgs> Activated;
        event EventHandler<DeactivatedEventArgs> Deactivated;     
    }

    public class PhoneApplicationServiceAdapter : IPhoneApplicationService
    {
        public event EventHandler<LaunchingEventArgs> Launching;
        public event EventHandler<ClosingEventArgs> Closing;
        public event EventHandler<ActivatedEventArgs> Activated;
        public event EventHandler<DeactivatedEventArgs> Deactivated;

        public IDictionary<string, object> State
        {
            get { return PhoneApplicationService.Current.State; }
        }

        public T GetState<T>(string key)
        {
            return (T) State[key];
        }

        public void AddOrReplace(string key, object item)
        {
            if (State.ContainsKey(key))
                State.Remove(key);

            State.Add(key, item);
        }

        public PhoneApplicationServiceAdapter()
        {
            if(PhoneApplicationService.Current == null) return;

            PhoneApplicationService.Current.Launching += (o, e) => { if (Launching != null) Launching(o, e); };
            PhoneApplicationService.Current.Closing += (o, e) => { if (Closing != null) Closing(o, e); };
            PhoneApplicationService.Current.Activated += (o, e) => { if (Activated != null) Activated(o, e); };
            PhoneApplicationService.Current.Deactivated += (o, e) => { if (Deactivated != null) Deactivated(o, e); };
        }
    }
}
