using System;

namespace FlightsNorway.Phone.Model
{
    public class Airport
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public Location Location { get; set; }

        public Airport()
        {                       
        }

        public Airport(string code, string name)
        {
            Code = code;
            Name = name;
        }

        public Airport(string code, string name, double latitude, double longitude)
        {
            Code = code;
            Name = name;
            Location = new Location(latitude, longitude);
        }

        public override string ToString()
        {
            return string.Format("{0} - {1}", Code, Name);
        }

        public override bool Equals(object obj)
        {
            var airport = obj as Airport;
            if(airport != null)
            {
                return airport.Code.Equals(Code) &&
                       airport.Name.Equals(Name);
            }
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return Code.GetHashCode();
        }

        private static readonly Airport _nearest = new Airport("NEAREST", "Nærmeste flyplass", -1, -1);

        public static Airport Nearest
        {
            get { return _nearest; }    
        }

        public double DistanceFrom(Location otherLocation)
        {
            return otherLocation.DistanceTo(Location);
        }
    }
}
