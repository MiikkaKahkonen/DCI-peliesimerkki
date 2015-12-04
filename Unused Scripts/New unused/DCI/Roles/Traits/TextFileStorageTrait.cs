using UnityEngine;
using System.Collections;

namespace DCI.Roles
{
	public static class TextFileStorageExtension
	{
		public static string SetPath(this TextFileStorage self, Options options)
		{
			return self.System.SetRelativePath(options.filename);
		}

		public static string StorageAsText(this TextFileStorage self)
		{
			return self.System.LoadTextFile();
		}
		
		public static bool TextToStorage(this TextFileStorage self,string text)
		{
			return self.System.SaveTextFile(text); 
		}
	}
}
