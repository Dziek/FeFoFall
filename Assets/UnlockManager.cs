using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnlockManager : MonoBehaviour {
	
	// public GameObject playMenuGO;
	private StatsManager statsManager;
	// public PlayMenuController playMenuController;
	public MainMenuController mainMenuController;
	
	void Awake () {
		statsManager = GameObject.Find("StatsManager").GetComponent<StatsManager>();
	}
	
	void OnEnable () {
		Messenger.AddListener("BackToMenu", CheckForUnlocks);
	}
	
	void OnDisable () {
		Messenger.RemoveListener("BackToMenu", CheckForUnlocks);
	}
	
	void CheckForUnlocks () {
		// if (StatsManager.
		// if Main best score < 100 then display tricky button
	}
	
	
	// checks if (and then does) load main game if nothing is unlocked - or load the "PlayWhat?" menu
	public void PressPlay () {
		if (statsManager.GetModeTimesCompleted(Mode.Main) > 0)
		{
			// load menu
			// playMenuGO.SetActive(true);
			mainMenuController.OpenPlaySub();
			
		}else{
			// just Play Main
			GameStates.ChangeState("Transition", null);
		}
	}
}
