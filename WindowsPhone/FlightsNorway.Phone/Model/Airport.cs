namespace FlightsNorway.Model
{
    public class Airport
    {
        public static readonly Airport Nearest = new Airport("NEAREST", "Nærmeste flyplass", -1, -1);

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

        public double DistanceFrom(Location otherLocation)
        {
            return otherLocation.DistanceTo(Location);
        }
    }
}
