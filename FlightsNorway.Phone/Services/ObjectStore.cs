using System.IO;
using System.IO.IsolatedStorage;
using System.Runtime.Serialization;

namespace FlightsNorway.Phone.Services
{
    public class ObjectStore
    {
        private const string RootDirectory = "FlightsNorway";

        public void Save<T>(T item, string fileName)
        {
            var isoStore = IsolatedStorageFile.GetUserStoreForApplication();
            
            if(!isoStore.DirectoryExists(RootDirectory))
            {
                isoStore.CreateDirectory(RootDirectory);
            }

            var serializer = new DataContractSerializer(typeof(T));
            using (var stream = new IsolatedStorageFileStream(RootDirectory + "\\" + fileName, FileMode.OpenOrCreate, isoStore))
            {
                serializer.WriteObject(stream, item);
                stream.Close();
            }               
        }

        public T Load<T>(string fileName)
        {
            var isoStore = IsolatedStorageFile.GetUserStoreForApplication();
            var serializer = new DataContractSerializer(typeof(T));
            using (var stream = new IsolatedStorageFileStream(RootDirectory + "\\" + fileName, FileMode.Open, isoStore))
            {
                return (T)serializer.ReadObject(stream);                
            }               
        }
    }
}