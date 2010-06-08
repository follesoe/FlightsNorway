using System.Collections.Generic;
using FlightsNorway.Shared.Services;

namespace FlightsNorway.Shared.Tests.Stubs
{
    public class ObjectStoreStub : IStoreObjects
    {        
        private readonly Dictionary<string, object> _savedItems;

        public ObjectStoreStub()
        {
            _savedItems = new Dictionary<string, object>();
        }

        public void Save<T>(T item, string fileName)
        {
            if (_savedItems.ContainsKey(fileName))
                _savedItems[fileName] = item;
            else
                _savedItems.Add(fileName, item);
        }

        public T Load<T>(string fileName)
        {
            return (T)_savedItems[fileName];
        }

        public void Delete(string fileName)
        {
            _savedItems.Remove(fileName);
        }

        public bool FileExists(string fileName)
        {
            return _savedItems.ContainsKey(fileName);
        }
    }
}
