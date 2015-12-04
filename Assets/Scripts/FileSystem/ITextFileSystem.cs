using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public interface ITextFileSystem : IFileSystem
{
	string LoadTextFile();
	bool SaveTextFile(string data);
}
