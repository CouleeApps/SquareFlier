using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class EndArea : MonoBehaviour {

	public float activationTime = 2.0f;
	private float insideTime = 0.0f;
	private int playersInside = 0;

	public UnityEvent winEvent;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void FixedUpdate() {
		if (playersInside > 0) {
			insideTime = Mathf.Clamp(insideTime + (Time.fixedDeltaTime * playersInside), 0, activationTime);
		} else {
			insideTime = Mathf.Clamp(insideTime - Time.fixedDeltaTime, 0, activationTime);
		}

		if (insideTime >= activationTime) {
			insideTime = 0;
			winEvent.Invoke();
		}

		Color newColor = GetComponent<SpriteRenderer>().color;
		newColor.a = insideTime / activationTime;
		GetComponent<SpriteRenderer>().color = newColor;
	}

	void OnTriggerEnter2D(Collider2D coll) {
		if (coll.GetComponent<Player>()) {
			playersInside ++;
		}
	}

	void OnTriggerExit2D(Collider2D coll) {
		if (coll.GetComponent<Player>()) {
			playersInside --;
		}
	}
}
