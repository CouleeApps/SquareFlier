using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour {

	public float destroyTime = 2.0f;
	public float fadeoutTime = 1.0f;
	public float updateTime = 1.0f;
	private float startTime;

	private Rigidbody2D rigid;

	// Use this for initialization
	void Start () {
		startTime = Time.time;
		rigid = GetComponent<Rigidbody2D>();
		rigid.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
	}
	
	// Update is called once per frame
	void Update () {
		var elapsed = Time.time - startTime;

		if (elapsed > destroyTime - fadeoutTime) {
			Color newColor = GetComponent<SpriteRenderer>().color;
			newColor.a = 1.0f - ((elapsed - (destroyTime - fadeoutTime)) / fadeoutTime);
			GetComponent<SpriteRenderer>().color = newColor;
		}
		if (elapsed > destroyTime) {
			GameObject.Destroy(gameObject);
		}
	}

	void OnCollisionEnter2D(Collision2D coll) {
		GetComponent<Rigidbody2D>().collisionDetectionMode = CollisionDetectionMode2D.Discrete;
	}
}
