using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public interface IXmlFileSystem : IFileSystem
{
	bool SaveXmlFile(List<string> data);
	List<string> LoadXmlFile();
}
