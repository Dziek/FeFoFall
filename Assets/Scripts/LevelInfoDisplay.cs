using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LevelInfoDisplay : MonoBehaviour {
	
	public Text lvlName;
	public Text lvlDesc;
	
	public Text lvlID;
	public Text lvlAttempts;
	public Text lvlBestTime;
	
	// public string completeDesc;
	
	// private bool canRid = true; // whether can move buttons to get rid of screen
	
	void OnEnable () {
		
		// Messenger<GameObject>.AddListener("NewLevelLoaded", UpdateLevel);
		// Messenger.AddListener("LevelCanvasPresent", GetRidOfText);
		
		GameObject levelObject = GameObject.FindWithTag("Level");
		
		if (levelObject != null)
		{
			// lvlDesc.text = levelObject.GetComponent<LevelInfo>().GetDesc();
			string lN, lD;
			levelObject.GetComponent<LevelInfo>().GetInfo(out lN, out lD);
			
			lvlName.text = lN;
			lvlDesc.text = lD;
			
			if (Application.loadedLevelName != "LevelTesting")
			{
				lvlID.text = levelObject.name;
				lvlAttempts.text = "Attempts: " + LoadLevel.GetCurrentLevelCurrentAttempts().ToString();
				lvlBestTime.text = "Level Best Time: " + LoadLevel.GetCurrentLevelBestTime().ToString("f2");
			}
		}
		
		if (levelObject.GetComponentInChildren<Canvas>() != null)
		{
			GetRidOfText();
		}
		
		// GameObject.Find("Player").GetComponent<PlayerControl>().UpdateLevelInfoDisplayObject(gameObject);
		
		//At Some Point change this to send this script, not this gameObject
		// Debug.Log("Level Display " + levelObject);
		levelObject.GetComponentInChildren<PlayerControl>().UpdateLevelInfoDisplayObject(this);
		
		// if (lvlDesc.text == null)
		// {
			// lvlDesc.text = completeDesc;
			// canRid = false;
		// }
	}
	
	// void OnDisable () {
		// Messenger.RemoveListener("LevelCanvasPresent", GetRidOfText);
		// Messenger<GameObject>.RemoveListener("NewLevelLoaded", UpdateLevel);
	// }
	
	void UpdateLevel (GameObject levelGO) {
		GameObject levelObject = levelGO;
		
		if (levelObject != null)
		{
			// lvlDesc.text = levelObject.GetComponent<LevelInfo>().GetDesc();
			string lN, lD;
			levelObject.GetComponent<LevelInfo>().GetInfo(out lN, out lD);
			
			lvlName.text = lN;
			lvlDesc.text = lD;
			
			if (Application.loadedLevelName != "LevelTesting")
			{
				lvlID.text = levelObject.name;
				lvlAttempts.text = "Attempts: " + LoadLevel.GetCurrentLevelCurrentAttempts().ToString();
				lvlBestTime.text = "Level Best Time: " + LoadLevel.GetCurrentLevelBestTime().ToString("f2");
			}
		}
		
		// GameObject.Find("Player").GetComponent<PlayerControl>().UpdateLevelInfoDisplayObject(gameObject);
		
		//At Some Point change this to send this script, not this gameObject
		Debug.Log("Level Display " + levelObject);
		levelObject.GetComponentInChildren<PlayerControl>().UpdateLevelInfoDisplayObject(this);
	}
	
	void GetRidOfText () {
		// lvlName.enabled = false;
		// lvlDesc.enabled = false;
			
		// lvlID.enabled = false;
		// lvlAttempts.enabled = false;
		// lvlBestTime.enabled = false;
		
		lvlName.text = "";
		lvlDesc.text = "";
			
		lvlID.text = "";
		lvlAttempts.text = "";
		lvlBestTime.text = "";
	}
	
	public void GoAway () {
		if (Application.loadedLevelName != "LevelTesting")
		{
			LoadLevel.AddToCurrentAttempts();
		}
		gameObject.SetActive(false);
	}
}
