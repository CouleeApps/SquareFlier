using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class LevelManager : MonoBehaviour {

	public static LevelManager CurrentManager;

	public GameObject playerPrefab;
	public SceneField[] levelScenes;
	public string[] levelNames;
	public int currentLevel;
	public GameObject currentPlayer;
	public bool isLevelSelect = true;
    public bool isGameRunning = false;

	private string currentLevelScene;

	void Start() {
		DontDestroyOnLoad(gameObject);
		CurrentManager = this;
		LoadLevelSelect();

		SceneManager.activeSceneChanged += SceneActivated;
		SceneManager.sceneLoaded += SceneLoaded;
	}

	// Update is called once per frame
	void Update() {
	
	}

	public void LoadLevelSelect() {
		SceneManager.LoadScene("LevelSelect");
		isLevelSelect = true;
		currentLevel = -1;
    }

    public void NextLevel() {
		int level = currentLevel + 1;
		LoadLevel(level);
	}

    public void RestartLevel()
    {
        Debug.Log("Restarting level #" + currentLevel);

        //Load the scene they suggested
        SceneManager.LoadScene(currentLevelScene);
    }

    public void LoadLevel(int levelNum) {
		if (currentLevel == levelNum && (currentLevelScene != null))
			return;
		
		//Back to menu
		if (levelNum >= levelScenes.Length || levelNum < 0) {
			LoadLevelSelect();
			return;
		}
		
		isLevelSelect = false;
		Debug.Log("Loading level #" + levelNum);

        TimeManager.CurrentManager.ResetTimer();

        currentLevel = levelNum;
		currentLevelScene = levelScenes[currentLevel].SceneName;
	
		//Load the scene they suggested
		SceneManager.LoadScene(currentLevelScene);
	}

	void SceneLoaded(Scene scene, LoadSceneMode mode) {
        isGameRunning = false;
        Physics2D.autoSimulation = false;
	}

	void SceneActivated(Scene oldScene, Scene newScene) {
		if (IsLevelSelect()) {
			return;
		}
		LevelLoaded();
	}

    private void LevelLoaded() {
        Invoke("StartLevel", 1.0f);
        isGameRunning = false;

        GameObject[] endPoints = GetEndPoints();
        foreach (GameObject obj in endPoints) {
            obj.GetComponent<EndArea>().winEvent.AddListener(() => {
                this.LevelEnded();
            });
        }

        //Spawn player
        currentPlayer = GameObject.Instantiate(playerPrefab);
        currentPlayer.GetComponent<Player>().Respawn();
    }

    private void StartLevel()
    {
        Physics2D.autoSimulation = true;
        isGameRunning = true;
        TimeManager.CurrentManager.StartTimer();
	}

	private void LevelEnded() {
		TimeManager.CurrentManager.StopTimer();
        isGameRunning = false;
        Destroy(currentPlayer);

		Invoke("NextLevel", 1.0f);
	}

	public static bool IsLevelSelect() {
		return CurrentManager.isLevelSelect;
	}

	public static GameObject GetSpawnPoint() {
		GameObject[] roots = SceneManager.GetActiveScene().GetRootGameObjects();
		foreach (GameObject obj in roots) {
			if (obj.GetComponent<SpawnPoint>()) {
				return obj;
			}
		}
		return null;
	}

	public static GameObject[] GetEndPoints() {
		List<GameObject> endPoints = new List<GameObject>();
		GameObject[] roots = SceneManager.GetActiveScene().GetRootGameObjects();
		foreach (GameObject obj in roots) {
			if (obj.GetComponent<EndArea>()) {
				endPoints.Add(obj);
			}
		}
		return endPoints.ToArray();
	}
}
