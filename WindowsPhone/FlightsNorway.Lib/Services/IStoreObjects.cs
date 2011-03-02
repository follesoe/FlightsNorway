namespace FlightsNorway.Lib.Services
{
    public interface IStoreObjects
    {
        void Save<T>(T item, string fileName);
        T Load<T>(string fileName);
        void Delete(string fileName);
        bool FileExists(string fileName);
    }
}