using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndScreenController : MonoBehaviour {
	
	public GameObject mainCompleteScreen;
	
	public GameObject timeCompleteScreen;
	public GameObject timeFailedScreen;
	
	public GameObject livesCompleteScreen;
	public GameObject livesFailedScreen;
	
	public GameObject oneShotCompleteScreen;
	public GameObject oneShotFailedScreen;
	
	private ModeManager modeManager;
	
	void Awake () {
		modeManager = GameObject.Find("ModeManager").GetComponent<ModeManager>();
		
		HideAll();
	}
	
	void OnEnable () {
		Messenger.AddListener("BackToMenu", HideAll);
	}
	
	void OnDisable () {
		Messenger.RemoveListener("BackToMenu", HideAll);
	}
	
	public void Completed () {
		switch (modeManager.GetMode())
		{
			default:
				mainCompleteScreen.SetActive(true);
			break;
			
			case Mode.Time:
				timeCompleteScreen.SetActive(true);
			break;
			
			case Mode.Lives:
				livesCompleteScreen.SetActive(true);
			break;
			
			case Mode.OneShot:
				oneShotCompleteScreen.SetActive(true);
			break;
		}
	}
	
	public void GameOver () {
		switch (modeManager.GetMode())
		{
			default:
				Debug.Log("This Should Be Impossible - Main Game Over Screen");
			break;
			
			case Mode.Time:
				timeFailedScreen.SetActive(true);
			break;
			
			case Mode.Lives:
				livesFailedScreen.SetActive(true);
			break;
			
			case Mode.OneShot:
				oneShotFailedScreen.SetActive(true);
			break;
		}
	}
	
	public void HideAll () {
		mainCompleteScreen.SetActive(false);
		timeCompleteScreen.SetActive(false);
		timeFailedScreen.SetActive(false);
		livesCompleteScreen.SetActive(false);
		livesFailedScreen.SetActive(false);
		oneShotCompleteScreen.SetActive(false);
		oneShotFailedScreen.SetActive(false);
	}
}
