using System.Collections.Generic;

namespace FlightsNorway.Phone.Model
{
    public class AirportDictionary : Dictionary<string, Airport>
    {
        public void AddRange(IEnumerable<Airport> airports)
        {
            airports.ForEach(Add);
        }

        public void Add(Airport airport)
        {
            if (ContainsKey(airport.Code)) return;
            Add(airport.Code, airport);
        }
    }
}
