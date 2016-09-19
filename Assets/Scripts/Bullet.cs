using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class Bullet : NetworkBehaviour {

	public float destroyTime = 2.0f;
	public float fadeoutTime = 1.0f;
	public float updateTime = 1.0f;
	private float lastUpdate = 0;
	private float startTime;

	private Rigidbody2D rigid;

	// Use this for initialization
	void Start () {
		lastUpdate = 0;
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

		if (Time.time > (lastUpdate + updateTime)) {
			lastUpdate = Time.time;
			SetDirtyBit(1);
		}
	}

	void OnCollisionEnter2D(Collision2D coll) {
		GetComponent<Rigidbody2D>().collisionDetectionMode = CollisionDetectionMode2D.Discrete;
	}

	public override bool OnSerialize(NetworkWriter writer, bool initial) {
		writer.Write(GetComponent<Rigidbody2D>().position);
		writer.Write(GetComponent<Rigidbody2D>().velocity);
		writer.Write(GetComponent<Rigidbody2D>().rotation);
		writer.Write(GetComponent<Rigidbody2D>().angularVelocity);
		return true;
	}

	public override void OnDeserialize(NetworkReader reader, bool initial) {
		float delta = Time.deltaTime;
		Vector2 position = reader.ReadVector2();
		Vector2 velocity = reader.ReadVector2();
		float rotation = reader.ReadSingle();
		float angularVelocity = reader.ReadSingle();

		var rigid = GetComponent<Rigidbody2D>();
		rigid.position = position + (velocity * delta);
		rigid.rotation = rotation + (angularVelocity * delta);
		rigid.velocity = velocity;
		rigid.angularVelocity = angularVelocity;
	}
}
