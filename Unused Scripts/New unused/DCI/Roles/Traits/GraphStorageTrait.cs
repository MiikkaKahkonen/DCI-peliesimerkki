using UnityEngine;
using System.Collections;
using DCI;


namespace DCI.Roles
{
	public static class GraphStorageTrait
	{
		public static string GraphToStorage(this GraphStorage self,GameGraph graph)
		{
			self.Name = graph.Name;
			self.Data = graph;

			return self.Name;
		}
		public static GameGraph StorageToGraph(this GraphStorage self,string name)
		{
			self.Name = name;
			return self.Data;
		}

	}
}