using System.Collections.Generic;
using System.Collections.ObjectModel;
using FlightsNorway.Lib.Extensions;

namespace FlightsNorway.Extensions
{
    public static class ObservableCollectionExtensions
    {
        public static void AddRange<T>(this ObservableCollection<T> collection, IEnumerable<T> items)
        {
            items.ForEach(collection.Add);
        }
    }
}
