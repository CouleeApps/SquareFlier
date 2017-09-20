using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Basic manager that controls both the current time and the UI
/// </summary>
public class TimeManager : MonoBehaviour {
	public static TimeManager CurrentManager;

	/// <summary>
	/// Time in seconds
	/// </summary>
	public float CurrentTime;
	public bool TimerRunning;

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
		if (TimerRunning) {
			CurrentTime += Time.deltaTime;

			OnTimeUpdate.Invoke();
		}
	}

	public static string FormatTime(float time) {
		int mins = Mathf.FloorToInt(time) / 60;
		int secs = Mathf.FloorToInt(time) % 60;
		int mils = Mathf.FloorToInt(time * 1000) % 1000;

		return string.Format("{0:00}:{1:00}.{2:000}", mins, secs, mils);
	}

	public void StartTimer() {
		CurrentTime = 0.0f;
		TimerRunning = true;

		OnStartTimer.Invoke();
	}

	public void StopTimer() {
		TimerRunning = false;

		OnStopTimer.Invoke();
	}

    public void ResetTimer()
    {
        CurrentTime = 0.0f;
        StopTimer();
    }
}
