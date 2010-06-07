using FlightsNorway.Shared.Model;
using GalaSoft.MvvmLight.Messaging;

namespace FlightsNorway.Shared.Messages
{
    public class AirportSelectedMessage : GenericMessage<Airport>
    {
        public AirportSelectedMessage(Airport content) : base(content)
        {
        }

        public AirportSelectedMessage(object sender, Airport content) : base(sender, content)
        {
        }

        public AirportSelectedMessage(object sender, object target, Airport content) : base(sender, target, content)
        {
        }
    }
}
