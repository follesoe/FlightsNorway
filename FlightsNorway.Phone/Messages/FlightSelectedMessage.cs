using FlightsNorway.Phone.Model;
using GalaSoft.MvvmLight.Messaging;

namespace FlightsNorway.Phone.Messages
{
    public class FlightSelectedMessage : GenericMessage<Flight>
    {
        public FlightSelectedMessage(Flight content) : base(content)
        {
        }

        public FlightSelectedMessage(object sender, Flight content) : base(sender, content)
        {
        }

        public FlightSelectedMessage(object sender, object target, Flight content) : base(sender, target, content)
        {
        }
    }
}
