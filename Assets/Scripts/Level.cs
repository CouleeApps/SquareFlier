using UnityEngine;
using System.Collections;

public class Level : MonoBehaviour {

	public GameObject startArea;
	public GameObject endArea;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public IEnumerator SpawnPlayer(GameObject prefab) {
		yield return new WaitForSeconds(0.2f);

		var player = GameObject.Instantiate(prefab);
		var rigid = player.GetComponent<Rigidbody2D>();
		rigid.rotation = 0;
		rigid.angularVelocity = 0;
		rigid.velocity = Vector2.zero;

		var width = player.transform.localScale;

		Vector2 lowBound = startArea.transform.position - (startArea.transform.localScale / 2.0f) + (width / 2.0f);
		Vector2 highBound = startArea.transform.position + (startArea.transform.localScale / 2.0f) - (width / 2.0f);

		Vector2 spawn = new Vector2(Random.Range(lowBound.x, highBound.x), Random.Range(lowBound.y, highBound.y));
		rigid.position = spawn;
	}
}
