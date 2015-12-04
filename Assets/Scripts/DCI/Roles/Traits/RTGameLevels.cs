using UnityEngine;
using System.Collections;

public static class RTGameLevels
{
	public static string[] GetLevelNames(this RGameLevels self)
	{
        string[] arr = new string[self.levels.Count];
        int counter = 0;
        foreach (string key in self.levels.Keys)
        {
            arr[counter] = key;
            counter++;
        }
        return arr;
	}

    public static RGameGraph GetGraph(this RGameLevels self, string key)
    {
        return self.levels[key];
    }
    
    public static void LoadLevels(this RGameLevels self)
    {
        BaseFileSystem bfs = BaseFileSystem.GetInstance();
        string[] names = bfs.GetFileNames();

        foreach(string name in names)
        {
            DGraph graph = new DGraph();
            self.levels.Add(name, graph);
            Manager.Message("Load", name);
        }
    }
}