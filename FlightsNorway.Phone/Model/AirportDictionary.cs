using System.Collections.Generic;
using FlightsNorway.Phone.Extensions;

namespace FlightsNorway.Phone.Model
{
    public class AirportDictionary : Dictionary<string, Airport>
    {
        public new Airport this[string code]
        {
            get
            {
                if (ContainsKey(code)) return base[code];
                return new Airport { Code = code, Name = "Unknown" };
            }
            set { base[code] = value; }
        }

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
