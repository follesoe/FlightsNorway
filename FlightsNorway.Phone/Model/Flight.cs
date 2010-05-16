using System;

namespace FlightsNorway.Phone.Model
{
    public class Flight
    {
        public string UniqueId { get; set; }
        public Airline Airline { get; set; }
        public Airport Airport { get; set; }
        public FlightStatus FlightStatus { get; set; }
        public DateTime StatusTime { get; set; }
        public DateTime ScheduledTime { get; set; }
        public string FlightId { get; set; }
        public FlightArea LastLeg { get; set; }
        public Direction Direction { get; set; }
        public string Gate { get; set; }
        public string Belt { get; set; }

        public Flight() : this(new Airline(), new Airport())
        {
        }

        public Flight(Airline airline, Airport airport)
        {
            Airline = airline;
            Airport = airport;
            FlightStatus = new FlightStatus();            
        }

        public override string ToString()
        {
            if(Direction == Direction.Depature)
            {
                return string.Format("{0} {1:HH:mm} {2} {3} {4}", FlightId, ScheduledTime, Gate, Airport.Name, FlightStatus.Status.StatusTextNorwegian);
            }

            return string.Format("{0} {1:HH:mm} {2} {3} {4} {5:HH:mm}",                     
                                 FlightId, ScheduledTime, Belt, Airport.Name,
                                 FlightStatus.Status.StatusTextNorwegian, FlightStatus.StatusTime);
        }
    }
}
