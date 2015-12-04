using UnityEngine;
using System.Collections;
using System.IO;
using System.Collections.Generic;
using System.Xml;

public class BaseFileSystem : ITextFileSystem{

	// data and value attribute separators
	public const char ATTSEP = ':';
	public const char VALSEP = '=';
	public const char AXISSEP = ',';

	private static BaseFileSystem fs;

	private string currentPath;
	private string location;
	private string defaultPath;
	
	public static BaseFileSystem GetInstance()
	{
		if(fs == null)
		{
			fs = new BaseFileSystem();

			fs.location = Application.persistentDataPath;
			fs.defaultPath = fs.location+"/default";
		}
		return fs;
	}
    public string[] GetFileNames()
    {
        string[] filePaths = Directory.GetFiles(this.location, "*.txt",SearchOption.TopDirectoryOnly);
        for (int i = 0; i < filePaths.Length; i++)
        {
            filePaths[i] = filePaths[i].Replace(this.location+ "\\", "");
            filePaths[i] = filePaths[i].Replace(".txt", "");
        }
        return filePaths;
    }
	// Make constructor private, singleton use only
	private BaseFileSystem(){}

	public string SetRelativePath(string path)
	{
		currentPath = location+"/"+path;
		return currentPath;
	}

	public List<string> LoadTextFile()
	{
		if(currentPath != null)
		{
			return LoadTextFile(currentPath);
		}
		else 
		{
			return LoadTextFile(defaultPath);
		}
	}
	private List<string> LoadTextFile(string fullPath)
	{
		StreamReader sr = new StreamReader(fullPath);
		List<string> texts = new List<string>();

		string line = null;

		while((line = sr.ReadLine()) != null)
		{
			texts.Add(line);
		}

		sr.Close();
		return texts;
	}
	public bool SaveTextFile(List<string> data)
	{
		if(currentPath != null)
		{
			return SaveTextFile(currentPath,data);
		}
		else
		{
			return SaveTextFile(defaultPath,data);
		}
	}
	private bool SaveTextFile(string fullPath,List<string> data)
	{
		try
		{
		StreamWriter sw = new StreamWriter(fullPath);

		foreach(string line in data)
		{
			sw.WriteLine(line);
		}

		sw.Close();

		}
		catch(IOException e)
		{
			Debug.Log(e.ToString());
			return false;
		}
		return true;
	}

	public List<string> LoadXmlFile()
	{
		if(currentPath != null)
		{
			return LoadXmlFile(currentPath);
		}
		else
		{
			return LoadXmlFile(defaultPath);
		}
	}
	private List<string> LoadXmlFile(string fullPath)
	{
		try
		{

		XmlDocument doc = new XmlDocument();
		List<string> xmlstring = LoadTextFile(fullPath);
		doc.LoadXml(xmlstring[0]);
		
		return XmlToStringList(doc);

		}
		catch(IOException e)
		{
			Debug.Log(e.ToString());
			return null;
		}
	}
	public bool SaveXmlFile(List<string> data)
	{
		if(currentPath != null)
		{
			return SaveXmlFile(currentPath,data);
		}
		else
		{
			return SaveXmlFile(defaultPath,data);
		}
	}
	private bool SaveXmlFile(string fullPath,List<string> data)
	{
		try
		{
		XmlDocument doc = new XmlDocument();
		XmlNode node = doc.CreateNode(XmlNodeType.Element,"Graph","");
		doc.AppendChild(node);

		XmlElement root = doc.DocumentElement;

		XmlAttribute att;

		string[] exploded;
		string[] explodedAgain;

		foreach(string line in data)
		{
			string lowline = line.ToLower();
			exploded = lowline.Split(':');

			if(exploded[0].Contains("node") || exploded[0].Contains("edge"))
		    {
				node = doc.CreateNode(XmlNodeType.Element,exploded[0],"");
				for(int i = 1; i < exploded.Length; i++)
				{
					explodedAgain = exploded[i].Split('=');
					att = doc.CreateAttribute(explodedAgain[0]);
					att.Value = explodedAgain[1];
					node.Attributes.Append(att);
				}
				root.AppendChild(node);
			}
		}

		string xmldata = doc.OuterXml;
		List<string> data2 = new List<string>(xmldata.Split('\n'));

		return SaveTextFile(fullPath,data2);

		}
		catch(IOException e)
		{
			Debug.Log(e.ToString());
			return false;
		}
	}

	private List<string> XmlToStringList(XmlDocument xml)
	{
		List<string> list = new List<string>();
		string line = "";

		XmlNode root = xml.DocumentElement;
		foreach(XmlNode child in root.ChildNodes)
		{
			line = child.Name;
			foreach(XmlAttribute att in child.Attributes)
			{
				line += ":";
				line += att.Name+"="+att.Value;
			}
			list.Add(line);
		}
		return list;
	}
	public bool fileExists()
	{
		return File.Exists(currentPath);
	}

	public string GetPath ()
	{
		return currentPath;
	}

	#region ITextFileSystem implementation

	string ITextFileSystem.LoadTextFile ()
	{
		List<string> list = LoadTextFile();
		string single = "";
		foreach(string line in list)
		{
			single += line + System.Environment.NewLine;
		}
		single = single.TrimEnd(null);
		return single;
	}

	public bool SaveTextFile (string data)
	{
		List<string> list = new List<string>(data.Split('\n'));
		return this.SaveTextFile(list);
	}

	#endregion
}
