using UnityEngine;
using System.Collections;

public class DNode : Data, RGameNode
{
	string _word;
	int _state;
	string _eword;
	bool _eSet = false;

	#region RGameNode implementation

	public string Word {
		get {
			return _word;
		}
		set {
			_word = value;
		}
	}

	public int State {
		get {
			return _state;
		}
		set {
			_state = value;
		}
	}

	public string EWord {
		get 
		{
			return _eword;
		}
		set
		{
			if(!_eSet)
			{
				_eword = value;
				_eSet = true;
			}
		}
	}

	public bool ESet {
		get{
			return _eSet;
		}
	}
	#endregion


}
