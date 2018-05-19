using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class MenuManager : MonoBehaviour {
	
	public GameObject mainMenu;
	public GameObject levelInfo;
	// public GameObject playing;
	public GameObject transition;
	public GameObject gameOver;
	public GameObject complete;
	
	public Color cameraColor;
	
	public GameObject transitionPortrait;
	
	#if UNITY_ANDROID || UNITY_EDITOR
		public GameObject touchControls;
		public GameObject changeControls;
	#endif
	
	// public GameObject gameItems;
	
	public GameObject mainMenuButton;
	public GameObject gameOverButton;
	
	public Transition transitionScript;
	
	public void LoadMenu (string state, string extra) {
		switch (state)
		{
			case "MainMenu":
				mainMenu.SetActive(true);
				// gameItems.SetActive(false);
				#if UNITY_ANDROID
					changeControls.SetActive(true);
					touchControls.SetActive(true);
				#endif
				
				// Camera.main.backgroundColor = cameraColor;
				// EventSystem.current.SetSelectedGameObject(mainMenuButton);
				
				// invoking because updating statsDisplay (which happens OnSelect) needs to be delayed until all stats are loaded in
				// Invoke("SelectPlayButton", 0.0f);
				
				// playing.SetActive(false);
				levelInfo.SetActive(false);
				transition.SetActive(false);
				complete.SetActive(false);
			break;
			
			case "Playing":
				
				EventSystem.current.SetSelectedGameObject(null);
				
				mainMenu.SetActive(false);
				// levelInfo.SetActive(false);
				transition.SetActive(false);
				complete.SetActive(false);
			break;
			
			case "Transition":
				transition.SetActive(true);
				
				// Debug.Log("MM");
				
				if (extra != null)
				{
					// transitionScript.StopCoroutine("Go");
					// transitionScript.StartCoroutine("Go", extra);
					// Messenger<TransitionReason>
				}else{
					// transitionScript.StartCoroutine("Go", "Load");
					Messenger<TransitionReason>.Broadcast("Transition", TransitionReason.levelLoad);
				}
				
				// mainMenu.SetActive(false);
				
				#if UNITY_ANDROID
					changeControls.SetActive(false);
				#endif
			break;
			
			case "Complete":
				// complete.SetActive(true);
				
				// Score.SaveScore();
				
				// LoadLevel.ClearLevel();
				// LoadLevel.GetLevel();
				LoadLevel.Completed();
				
				EventSystem.current.SetSelectedGameObject(gameOverButton);
				
				transition.SetActive(false);
			break;
			
			case "TestTransition":
				transition.SetActive(true);
				
				// transitionScript.Go(extra);
				if (extra != null)
				{
					transitionScript.StartCoroutine("Go", extra);
					// transitionScript.StartCoroutine("GoAlt", extra);
				}else{
					transitionScript.StartCoroutine("Go", "Test");
					// transitionScript.StartCoroutine("GoAlt", "Load");
				}
				
				mainMenu.SetActive(false);
				
				#if UNITY_ANDROID
					changeControls.SetActive(false);
				#endif
			break;
		}
		
	}
	
	void SelectPlayButton () {
		EventSystem.current.SetSelectedGameObject(mainMenuButton);
	}
	
	void DisableMenu () {
		mainMenu.SetActive(false);
	}
	
	void OnEnable () {
		Messenger.AddListener("TransitionMiddle", DisableMenu);
		// Messenger.AddListener("TransitionStart", DisableMenu);
		// Messenger.AddListener("Transition", DisableMenu);
	}
	
	void OnDisable () {
		Messenger.RemoveListener("TransitionMiddle", DisableMenu);
		// Messenger.RemoveListener("TransitionStart", DisableMenu);
		// Messenger.RemoveListener("Transition", DisableMenu);
	}
}
