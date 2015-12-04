using UnityEngine;
using System.Collections;

public interface RGameGraph : RGameItem
{
	GameNodeList Nodes {get;set;}
	GameEdgeList Edges {get;set;}

	RGameNode NewNode {get;}
	RGameEdge NewEdge {get;}
}
