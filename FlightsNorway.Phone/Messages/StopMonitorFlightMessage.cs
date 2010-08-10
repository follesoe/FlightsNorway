using FlightsNorway.Model;
using GalaSoft.MvvmLight.Messaging;

namespace FlightsNorway.Messages
{
    public class StopMonitorFlightMessage : GenericMessage<Flight>
    {
        public StopMonitorFlightMessage(Flight content) : base(content)
        {
        }

        public StopMonitorFlightMessage(object sender, Flight content) : base(sender, content)
        {
        }

        public StopMonitorFlightMessage(object sender, object target, Flight content) : base(sender, target, content)
        {
        }
    }
}
