using System;
using FlightsNorway.Lib.Services;

namespace FlightsNorway
{
	public class MonoObjectStore : IStoreObjects
	{
		public void Save<T> (T item, string fileName)
		{			
		}

		public T Load<T> (string fileName)
		{
			throw new NotImplementedException ();
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

