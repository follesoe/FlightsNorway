using System.Collections.Generic;
using FlightsNorway.Lib.Extensions;

namespace FlightsNorway.Lib.Model
{
    public class AirportDictionary : Dictionary<string, Airport>
    {
        public new Airport this[string code]
        {
            get
            {
                return ContainsKey(code) ? base[code] : new Airport(code, "Unknown");
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
