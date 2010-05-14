using System.Collections.Generic;

namespace FlightsNorway.Phone.Model
{
    public class AirlineDictionary : Dictionary<string, Airline>
    {
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
