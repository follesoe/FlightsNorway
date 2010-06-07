using System;
using FlightsNorway.Shared.Model;
using FlightsNorway.Shared.Services;

namespace FlightsNorway.Shared.DesignTimeData
{
    public class DesignTimeObjectStore : IStoreObjects
    {
        public void Save<T>(T item, string fileName)
        {
            throw new NotImplementedException();
        }

        public T Load<T>(string fileName)
        {
            if(fileName == ObjectStore.SelectedAirportFilename)
            {
                return (T) (object) new Airport("LKL", "Lakselv");
            } 
            throw new NotImplementedException();
        }

        public void Delete(string fileName)
        {
            throw new NotImplementedException();
        }

        public bool FileExists(string fileName)
        {
            if (fileName == ObjectStore.SelectedAirportFilename)
                return true;

            throw new NotImplementedException();
        }
    }
}
