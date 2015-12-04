using UnityEngine;
using System.Collections;

public static class RTGameEdge
{
	public static string ToTexT(this RGameEdge self)
	{
		string result = "";
		result += "edge";
		result += BaseFileSystem.ATTSEP;
		result += "head"+BaseFileSystem.VALSEP+self.HeadId;
		result += BaseFileSystem.ATTSEP;
		result += "tail"+BaseFileSystem.VALSEP+self.TailId;
		return result;
	}
}