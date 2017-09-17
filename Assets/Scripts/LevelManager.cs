using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class LevelManager : MonoBehaviour {

	public static LevelManager currentManager;

	public GameObject playerPrefab;
	public SceneField[] levelScenes;
	public string[] levelNames;
	public int currentLevel;
	public bool isLevelSelect = true;

	private string currentLevelScene;

	void Start() {
		DontDestroyOnLoad(gameObject);
		currentManager = this;
		SceneManager.LoadScene("LevelSelect");
		isLevelSelect = true;

		SceneManager.activeSceneChanged += SceneActivated;
		SceneManager.sceneLoaded += SceneLoaded;
	}

	// Update is called once per frame
	void Update() {
	
	}

	public void NextLevel() {
		int level = currentLevel + 1;
		LoadLevel(level);
	}

	public void LoadLevel(int levelNum) {
		if (currentLevel == levelNum && (currentLevelScene != null))
			return;
		
		//Back to menu
		if (levelNum >= levelScenes.Length || levelNum < 0) {
			SceneManager.LoadScene("LevelSelect");
			isLevelSelect = true;
			return;
		}

		isLevelSelect = false;
		Debug.Log("Loading level #" + levelNum);

		currentLevel = levelNum;
		currentLevelScene = levelScenes[currentLevel].SceneName;
	
		//Load the scene they suggested
		SceneManager.LoadScene(currentLevelScene);
	}

	void SceneLoaded(Scene scene, LoadSceneMode mode) {
	}

	void SceneActivated(Scene oldScene, Scene newScene) {
		if (IsLevelSelect()) {
			return;
		}

		//Spawn player
		var newPlayer = GameObject.Instantiate(playerPrefab);
		newPlayer.GetComponent<Player>().Respawn();

		GameObject[] endPoints = GetEndPoints();
		foreach (GameObject obj in endPoints) {
			obj.GetComponent<EndArea>().winEvent.AddListener(() => {
				this.NextLevel();
			});
		}
	}

	public static bool IsLevelSelect() {
		return currentManager.isLevelSelect;
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
