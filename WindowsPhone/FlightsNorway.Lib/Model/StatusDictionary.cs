using System.Collections.Generic;
using FlightsNorway.Lib.Extensions;

namespace FlightsNorway.Lib.Model
{
    public class StatusDictionary : Dictionary<string, Status>
    {
        public void AddRange(IEnumerable<Status> statuses)
        {
            statuses.ForEach(Add);
        }

        public void Add(Status status)
        {
            if (ContainsKey(status.Code)) return;

            Add(status.Code, status);
        }
    }
}
