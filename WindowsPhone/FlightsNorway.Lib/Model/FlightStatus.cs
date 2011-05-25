using System;

namespace FlightsNorway.Lib.Model
{
    public class FlightStatus
    {
        public Status Status { get; set; }
        public DateTime StatusTime { get; set; }

        public FlightStatus()
        {
            Status = new Status();
        }
    }
}
