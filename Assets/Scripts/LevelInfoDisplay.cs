using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LevelInfoDisplay : MonoBehaviour {
	
	public Text lvlName;
	public Text lvlDesc;
	
	public Text lvlID;
	public Text lvlAttempts;
	public Text lvlBestTime;
	
	void OnEnable () {
		// Messenger<GameObject>.AddListener("NewLevel", UpdateDisplay);
		
		UpdateDisplay(GameObject.FindWithTag("Level"));
	}
	
	void OnDisable () {
		// Messenger<GameObject>.RemoveListener("NewLevel", UpdateDisplay);
		// Debug.Log("Turning Off");
	}
	
	void UpdateDisplay (GameObject levelGO) {
		
		// Debug.Log(levelGO);
		
		if (levelGO != null)
		{
			LevelInfo levelInfo = levelGO.GetComponent<LevelInfo>();
			
			lvlName.text = levelInfo.name;
			lvlDesc.text = levelInfo.description;
			
			if (Application.loadedLevelName != "LevelTesting")
			{
				lvlID.text = levelGO.name;
				// lvlAttempts.text = "Attempts: " + LoadLevel.GetCurrentLevelCurrentAttempts().ToString();
				// lvlBestTime.text = "Level Best Time: " + LoadLevel.GetCurrentLevelBestTime().ToString("f2");
			}
		}
		
		if (levelGO.GetComponentInChildren<Canvas>() != null)
		{
			ClearText();
		}

		levelGO.GetComponentInChildren<PlayerControl>().UpdateLevelInfoDisplayObject(this);
	}
	
	void ClearText () {	
		lvlName.text = "";
		lvlDesc.text = "";
			
		lvlID.text = "";
		lvlAttempts.text = "";
		lvlBestTime.text = "";
	}
	
	public void GoAway () {
		if (Application.loadedLevelName != "LevelTesting")
		{
			// LoadLevel.AddToCurrentAttempts();
		}
		gameObject.SetActive(false);
	}
}
