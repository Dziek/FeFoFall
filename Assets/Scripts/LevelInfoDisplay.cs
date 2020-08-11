using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LevelInfoDisplay : MonoBehaviour {
	
	public Text lvlName;
	public Text lvlDesc;
	
	public Text lvlID;
	public Text lvlAttempts;
	public Text lvlBestTime;
	
	private ModeManager modeManager;
	
	void Awake () {
		modeManager = GameObject.Find("ModeManager").GetComponent<ModeManager>();
	}
	
	void OnEnable () {
		// Messenger<GameObject>.AddListener("NewLevel", UpdateDisplay);
		
		UpdateDisplay(GameObject.FindWithTag("Level"));
		// Debug.Log("Updating Display From OnEnable");
	}
	
	void OnDisable () {
		// Messenger<GameObject>.RemoveListener("NewLevel", UpdateDisplay);
		// Debug.Log("Turning Off");
	}
	
	void Update () {
		
		// this is for level skipping in Tricky mode. There may well be a better place for this code, but it lives here for now
		
		if (Input.GetButtonDown("NextLevel") && modeManager.GetMode() == Mode.Tricky)
		{
			// Messenger.Broadcast("Success");
			GameStates.ChangeState("Transition", "Skip");
			Messenger<TransitionReason>.Broadcast("Transition", TransitionReason.levelSkip);
			
			gameObject.SetActive(false);
		}
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
		
		// Debug.Log(levelGO);
		
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
