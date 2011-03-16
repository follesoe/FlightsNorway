using System;

namespace FlightsNorway.Lib.Model
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
		
		public string Line1()
		{
			if(Direction == Direction.Depature)
			{
                return string.Format("{0} {1:HH:mm} {2}", 
				                     	FlightId.PadRight(8), 
                    		 			ScheduledTime, 
                     					Gate);
			}
			return string.Format("{0} {1:HH:mm} {2}", 
			                     	FlightId.PadRight(8), 
                		 			ScheduledTime, 
                 					Belt);
		}
		
		public string Line2()
		{
			if(Direction == Direction.Depature)
			{
				return string.Format("{0} {1}", Airport.Name, FlightStatus.Status.StatusTextNorwegian);
			}
			
			return string.Format("{0} {1} {3:HH:mm}", Airport.Name, FlightStatus.Status.StatusTextNorwegian, FlightStatus.StatusTime);
		}

        public override string ToString()
        {
            if(Direction == Direction.Depature)
            {
                return string.Format("{0} {1:HH:mm} {2} {3} {4}", 
				                     FlightId.PadRight(8), 
				                     ScheduledTime, 
				                     Gate, 
				                     Airport.Name, 
				                     FlightStatus.Status.StatusTextNorwegian);
            }

            return string.Format("{0} {1:HH:mm} {2} {3} {4} {5:HH:mm}",                     
                                 FlightId.PadRight(8), 
			                     ScheduledTime, 
			                     Belt, 
			                     Airport.Name,
                                 FlightStatus.Status.StatusTextNorwegian, 
			                     FlightStatus.StatusTime);
        }
    }
}
