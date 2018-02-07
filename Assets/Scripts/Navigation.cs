using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Navigation : MonoBehaviour {
	
	void Awake () {
		// UI.orientation = UIOrientation.AutoRotation;
		// UIOrientation = UIOrientation.AutoRotation;
	}
	
	// Update is called once per frame
	void Update () {
		// if (Input.GetKeyDown("escape"))
		if (Input.GetButtonDown("Cancel"))
		{
			// if (GameStates.GetState() == "MainMenu")
			// {
				// PlayerPrefs.DeleteAll();
				// LoadLevel.Reset();
				// // GameStates.ChangeState("MainMenu");
			// }
			
			LoadLevel.ClearLevel();
			GameStates.ChangeState("MainMenu");
		}
	}
}
