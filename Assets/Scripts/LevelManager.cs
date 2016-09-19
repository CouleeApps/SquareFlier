using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class LevelManager : MonoBehaviour {

	public GameObject player;
	public SceneField[] levelScenes;
	public int currentLevel;

	public string managerSceneName = "ManagerScene";

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
		SceneManager.LoadScene(currentLevelScene, LoadSceneMode.Single);

		SceneManager.activeSceneChanged += SceneActivated;
		SceneManager.sceneLoaded += SceneLoaded;
	}

	void SceneLoaded(Scene scene, LoadSceneMode mode) {
	}

	void SceneActivated(Scene oldScene, Scene newScene) {
		if (state == LoadState.LoadingLevel) {
			//Find any level start areas
			List<GameObject> startAreas = new List<GameObject>();
			foreach (GameObject obj in newScene.GetRootGameObjects()) {
				if (obj.GetComponent<Level>()) {
					startAreas.Add(obj);
				}
			}
			//And pick a spawnpoint
			int spawnArea = Random.Range(0, startAreas.Count - 1);
			startAreas[spawnArea].GetComponent<Level>().StartCoroutine("SpawnPlayer", player);

			state = LoadState.LoadingFinish;
		}
	}
}
