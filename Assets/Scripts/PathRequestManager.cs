using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class PathRequestManager : MonoBehaviour {

	Queue<PathRequest> path_request_queue = new Queue<PathRequest>();
	PathRequest current_path_request;

	static PathRequestManager instance;
	AStar astar;

	bool isProcessingPath;

	void Awake()
	{
		instance = this;
		astar = GetComponent<AStar> ();
	}

	public static void RequestPath(Vector3 path_start, Vector3 path_end, Action<Vector3[], bool> callback)
	{
		PathRequest new_request = new PathRequest(path_start, path_end, callback);
		instance.path_request_queue.Enqueue(new_request);
		instance.TryProcessNext();
	}
	void TryProcessNext()
	{
		if (!isProcessingPath  && path_request_queue.Count > 0) 
		{
			current_path_request = path_request_queue.Dequeue ();
			isProcessingPath = true;
			astar.StartFindPath (current_path_request.path_start, current_path_request.path_end);
		}
	}

	public void FinishedProcessingPath(Vector3[] path, bool success)
	{
		current_path_request.callback (path, success);
		isProcessingPath = true;
		TryProcessNext ();
	}

	struct PathRequest
	{
		public Vector3 path_start;
		public Vector3 path_end;
		public Action<Vector3[], bool> callback;

		public PathRequest(Vector3 _start, Vector3 _end, Action<Vector3[],bool> _callback)
		{
			path_start = _start;
			path_end = _end;
			callback = _callback;
		}
	}
}
