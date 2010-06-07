using System;

namespace FlightsNorway.Shared.Model
{
    public class FlightStatus
    {
        public Status Status { get; set; }
        public DateTime StatusTime { get; set; }

        public static FlightStatus Empty { get; private set; }

        public FlightStatus()
        {
            Status = new Status();
        }

        static FlightStatus()
        {
            Empty = new FlightStatus();
            Empty.Status = new Status();
            Empty.StatusTime = DateTime.Today;
            Empty.Status.Code = "NA";
            Empty.Status.StatusTextEnglish = "";
            Empty.Status.StatusTextNorwegian = "";
        }
    }
}
