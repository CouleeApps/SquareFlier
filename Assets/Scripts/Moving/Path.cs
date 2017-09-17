using UnityEngine;
using System.Collections;

namespace SquareFlier {

/// <summary>
/// Basic path class. Add children which have a PathNode component for the nodes along the path.
/// </summary>
public class Path : MonoBehaviour {

	public bool looping;

	private int nodeCount;
	private PathNode[] nodes;

	private Vector3 currentPosition;
	private Vector3 currentLinearVelocity;
	private float currentRotation;
	private float currentAngularVelocity;

	// Use this for initialization
	void Start() {
		UpdateCache();
	}

	// Update is called once per frame
	void Update() {
	}

	/// <summary>
	/// Update the internal child node cache and node length. Call this when you add or
	/// delete a node from the path.
	/// </summary>
	public void UpdateCache() {
		nodes = GetComponentsInChildren<PathNode>();
		nodeCount = nodes.Length;
	}

	/// <summary>
	/// Gets the number of nodes that are children of this Path
	/// </summary>
	/// <returns>The number of nodes</returns>
	public int getNodeCount() {
		return nodeCount;
	}

	/// <summary>
	/// Get a node along the path by index, with support for wrap around
	/// </summary>
	/// <returns>The node for that index</returns>
	/// <param name="index">Index of the node (negatives and overflows wrap around)</param>
	public PathNode getNode(int index) {
		while (index < 0) {
			index += nodeCount;
		}
		index %= nodeCount;
		return nodes[index];
	}

	/// <summary>
	/// Update the current path position, rotation, and velocities. Note that you have
	/// to call this before using any of the getPath<X> methods to ensure their values
	/// are updated.
	/// </summary>
	/// <param name="time">The time offset from the beginning of the path</param>
	public void UpdatePath(float time) {
		if (!looping && time >= getTotalTime()) {
			//At the end
			PathNode node = getNode(getNodeCount() - 1);

			currentPosition = node.transform.position;
			currentRotation = node.transform.rotation.eulerAngles.z;

			currentLinearVelocity = Vector3.zero;
			currentAngularVelocity = 0;
		}
		//Normalize
		time %= getTotalTime();

		for (int i = 0; i < nodeCount; i ++) {
			PathNode node = getNode(i);
			//Passed this node, go to the next one
			if (time > node.timeToNext) {
				time -= node.timeToNext;
				continue;
			}
			//TODO: Implement spline interpolation or something
//			PathNode prev = getNode(i - 1);
			PathNode next = getNode(i + 1);

			//Basic linear interpolation
			currentPosition = Vector3.Lerp(node.transform.position, next.transform.position, time / node.timeToNext);
			currentAngularVelocity = (next.transform.rotation.eulerAngles.z - node.transform.rotation.eulerAngles.z) / node.timeToNext;

			//Just divide
			currentRotation = Mathf.LerpAngle(node.transform.rotation.eulerAngles.z, next.transform.rotation.eulerAngles.z, time / node.timeToNext);
			currentLinearVelocity = (next.transform.position - node.transform.position) / node.timeToNext;
			return;
		}
	}

	/// <summary>
	/// Gets the position along the path at the last call to UpdatePath.
	/// </summary>
	/// <returns>The current position</returns>
	public Vector3 getPathPosition() {
		return currentPosition;
	}
	/// <summary>
	/// Gets the linear velocity along the path at the last call to UpdatePath.
	/// </summary>
	/// <returns>The current linear velocity</returns>
	public Vector3 getPathLinearVelocity() {
		return currentLinearVelocity;
	}
	/// <summary>
	/// Gets the current rotation of the path (in radians) at the last call to UpdatePath.
	/// </summary>
	/// <returns>The current rotation</returns>
	public float getPathRotation() {
		return currentRotation;
	}
	/// <summary>
	/// Gets the current angular velocity (in rads / sec) at the last call to UpdatePath.
	/// </summary>
	/// <returns>The current angular velocity</returns>
	public float getPathAngularVelocity() {
		return currentAngularVelocity;
	}

	/// <summary>
	/// Get the total time in seconds of all path nodes which are children of this Path.
	/// </summary>
	/// <returns>The total time in seconds</returns>
	public float getTotalTime() {
		//Sum all child times
		float time = 0;
		foreach (PathNode node in GetComponentsInChildren<PathNode>()) {
			time += node.timeToNext;
		}
		return time;
	}
}

}