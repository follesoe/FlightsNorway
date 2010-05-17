using System.IO;
using System.IO.IsolatedStorage;
using System.Runtime.Serialization;

namespace FlightsNorway.Phone.Services
{
    public class ObjectStore
    {
        public void Save<T>(T item, string fileName)
        {
            var isoStore = IsolatedStorageFile.GetUserStoreForApplication();
            
            if(!isoStore.DirectoryExists("FlightsNorway"))
            {
                isoStore.CreateDirectory("FlightsNorway");
            }

            var serializer = new DataContractSerializer(typeof(T));
            using(var stream = new IsolatedStorageFileStream("FlightsNorway\\" + fileName, FileMode.OpenOrCreate, isoStore))
            {
                serializer.WriteObject(stream, item);
                stream.Close();
            }               
        }

        public T Load<T>(string fileName)
        {
            var isoStore = IsolatedStorageFile.GetUserStoreForApplication();
            var serializer = new DataContractSerializer(typeof(T));
            using (var stream = new IsolatedStorageFileStream("FlightsNorway\\" + fileName, FileMode.Open, isoStore))
            {
                return (T)serializer.ReadObject(stream);                
            }               
        }
    }
}
