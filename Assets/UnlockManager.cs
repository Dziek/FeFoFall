using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnlockManager : MonoBehaviour {
	
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
}
