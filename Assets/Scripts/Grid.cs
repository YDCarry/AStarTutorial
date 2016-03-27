using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Grid : MonoBehaviour 
{
	public bool display_grid_gizmos;
	public LayerMask unwalkable_mask;
	public Vector2 grid_world_size;
	public float node_radius;
	Node[,] grid;

	float node_diameter;
	int grid_size_x, grid_size_y;

	void Awake()
	{
		node_diameter = node_radius * 2;
		grid_size_x = Mathf.RoundToInt(grid_world_size.x/node_diameter);
		grid_size_y = Mathf.RoundToInt(grid_world_size.y/node_diameter);
		CreateGrid ();
	}

	public int MaxSize
	{
		get
		{
			return grid_size_x * grid_size_y;
		}
	}

	void CreateGrid()
	{
		grid = new Node[grid_size_x,grid_size_y];
		Vector3 world_bottom_left = transform.position - Vector3.right * grid_world_size.x / 2 - Vector3.forward * grid_world_size.y / 2;

		for(int x=0; x<grid_size_x; x++)
		{
			for(int y=0; y<grid_size_y; y++)
			{
				Vector3 world_point = world_bottom_left + Vector3.right * (x * node_diameter + node_radius) + Vector3.forward * (y * node_diameter + node_radius);
				bool walkable = !(Physics.CheckSphere (world_point, node_radius,unwalkable_mask));
				grid[x, y] = new Node (walkable, world_point, x, y);
			}
		}
	}

	public List<Node> GetNeighbours(Node node)
	{
		List<Node> neighours = new List<Node> ();

		for (int x = -1; x <= 1; x++) {
			for (int y = -1; y <= 1; y++) {
				if (x == 0 && y == 0)
					continue;

				int checkX = node.grid_x + x;
				int checkY = node.grid_y + y;

				if (checkX >= 0 && checkX < grid_size_x && checkY >= 0 && checkY < grid_size_y) {
					neighours.Add (grid[checkX,checkY]);
				}
			}

		}
		return neighours;
	}

	public Node NodeFromWorldPoint(Vector3 world_position)
	{
		float percent_x = (world_position.x + grid_world_size.x / 2) / grid_world_size.x;
		float percent_y = (world_position.z + grid_world_size.y / 2) / grid_world_size.y;
		percent_x = Mathf.Clamp01 (percent_x);
		percent_y = Mathf.Clamp01 (percent_y);

		int x = Mathf.RoundToInt((grid_size_x - 1) * percent_x);
		int y = Mathf.RoundToInt((grid_size_y - 1) * percent_y);
		return grid [x, y];
	}
}
