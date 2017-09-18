using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

/// <summary>
/// Basic manager that controls both the current time and the UI
/// </summary>
public class TimeManager : MonoBehaviour {
	public static TimeManager CurrentManager;
	public GameObject uiTimer;
	public GameObject uiCanvas;

	/// <summary>
	/// Time in seconds
	/// </summary>
	private float currentTime;
	private bool timerRunning;

	public UnityEvent OnStartTimer;
	public UnityEvent OnStopTimer;
	public UnityEvent OnTimeUpdate;

	void Start() {
		//Persist this, make it global
		DontDestroyOnLoad(gameObject);
		CurrentManager = this;
	}
	
	// Update is called once per frame
	void Update() {
		if (timerRunning) {
			currentTime += Time.deltaTime;
			uiTimer.GetComponent<Text>().text = FormatTime(currentTime);

			OnTimeUpdate.Invoke();
		}
	}

	public string FormatTime(float time) {
		int mins = Mathf.FloorToInt(time) / 60;
		int secs = Mathf.FloorToInt(time) % 60;
		int mils = Mathf.FloorToInt(time * 1000) % 1000;

		return string.Format("{0:00}:{1:00}.{2:000}", mins, secs, mils);
	}

	public void ShowTimer(bool shown) {
		uiCanvas.GetComponent<Canvas>().enabled = shown;
	}

	public void StartTimer() {
		currentTime = 0.0f;
		timerRunning = true;

		OnStartTimer.Invoke();
	}

	public void StopTimer() {
		timerRunning = false;

		OnStopTimer.Invoke();
	}
}
