using System;
using FlightsNorway.Phone.Services;

namespace FlightsNorway.Phone.Tests.Stubs
{
    public class ObjectStoreMock : IStoreObjects
    {
        public bool FileExistsShouldReturn;
        public object LoadShouldReturn;

        public void Save<T>(T item, string fileName)
        {
            throw new NotImplementedException();
        }

        public T Load<T>(string fileName)
        {
            return (T) LoadShouldReturn;
        }

        public void Delete(string fileName)
        {
            throw new NotImplementedException();
        }

        public bool FileExists(string fileName)
        {
            return FileExistsShouldReturn;
        }
    }
}
