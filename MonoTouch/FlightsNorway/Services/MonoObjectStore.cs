using System;
using System.IO;
using System.Xml.Serialization;
using FlightsNorway.Lib.Services;

namespace FlightsNorway
{
	public class MonoObjectStore : IStoreObjects
	{
		private string _folderPath;
		
		public MonoObjectStore()		
		{
			_folderPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "FlightsNorway");
			if(!Directory.Exists(_folderPath))
				Directory.CreateDirectory(_folderPath);			
		}
		
		public void Save<T>(T item, string fileName)
		{	
			
			var serializer = new XmlSerializer(typeof(T));
            using (var stream = new FileStream(Path.Combine(_folderPath, fileName), FileMode.Create))
            {
				serializer.Serialize(stream, item);
                stream.Close();
            }	
		}

		public T Load<T>(string fileName)
		{
			string path = Path.Combine(_folderPath, fileName);
			
			if(!File.Exists(path))
			    throw new FileNotFoundException("File not found: " + fileName);
					
            var serializer = new XmlSerializer(typeof(T));
			T item = default(T);
            using (var stream = new FileStream(path, FileMode.Open))
            {
                item = (T)serializer.Deserialize(stream);
            } 
			
			return item;
		}

		public void Delete(string fileName)
		{
			string path = Path.Combine(_folderPath, fileName);
			if(File.Exists(path))
				File.Delete(path);
		}

		public bool FileExists(string fileName)
		{
			return File.Exists(Path.Combine(_folderPath, fileName));
		}
	}
}