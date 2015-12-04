using UnityEngine;
using System.Collections;

public interface RGameNode : RGameItem
{
	string Word {get;set;}
	int State {get;set;}
	string EWord {get;set;}
	bool ESet {get;}
}
