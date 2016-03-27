using UnityEngine;
using System.Collections;

public class Unit : MonoBehaviour {


	public Transform target;
	float speed = 1;
	Vector3[] path;
	int target_index;

	void Start()
	{
		PathRequestManager.RequestPath (transform.position, target.position, OnPathFound);
	}

	public void OnPathFound(Vector3[] new_path, bool path_successful)
	{

		if (path_successful) 
		{
			path = new_path;
			StopCoroutine ("FollowPath");
			StartCoroutine ("FollowPath");
		}
	}

	IEnumerator FollowPath()
	{
		Vector3 current_waypoint = path [0];
		while (true) {
			if (transform.position == current_waypoint) 
			{
				target_index++;
				if (target_index >= path.Length) 
				{
					yield break;
				}
				current_waypoint = path [target_index];
			}

			transform.position = Vector3.MoveTowards (transform.position, current_waypoint, speed);
			yield return null;
		}
	}

	public void OnDrawGizmos()
	{
		if (path != null) {
			for (int i = target_index; i < path.Length; i++) 
			{
				Gizmos.color = Color.black;
				Gizmos.DrawCube (path [i], Vector3.one);

				if (i == target_index) 
				{
					Gizmos.DrawLine (transform.position, path [i]);
				} 
				else 
				{
					Gizmos.DrawLine (path [i - 1], path [i]);
				}
			}
		}
	}
}
