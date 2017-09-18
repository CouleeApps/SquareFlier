using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class HUD : MonoBehaviour {
    public GameObject canvas;

    /// <summary>
    /// Timer
    /// </summary>
    public GameObject timerPanel;
    public GameObject timerText;

    // Use this for initialization
    void Start() {
        //Persist this, make it global
        DontDestroyOnLoad(gameObject);
        SceneManager.sceneLoaded += SceneLoaded;

        TimeManager.CurrentManager.OnStartTimer.AddListener(UpdateTimer);
        TimeManager.CurrentManager.OnStopTimer.AddListener(UpdateTimer);
        TimeManager.CurrentManager.OnTimeUpdate.AddListener(UpdateTimer);
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void SceneLoaded(Scene scene, LoadSceneMode mode)
    {
        ShowTimer = ShouldShowTimer();
    }

#region Timer
    public bool ShouldShowTimer()
    {
        return !LevelManager.IsLevelSelect() && PreferenceManager.CurrentManager.ShowTimer;
    }

    private void UpdateTimer()
    {
        //Don't update UI if we're not showing
        if (!ShowTimer)
        {
            return;
        }
        timerText.GetComponent<Text>().text = TimeManager.FormatTime(TimeManager.CurrentManager.CurrentTime);
    }

    public bool ShowTimer
    {
        get
        {
            return timerPanel.activeSelf;
        }
        set
        {
            timerPanel.SetActive(value);
        }
    }
#endregion

}
