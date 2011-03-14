using TinyMessenger;

namespace FlightsNorway.Lib.Messages
{
    public class FindNearestAirportMessage : TinyMessageBase
    {
        public FindNearestAirportMessage(object sender) : base(sender)
        {
        }
    }
}
