using System.IO;
using System.IO.IsolatedStorage;
using System.Runtime.Serialization;

namespace FlightsNorway.Phone.Services
{
    public class ObjectStore
    {
        private const string RootDirectory = "FlightsNorway";
        private readonly IsolatedStorageFile _isoStore;

        public ObjectStore()
        {
            _isoStore = IsolatedStorageFile.GetUserStoreForApplication();
        }

        public void Save<T>(T item, string fileName)
        {
            if(!_isoStore.DirectoryExists(RootDirectory))
            {
                _isoStore.CreateDirectory(RootDirectory);
            }

            var serializer = new DataContractSerializer(typeof(T));
            using (var stream = new IsolatedStorageFileStream(RootDirectory + "\\" + fileName, FileMode.OpenOrCreate, _isoStore))
            {
                serializer.WriteObject(stream, item);
                stream.Close();
            }               
        }

        public T Load<T>(string fileName)
        {
            if(!_isoStore.FileExists(RootDirectory + "\\" + fileName))
                throw new FileNotFoundException("File not found: " + fileName);

            var serializer = new DataContractSerializer(typeof(T));
            using (var stream = new IsolatedStorageFileStream(RootDirectory + "\\" + fileName, FileMode.Open, _isoStore))
            {
                return (T)serializer.ReadObject(stream);                
            }               
        }

        public void Delete(string fileName)
        {
            _isoStore.DeleteFile(RootDirectory + "\\" + fileName);
        }
    }
}