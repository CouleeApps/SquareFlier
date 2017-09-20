using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class HUD : MonoBehaviour
{
    public GameObject canvas;

    /// <summary>
    /// Timer
    /// </summary>
    public GameObject timerPanel;
    public GameObject timerText;

    /// <summary>
    /// Pause Menu
    /// </summary>
    public GameObject pausePanel;

    // Use this for initialization
    void Start()
    {
        //Persist this, make it global
        DontDestroyOnLoad(gameObject);
        SceneManager.sceneLoaded += SceneLoaded;

        TimeManager.CurrentManager.OnStartTimer.AddListener(UpdateTimer);
        TimeManager.CurrentManager.OnStopTimer.AddListener(UpdateTimer);
        TimeManager.CurrentManager.OnTimeUpdate.AddListener(UpdateTimer);

        ShowPauseMenu = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ShowPauseMenu = !ShowPauseMenu;
        }
    }

    private void SceneLoaded(Scene scene, LoadSceneMode mode)
    {
        ShowTimer = ShouldShowTimer();
    }

    #region Pause Menu

    public bool ShowPauseMenu
    {
        get
        {
            return pausePanel.activeSelf;
        }
        set
        {
            pausePanel.SetActive(value);
        }
    }

    public void EndLevel()
    {
        ShowPauseMenu = false;
        LevelManager.CurrentManager.LoadLevelSelect();
    }

    public void RestartLevel()
    {
        ShowPauseMenu = false;
        LevelManager.CurrentManager.RestartLevel();
    }

    #endregion

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