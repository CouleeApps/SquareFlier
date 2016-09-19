using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class LevelManager : NetworkManager {

	public GameObject player;
	public SceneField[] levelScenes;
	public int currentLevel;

	private string currentLevelScene;
	private enum LoadState {
		LoadingManager,
		LoadingLevel,
		LoadingFinish
	};
	private LoadState state;

	void Start() {
		DontDestroyOnLoad(gameObject);
	}

	// Update is called once per frame
	void Update() {
	
	}

	public void NextLevel() {
		int level = currentLevel + 1;
		//Just wrap for now
		if (level == levelScenes.Length)
			level = 0;
		
		LoadLevel(level);
	}

	public void LoadLevel(int levelNum) {
		if (currentLevel == levelNum && (currentLevelScene != null))
			return;

		Debug.Log("Loading level #" + levelNum);

		state = LoadState.LoadingLevel;
		currentLevel = levelNum;
		currentLevelScene = levelScenes[currentLevel].SceneName;
	
		//Load the scene they suggested
		ServerChangeScene(currentLevelScene);

		SceneManager.activeSceneChanged += SceneActivated;
		SceneManager.sceneLoaded += SceneLoaded;
	}

	void SceneLoaded(Scene scene, LoadSceneMode mode) {
	}

	void SceneActivated(Scene oldScene, Scene newScene) {
	}
}
