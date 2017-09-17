using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

[InitializeOnLoad]
public class AddLevelsToBuild {

	const string LevelsDir = "Assets/Scenes/Levels";

	static void AddLevels() {
		EditorBuildSettingsScene[] scenes = EditorBuildSettings.scenes;
		List<EditorBuildSettingsScene> newScenes = new List<EditorBuildSettingsScene>(scenes);

		FileInfo[] files = new DirectoryInfo(LevelsDir).GetFiles();

		foreach (FileInfo file in files) {
			if (file.Extension.Equals(".unity")) {
				string relPath = LevelsDir + "/" + file.Name;
				//See if we have it in the scenes
				bool foundIt = false;
				foreach (EditorBuildSettingsScene scene in newScenes) {
					if (scene.path.Equals(relPath)) {
						foundIt = true;
						break;
					}
				}
				if (!foundIt) {
					Debug.Log("Adding scene to build settings: " + relPath);
					newScenes.Add(new EditorBuildSettingsScene(relPath, true));
				}
			}
		}

		EditorBuildSettings.scenes = newScenes.ToArray();
	}

	//Called whenever you open something. You open scenes when you create them. So this should get those events.
	[UnityEditor.Callbacks.OnOpenAsset]
	static bool OnOpenAsset(int instanceId, int line) {
		AddLevels();
		return false;
	}

	//Gets called every time scripts are reloaded in the editor, will catch anything else
	[UnityEditor.Callbacks.DidReloadScripts]
	static void OnDidReloadScripts() {
		AddLevels();
	}
}