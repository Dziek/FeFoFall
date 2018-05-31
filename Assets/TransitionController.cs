using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionController : MonoBehaviour {
	
	// [HideInInspector]
	// public GameObject playerGO;
	// [HideInInspector]
	// public GameObject endPointGO;
	
	// [HideInInspector]
	// public Vector2 playerLastPos;
	// [HideInInspector]
	// public Vector2 endPointLastPos;
	
	[HideInInspector]
	public TransitionReason transitionReason;
	[HideInInspector]
	public bool gameCompleted;
	[HideInInspector]
	public bool gameReset; // this means it was completed, but is being reset as player is starting again
	
	[HideInInspector]
	public bool closeCall;
	
	public LevelManager levelManager;
	public StatsManager statsManager;
	public ModeManager modeManager;
	
	private TransitionScreen transitionScreen;
	private TransitionText transitionText;
	
	private bool transitioning;
	
	void Awake () {
		transitionScreen = GetComponent<TransitionScreen>();
		transitionText = GetComponent<TransitionText>();
	}
	
	void BeginTransition (TransitionReason tS) {
		// Debug.Log(tS);
		// Debug.Break();
		
		gameCompleted = false;
		
		if (transitioning == false)
		{
			transitionReason = tS;
			StartCoroutine("Transition");
		}
		
		Debug.Log("Starting T");
	}
	
	IEnumerator Transition () {
		
		transitioning = true;
		
		if (transitionReason == TransitionReason.levelSuccess)
		{
			// Tells LoadLevel a level has been completed, and returns whether that was the last level
			// gameCompleted = LoadLevel.LevelCompleted();
			levelManager.LevelCompleted();
			gameCompleted = levelManager.CheckForGameOver();
		}
		
		if (transitionReason == TransitionReason.levelLoad)
		{
			// Tells LoadLevel a level has been completed, and returns whether that was the last level
			// gameCompleted = LoadLevel.LevelCompleted();
			// levelManager.LevelCompleted();
			gameReset = levelManager.CheckForGameOver();
			
			if (gameReset == true)
			{
				// statsManager.ClearModeStats(modeManager.GetMode());	
				levelManager.ResetModeLevels();
			}
		}
		
		// if (transitionReason == TransitionReason.levelFailure)
		// {
			// LoadLevel.LevelFailed();
		// }
		
		// if (transitionReason == TransitionReason.levelSuccess)
		// {
			// // Reset Game if you start to play and levels are all complete
			// if (LoadLevel.CheckComplete())
			// {
				// LoadLevel.Reset();
				// // Debug.Log("R");
			// }
		// }
		
		// Debug.Log("About To Start Phase One");
		// Debug.Break();
		
		// HANGS AFTER HERE
		
		yield return transitionScreen.StartPhaseOne();
		
		// Debug.Log("Just After Phase One");
		// Debug.Break();
		
		transitionText.UpdateText();
		string text = transitionText.GetText();
		
		// string text = "Hello There";
		
		
		MinMax waitTime = new MinMax(0.25f, 1f);
		MinMax stringLength = new MinMax(8, 30);
		
		float lerpValue = Mathf.InverseLerp(stringLength.min, stringLength.max, text.Length);
		
		float actualWaitTime = Mathf.Lerp(waitTime.min, waitTime.max, lerpValue);
		
		// Debug.Log("Wait Time: " + actualWaitTime);
		
		// yield return transitionScreen.StartPhaseOne();
		
		// BUT BEFORE HERE
		
		// Debug.Log("About To Start Phase Two");
		// Debug.Break();
		
		yield return transitionScreen.StartPhaseTwo(actualWaitTime);
		
		// if (gameCompleted)
		// {
			// levelManager.ClearLevel();
		// }
		
		yield return transitionScreen.StartPhaseThree();
		
		if (gameCompleted == false)
		{
			GameStates.ChangeState("Playing");
		}else{
			GameStates.ChangeState("Complete");
		}
		
		Messenger.Broadcast("StopConstantShake");
		// Debug.Log("Transition Done");
		
		// Debug.Log("T Still running y'all");
		
		transitioning = false;
		
		yield return null;
	}
	
	void CloseCallUpdate (bool v) {
		closeCall = v;
	}
	
	void Cancel () {
		transitioning = false;
		StopCoroutine("Transition");
		transitionScreen.Cancel();
		Messenger.Broadcast("StopConstantShake");
		StopAllCoroutines();
	}
	
	void OnEnable () {
		Messenger<TransitionReason>.AddListener("Transition", BeginTransition);
		Messenger<bool>.AddListener("CloseCall", CloseCallUpdate);
		Messenger.AddListener("BackToMenu", Cancel);
	}
	
	void OnDisable () {
		Messenger<TransitionReason>.RemoveListener("Transition", BeginTransition);
		Messenger<bool>.RemoveListener("CloseCall", CloseCallUpdate);
		Messenger.RemoveListener("BackToMenu", Cancel);
	}
}

public enum TransitionReason {
	levelSuccess,
	levelFailure,
	levelLoad,
	levelTest
}

public struct MinMax {
	public float min;
	public float max;
	
	public MinMax (float mi, float ma) {
		min = mi;
		max = ma;
	}
}
