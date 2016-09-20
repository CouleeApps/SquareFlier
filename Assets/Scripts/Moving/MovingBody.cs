using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class MovingBody : MonoBehaviour {

	public Path path;

	private float startTime;

	private Rigidbody2D rbComponent;

	// Use this for initialization
	void Start() {
		startTime = Time.time;
		rbComponent = GetComponent<Rigidbody2D>();
	}

	// Update is called once per frame
	void Update() {
		//Get the path's position at our current time
		float current = Time.time - startTime;
		path.UpdatePath(current);

		//Update our transform so we move
		transform.position = path.getPathPosition();
		transform.rotation = Quaternion.Euler(new Vector3(0, 0, path.getPathRotation()));

		//Update the rigid body's velocities so we don't bump around
		rbComponent.velocity = path.getPathLinearVelocity();
		rbComponent.angularVelocity = path.getPathAngularVelocity();
	}
}
