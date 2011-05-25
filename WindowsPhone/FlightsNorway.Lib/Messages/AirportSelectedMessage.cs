using FlightsNorway.Lib.Model;
using TinyMessenger;

namespace FlightsNorway.Lib.Messages
{
    public class AirportSelectedMessage : GenericTinyMessage<Airport>
    {
        public AirportSelectedMessage(object sender, Airport content)
            : base(sender, content)
        {
        }
    }

}
