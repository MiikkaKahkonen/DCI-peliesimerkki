using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NodePair
{
	RGameNode _head;
	RGameNode _tail;

	public NodePair(RGameNode head, RGameNode tail)
	{
		_head = head;
		_tail = tail;
	}


	public RGameNode Head {
		get {
			return _head;
		}
	}

	public RGameNode Tail {
		get {
			return _tail;
		}
	}
}

