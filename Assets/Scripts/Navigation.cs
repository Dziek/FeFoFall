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
		
		if (Input.GetButtonDown("Cancel"))
		{
			if (GameStates.GetState() != "MainMenu")
			{
				LoadLevel.ClearLevel();
				GameStates.ChangeState("MainMenu");
			}else{
				
				GameObject buttonGO = GameObject.Find("Back");
				
				if (buttonGO != null)
				{
					Button backButton = buttonGO.GetComponent<Button>();
					backButton.onClick.Invoke();
				}
			}
		}
	}
}
