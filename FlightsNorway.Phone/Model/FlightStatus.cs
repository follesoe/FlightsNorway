using System;

namespace FlightsNorway.Phone.Model
{
    public class FlightStatus
    {
        public Status Status { get; set; }
        public DateTime StatusTime { get; set; }

        public static FlightStatus Empty { get; private set; }

        static FlightStatus()
        {
            Empty = new FlightStatus();
            Empty.Status = new Status();
            Empty.StatusTime = DateTime.Today;
            Empty.Status.Code = "NA";
            Empty.Status.StatusTextEnglish = "Missing status information";
            Empty.Status.StatusTextNorwegian = "Mangler status informasjon";
        }
    }
}
