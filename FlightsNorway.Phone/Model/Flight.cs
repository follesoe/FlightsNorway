using System;

namespace FlightsNorway.Phone.Model
{
    public class Flight
    {
        public string UniqueId { get; set; }
        public Airline Airline { get; set; }
        public Airport Airport { get; set; }
        public FlightStatus Status { get; set; }
        public DateTime StatusTime { get; set; }
        public DateTime ScheduledTime { get; set; }
        public string FlightId { get; set; }
        public FlightArea LastLeg { get; set; }
        public Direction Direction { get; set; }
        public string Gate { get; set; }        
    }
}
