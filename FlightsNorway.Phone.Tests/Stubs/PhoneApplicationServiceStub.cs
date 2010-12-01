using System;
using System.Collections.Generic;
using FlightsNorway.Services;
using Microsoft.Phone.Shell;

namespace FlightsNorway.Tests.Stubs
{
    public class PhoneApplicationServiceStub : IPhoneApplicationService
    {
        public event EventHandler<LaunchingEventArgs> Launching;
        public event EventHandler<ClosingEventArgs> Closing;
        public event EventHandler<ActivatedEventArgs> Activated;
        public event EventHandler<DeactivatedEventArgs> Deactivated;

        public IDictionary<string, object> State { get; set; }

        public PhoneApplicationServiceStub()
        {
            State = new Dictionary<string, object>();
        }

        public void TriggerActivated()
        {
            if(Activated != null)
                Activated(this, new ActivatedEventArgs());
        }

        public void TriggerDeactivated()
        {
            if(Deactivated != null)
                Deactivated(this, new DeactivatedEventArgs());
        }
    }
}
