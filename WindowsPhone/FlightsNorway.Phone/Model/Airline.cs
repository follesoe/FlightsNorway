namespace FlightsNorway.Model
{
    public class Airline
    {
        public string Code { get; set; }
        public string Name { get; set; }

        public Airline()
        {
            
        }

        public Airline(string code, string name)
        {
            Code = code;
            Name = name;
        }
    }
}
