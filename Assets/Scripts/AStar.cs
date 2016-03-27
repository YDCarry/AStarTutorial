using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System;
public class AStar : MonoBehaviour {

	PathRequestManager request_manager;
	Grid grid;

	void Awake()
	{
		request_manager = GetComponent<PathRequestManager> ();
		grid = GetComponent<Grid> ();
	}

	public void StartFindPath(Vector3 start_pos, Vector3 target_pos)
	{
		StartCoroutine (FindPath (start_pos, target_pos));
	}

	IEnumerator FindPath(Vector3 start_pos, Vector3 target_pos)
	{
		Stopwatch sw = new Stopwatch();
		sw.Start();

		Vector3[] waypoints = new Vector3[0];
		bool path_success = false;

		Node start_node = grid.NodeFromWorldPoint (start_pos);
		Node target_node = grid.NodeFromWorldPoint (target_pos);

		if(start_node.walkable && target_node.walkable)
		{		
			Heap<Node> open_set = new Heap<Node> (grid.MaxSize);
			HashSet<Node> closed_set = new HashSet<Node> ();
			open_set.Add (start_node);

			while (open_set.Count > 0) 
			{
				Node current_node = open_set.RemoveFirst();
				closed_set.Add (current_node);

				if (current_node == target_node) 
				{
					sw.Stop();
					print("Path found: " + sw.ElapsedMilliseconds + "ms");
							path_success = true;
					
					break;
				}

				foreach (Node neighbour in grid.GetNeighbours(current_node)) 
				{
					if (!neighbour.walkable || closed_set.Contains (neighbour))
						continue;
					int new_cost = current_node.g_cost + GetDistance (current_node, neighbour);
					if (new_cost < neighbour.g_cost || !open_set.Contains (neighbour)) 
					{
						neighbour.g_cost = new_cost;
						neighbour.h_cost = GetDistance (current_node, neighbour);
						neighbour.parent = current_node;

						if (!open_set.Contains (neighbour)) 
						{
							open_set.Add (neighbour);
						} 
						else 
						{
							open_set.UpdateItem (neighbour);
						}
					}
				}
			}
			yield return null;
			if(path_success)
			{
				waypoints = RetracePath (start_node, target_node);;
			}
			request_manager.FinishedProcessingPath(waypoints, path_success);
		}
	}

	Vector3[] RetracePath(Node start_node, Node end_node)
	{
		List<Node> path = new List<Node> ();
		Node current_node = end_node;
		while (current_node != start_node) 
		{
			path.Add(current_node);
			current_node = current_node.parent;
		}
		path.Add (start_node);
		Vector3[] waypoints = SimplifyPath (path);
		Array.Reverse (waypoints);
		return waypoints;
	}
	
	Vector3[] SimplifyPath(List<Node> path)
	{
		List<Vector3> waypoints = new List<Vector3> ();
		Vector2 direction_old = Vector2.zero;

		for (int i = 1; i < path.Count; i++) 
		{
			Vector2 direction_new = new Vector2 (path [i - 1].grid_x - path [i].grid_x, path [i - 1].grid_y - path [i].grid_y);
			if (direction_new != direction_old) 
			{
				waypoints.Add (path [i].world_position);
			}
			direction_old = direction_new;
		}
		return waypoints.ToArray ();
	}
	
	int GetDistance(Node nodeA, Node nodeB)
	{
		int dstX = Mathf.Abs (nodeA.grid_x - nodeB.grid_x);
		int dstY = Mathf.Abs (nodeA.grid_y - nodeB.grid_y);

		if (dstX > dstY)
			return 14 * dstY + 10 * (dstX - dstY);
		return 14 * dstX + 10 * (dstY - dstX);
	}
}
