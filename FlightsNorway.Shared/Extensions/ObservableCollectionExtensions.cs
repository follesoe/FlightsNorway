using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace FlightsNorway.Shared.Extensions
{
    public static class ObservableCollectionExtensions
    {
        public static void AddRange<T>(this ObservableCollection<T> collection, IEnumerable<T> items)
        {
            items.ForEach(collection.Add);
        }
    }
}
