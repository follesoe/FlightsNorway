namespace FlightsNorway.Phone.Model
{
    public class Airport
    {
        public string Code { get; set; }
        public string Name { get; set; }

        public Airport()
        {                       
        }

        public Airport(string code, string name)
        {
            Code = code;
            Name = name;
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
    }
}
