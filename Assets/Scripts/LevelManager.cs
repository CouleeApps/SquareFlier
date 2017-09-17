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

	private string currentLevelScene;

	void Start() {
		DontDestroyOnLoad(gameObject);
		currentManager = this;
		SceneManager.LoadScene("LevelSelect");

		SceneManager.activeSceneChanged += SceneActivated;
		SceneManager.sceneLoaded += SceneLoaded;
	}

	// Update is called once per frame
	void Update() {
	
	}

	public void NextLevel() {
		int level = currentLevel + 1;
		//Back to menu
		if (level == levelScenes.Length) {
			SceneManager.LoadScene("LevelSelect");
		}
		
		LoadLevel(level);
	}

	public void LoadLevel(int levelNum) {
		if (currentLevel == levelNum && (currentLevelScene != null))
			return;

		Debug.Log("Loading level #" + levelNum);

		currentLevel = levelNum;
		currentLevelScene = levelScenes[currentLevel].SceneName;
	
		//Load the scene they suggested
		SceneManager.LoadScene(currentLevelScene);
	}

	void SceneLoaded(Scene scene, LoadSceneMode mode) {
	}

	void SceneActivated(Scene oldScene, Scene newScene) {
		//Spawn player
		var newPlayer = GameObject.Instantiate(playerPrefab);
		newPlayer.GetComponent<Player>().Respawn();
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
}
