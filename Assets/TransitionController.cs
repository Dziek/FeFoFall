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
	public TransitionState transitionState;
	[HideInInspector]
	public bool gameCompleted;
	
	[HideInInspector]
	public bool closeCall;
	
	private TransitionScreen transitionScreen;
	private TransitionText transitionText;
	
	void Awake () {
		transitionScreen = GetComponent<TransitionScreen>();
		transitionText = GetComponent<TransitionText>();
	}
	
	void BeginTransition (TransitionState tS) {
		
		transitionState = tS;
		StartCoroutine("Transition");
	}
	
	IEnumerator Transition () {
		
		if (transitionState == TransitionState.levelSuccess)
		{
			// Tells LoadLevel a level has been completed, and returns whether that was the last level
			gameCompleted = LoadLevel.LevelCompleted();
		}
		
		if (transitionState == TransitionState.levelSuccess)
		{
			// Reset Game if you start to play and levels are all complete
			if (LoadLevel.CheckComplete())
			{
				LoadLevel.Reset();
				// Debug.Log("R");
			}
		}
		
		transitionText.UpdateText();
		
		
		string text = transitionText.GetText();
		
		MinMax waitTime = new MinMax(0.25f, 0.45f);
		MinMax stringLength = new MinMax(8, 20);
		
		float lerpValue = Mathf.InverseLerp(stringLength.min, stringLength.max, text.Length);
		
		float actualWaitTime = Mathf.Lerp(waitTime.min, waitTime.max, lerpValue);
		
		Debug.Log("Wait Time: " + actualWaitTime);
		
		yield return transitionScreen.StartPhaseOne();
		yield return transitionScreen.StartPhaseTwo(actualWaitTime);
		yield return transitionScreen.StartPhaseThree();
		
		if (gameCompleted == false)
		{
			GameStates.ChangeState("Playing");
		}else{
			GameStates.ChangeState("Complete");
		}
		
		Messenger.Broadcast("StopConstantShake");
		// Debug.Log("Transition Done");
		
		yield return null;
	}
	
	void CloseCallUpdate (bool v) {
		closeCall = v;
	}
	
	void OnEnable () {
		Messenger<TransitionState>.AddListener("Transition", BeginTransition);
		Messenger<bool>.AddListener("CloseCall", CloseCallUpdate);
	}
	
	void OnDisable () {
		Messenger<TransitionState>.RemoveListener("Transition", BeginTransition);
		Messenger<bool>.RemoveListener("CloseCall", CloseCallUpdate);
	}
}

public enum TransitionState {
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
