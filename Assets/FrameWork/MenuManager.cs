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
				
				Camera.main.backgroundColor = cameraColor;
				EventSystem.current.SetSelectedGameObject(mainMenuButton);
				
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
					transitionScript.StartCoroutine("Go", extra);
				}else{
					transitionScript.StartCoroutine("Go", "Load");
				}
				
				mainMenu.SetActive(false);
				
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
}
