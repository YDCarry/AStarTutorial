using UnityEngine;
using System.Collections;

public class Node:IHeapItem<Node>{

	public bool walkable;
	public Vector3 world_position;
	public int grid_x;
	public int grid_y;

	public int g_cost;
	public int h_cost;

	public Node parent;
	public int heap_index;

	public int f_cost()
	{
		return g_cost + h_cost;
	}

	public Node(bool _walkable, Vector3 _worldpos, int _gridx, int _gridy)
	{
		walkable = _walkable;
		world_position = _worldpos;
		grid_x = _gridx;
		grid_y = _gridy;
	}


	public int HeapIndex
	{
		get
		{ 
			return heap_index;
		}
		set
		{ 
			heap_index = value;
		}
	}

	public int CompareTo(Node node)
	{
		int compare = f_cost().CompareTo (node.f_cost());
		if (compare == 0) 
		{
			compare = h_cost.CompareTo (node.h_cost);
		}
		return -compare;
	}

}
