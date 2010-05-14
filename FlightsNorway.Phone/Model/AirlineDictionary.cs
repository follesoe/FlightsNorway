using System.Collections.Generic;

namespace FlightsNorway.Phone.Model
{
    public class AirlineDictionary : Dictionary<string, Airline>
    {
        public void Add(Airline airline)
        {
            if (ContainsKey(airline.Code)) return;

            Add(airline.Code, airline);
        }
    }
}
