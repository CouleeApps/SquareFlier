using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D))]
public class Player : MonoBehaviour {

	public float horizForce = 30.0f;
	public float upForce = 30.0f;
	public float downForce = 20.0f;

	public float projectileVelocity = 10.0f;
	public float shootTime = 0.2f;
	public float projectileOffset = 0.3f;
	public GameObject projectilePrefab;

	private Rigidbody2D rigid;
	private float lastShoot;

	// Use this for initialization
	void Start () {
		rigid = GetComponent<Rigidbody2D>();
		lastShoot = Time.time;

		if (Camera.main && Camera.main.GetComponent<FollowCamera>()) {
			Camera.main.GetComponent<FollowCamera>().target = gameObject;
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void FixedUpdate() {
        //Don't move unless the game is running
        if (!Physics2D.autoSimulation)
        {
            return;
        }

		//So we know which direction to fire
		var up    = Input.GetKey("up")    || Input.GetKey("w");
		var down  = Input.GetKey("down")  || Input.GetKey("s");
		var left  = Input.GetKey("left")  || Input.GetKey("a");
		var right = Input.GetKey("right") || Input.GetKey("d");

		//In case some idiot tries to use a controller
		var horiz = Input.GetAxisRaw("Horizontal");
		var vert = Input.GetAxisRaw("Vertical");

		//Convert binary keys to analog input to force
		if (horiz == 0) {
			horiz = (left ? -1 : 0) + (right ? 1 : 0);
		} else {
			horiz *= horizForce;
			if (horiz < 0) {
				left = true;
			} else if (horiz > 0) {
				right = true;
			}
		}
		if (vert == 0) {
			//Upforce - downforce ~= gravity
			vert = (up ? upForce : 0) + (down ? -downForce : 0);
		} else if (vert < 0) {
			down = true;
			vert *= downForce;
		} else if (vert > 0) {
			up = true;
			vert *= upForce;
		}

		rigid.AddForce(new Vector2(horiz, vert));

		if (Time.time - lastShoot > shootTime) {
			lastShoot = Time.time;

			if (up) {
				Shoot(new Vector2(0, -1));
			}
			if (down) {
				Shoot(new Vector2(0, 1));
			}
			if (left) {
				Shoot(new Vector2(1, 0));
			}
			if (right) {
				Shoot(new Vector2(-1, 0));
			}
		}
	}

	void Shoot(Vector2 direction) {
		var proj = GameObject.Instantiate(projectilePrefab);
		proj.transform.position = transform.position + new Vector3(direction.x, direction.y, 0) * projectileOffset;
		proj.GetComponent<Rigidbody2D>().velocity = rigid.velocity + direction * projectileVelocity;
	}

	public void Respawn() {
		GameObject spawnPoint = LevelManager.GetSpawnPoint();
		Rigidbody2D rigid = GetComponent<Rigidbody2D>();
		rigid.position = spawnPoint.transform.position;
		rigid.rotation = 0;
		rigid.velocity = Vector2.zero;
		rigid.angularVelocity = 0;

        transform.position = spawnPoint.transform.position;
	}
}
