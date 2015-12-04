using UnityEngine;
using System.Collections;

namespace DCI.Roles
{
	public static class GameEdgeTrait
	{
		public static void Update(this GameEdge self,string message)
		{

		}

		public static string Report(this GameEdge self)
		{
			string report = "Edge: "+self.Id+"";

			if(self.Head != null)
			{
				report += "\nHead: "+self.Head.Id+"";
			}
			else
			{
				report += "\nHead: null";
			}
			if(self.Tail != null)
			{
				report += "\nTail: "+self.Head.Id+"";
			}
			else
			{
				report += "\nTail: null";
			}
			return report;
		}
	}
}