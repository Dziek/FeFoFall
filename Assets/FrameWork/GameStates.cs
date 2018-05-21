using UnityEngine;
using System.Collections;

public class GameStates : MonoBehaviour {

	static MenuManager menuScript;
	
	public enum states{
		MainMenu,
		Playing,
		Transition,
		Complete,
		GameOver,
		
		TestTransition
	}public static states state;
	
	void Awake () {
		menuScript = transform.GetComponent<MenuManager>();
		
		// QualitySettings.vSyncCount = 0;
	}
	
	void Start () {
		ChangeState("MainMenu");
		
		if (Application.loadedLevelName == "LevelTesting" || Application.loadedLevelName == "GraphicsTesting"
			|| Application.loadedLevelName == "CongratsTesting")
		{
			ChangeState("Playing");
		}
		
		// Application.targetFrameRate = 60;
	}

	public void ChangeStateOnClick (string newState) {
		state = (states)System.Enum.Parse(typeof(states), newState);
		if (menuScript != null)
		{
			menuScript.LoadMenu(newState, null);
		}
	}
	
	public static void ChangeState (string newState) {
		state = (states)System.Enum.Parse(typeof(states), newState);
		if (menuScript != null)
		{
			menuScript.LoadMenu(newState, null);
		}
	}
	
	public static void ChangeState (string newState, string extra) {
		state = (states)System.Enum.Parse(typeof(states), newState);
		if (menuScript != null)
		{
		}
	}
	
	public static string GetState () {
		return state.ToString();
	}
}

