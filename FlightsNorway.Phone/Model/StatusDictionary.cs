﻿using System.Collections.Generic;

namespace FlightsNorway.Phone.Model
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
