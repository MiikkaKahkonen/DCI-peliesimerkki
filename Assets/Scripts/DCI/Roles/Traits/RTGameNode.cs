using UnityEngine;
using System.Collections;

public static class RTGameNode
{
	public static void SetEWord(this RGameNode self)
	{
		System.Text.RegularExpressions.Regex rgx = new System.Text.RegularExpressions.Regex("[^ ]");

		string eword = self.Word;
		eword = rgx.Replace(self.Word, "X");
		eword += "("+(self.Word.Replace(" ","")).Length+")";

		self.EWord = eword;
	}

	public static string ToTexT(this RGameNode self)
	{
		string result = "";
		result += "node";
		result += BaseFileSystem.ATTSEP;
		result += "id"+BaseFileSystem.VALSEP+self.Id;
		result += BaseFileSystem.ATTSEP;
		result += "word"+BaseFileSystem.VALSEP+self.Word;
		result += BaseFileSystem.ATTSEP;
		result += "state"+BaseFileSystem.VALSEP+self.State;
		return result;
	}

	public static bool CompaireQuess(this RGameNode self, string quess)
	{
		return self.Word == quess;
	}
}