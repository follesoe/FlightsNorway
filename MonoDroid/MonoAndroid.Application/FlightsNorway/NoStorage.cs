using FlightsNorway.Lib.Services;

namespace FlightsNorway
{
    public class NoStorage : IStoreObjects
    {
        public void Save<T>(T item, string fileName)
        {
        }

        public T Load<T>(string fileName)
        {
            return default(T);
        }

        public void Delete(string fileName)
        {
        }

        public bool FileExists(string fileName)
        {
            return false;
        }
    }
}