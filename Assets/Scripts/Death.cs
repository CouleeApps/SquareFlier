using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class Death : NetworkBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnCollisionEnter2D(Collision2D coll) {
		if (coll.gameObject.GetComponent<Bullet>()) {
			GameObject.Destroy(coll.gameObject);
		}
		if (coll.gameObject.GetComponent<Player>()) {
			coll.gameObject.GetComponent<Player>().CmdRespawn();
		}
	}
}
