using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelect : MonoBehaviour {

	public GameObject buttonPrefab;
    public GameObject uiTimerToggle;
	RectTransform t;

	// Use this for initialization
	void Start() {
		LevelManager manager = LevelManager.CurrentManager;
		t = GetComponent<RectTransform>();

		for (int i = 0; i < manager.levelNames.Length; i++) {
			string name = manager.levelNames[i];

			GameObject levelButton = Instantiate(buttonPrefab);
			levelButton.transform.SetParent(t);
			int levelNum = i;

			RectTransform buttonTrans = levelButton.GetComponent<RectTransform>();
			levelButton.GetComponent<Button>().onClick.AddListener(() => {
				manager.LoadLevel(levelNum);
			});
			levelButton.GetComponentInChildren<Text>().text = name;
			buttonTrans.anchoredPosition = new Vector3(0, -20 - (i * (buttonTrans.rect.height + 10)), 0);
		}

        uiTimerToggle.GetComponent<Toggle>().onValueChanged.AddListener((bool newState) =>
        {
            PreferenceManager.CurrentManager.ShowTimer = newState;
        });
	}
	
	// Update is called once per frame
	void Update() {
		
	}
}
