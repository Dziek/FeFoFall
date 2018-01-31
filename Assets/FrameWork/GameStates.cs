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
		
		// Application.targetFrameRate = 60;
	}

	public void ChangeStateOnClick (string newState) {
		state = (states)System.Enum.Parse(typeof(states), newState);
		menuScript.LoadMenu(newState, null);
	}
	
	public static void ChangeState (string newState) {
		state = (states)System.Enum.Parse(typeof(states), newState);
		menuScript.LoadMenu(newState, null);
	}
	
	public static void ChangeState (string newState, string extra) {
		state = (states)System.Enum.Parse(typeof(states), newState);
		menuScript.LoadMenu(newState, extra);
	}
	
	public static string GetState () {
		return state.ToString();
	}
}

