using System.Collections.Generic;

namespace FlightsNorway.Lib.Model
{
    public class CodeNameDictionary<T> : Dictionary<string, T> where T : CodeName, new()
    {
        public new T this[string code]
        {
            get
            {
                if (ContainsKey(code)) return base[code];
                var item = new T();
                item.Code = code;
                item.Name = "Unknown";
                return item;
            }
            set { base[code] = value; }
        }

        public void AddRange(IEnumerable<T> items)
        {
            foreach (var item in items)
                Add(item);
        }

        public void Add(T item)
        {
            if (ContainsKey(item.Code)) return;

            Add(item.Code, item);
        }
    }
}
