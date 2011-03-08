using System;
using System.Linq;
using System.IO.IsolatedStorage;

namespace FlightsNorway.Lib
{
	public static class IsolatedStorageFileExtensions
	{
		public static bool DirectoryExists(this IsolatedStorageFile isoStore, string path)
		{
			return isoStore.GetDirectoryNames("*.*").Contains(path);
		}
		
		public static bool FileExists(this IsolatedStorageFile isoStore, string path)
		{
			return isoStore.GetFileNames("*.*").Contains(path);
		}
	}
}

