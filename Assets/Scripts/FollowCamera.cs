using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Camera))]
public class FollowCamera : MonoBehaviour {

	public GameObject target;
	public float distance = -10.0f;

	// Use this for initialization
	void Start() {

	}

	// Update is called once per frame
	void Update() {
		if (target) {
			transform.position = new Vector3(target.transform.position.x, target.transform.position.y, distance);
		}
	}
}
