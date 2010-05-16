using System.Collections.Generic;

namespace FlightsNorway.Phone.Model
{
    public class AirlineDictionary : Dictionary<string, Airline>
    {        
        public new Airline this[string code]
        {
            get
            {
                if (ContainsKey(code)) return base[code];
                return new Airline {Code = code, Name = "Unknown"};
            }
            set { base[code] = value; }
        }

        public void AddRange(IEnumerable<Airline> airlines)
        {
            airlines.ForEach(Add);
        }

        public void Add(Airline airline)
        {
            if (ContainsKey(airline.Code)) return;

            Add(airline.Code, airline);
        }
    }
}
