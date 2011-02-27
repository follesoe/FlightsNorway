using System;
using System.Collections.Generic;

namespace FlightsNorway.Lib.Extensions
{
    public static class IEnumerableExtesnions
    {
        public static void ForEach<T>(this IEnumerable<T> enumerable, Action<T> action)
        {
            foreach (var item in enumerable)
                action(item);
        }
    }
}
