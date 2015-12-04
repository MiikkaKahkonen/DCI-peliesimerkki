using UnityEngine;
using System.Collections;

public class Data
{
	private static long idNum = 0;
	string _id;

    public Data()
    {
        Id = GetNextId();
    }

	public string Id
	{
		get {
			return _id;
		}
		set {
			_id = value;
		}
	}

	public string GetNextId()
	{
		idNum++;
		return this.GetType().ToString()+":"+idNum;
	}
}
