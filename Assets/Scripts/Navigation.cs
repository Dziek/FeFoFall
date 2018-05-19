using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.EventSystems;

public class Navigation : MonoBehaviour {
	
	private EventSystem mainEventSystem;
	
	void Awake () {
		// UI.orientation = UIOrientation.AutoRotation;
		// UIOrientation = UIOrientation.AutoRotation;
		
		mainEventSystem = EventSystem.current;
	}
	
	// Update is called once per frame
	void Update () {
		
		if (mainEventSystem.enabled == true)
		{
			if (Input.GetButtonDown("Cancel"))
			{
				if (GameStates.GetState() != "MainMenu")
				{
					Messenger.Broadcast("BackToMenu");
					// LoadLevel.ClearLevel();
					GameStates.ChangeState("MainMenu");
					
					// Debug.Break();
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
}
