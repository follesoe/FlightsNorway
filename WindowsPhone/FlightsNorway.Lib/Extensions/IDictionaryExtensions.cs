using System.Collections.Generic;

namespace FlightsNorway.Lib.Extensions
{
    public static class IDictionaryExtensions
    {
        public static T Get<T>(this IDictionary<string, object> dictionary, string key)
        {
            return (T)dictionary[key];
        }

        public static void AddOrReplace<T>(this IDictionary<string, T> dictionary, string key, T item)
        {
            if (dictionary.ContainsKey(key))
                dictionary.Remove(key);

            dictionary.Add(key, item);
        }
    }
}
