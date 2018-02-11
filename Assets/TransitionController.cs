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
	public TransitionStates transitionState;
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
	
	void BeginTransition (TransitionStates tS) {
		Debug.Log("Start Transition");
		StartCoroutine(Transition(tS));
	}
	
	// void BeginTransition (TransitionStates tS, Vector2 playerPos, Vector2 endPointPos) {
		// playerLastPos = playerPos;
		// endPointLastPos = endPointPos;
		
		// StartCoroutine(Transition(tS));
	// }
	
	IEnumerator Transition (TransitionStates tS) {
		
		Debug.Log("Transition");
		
		// Tells LoadLevel a level has been completed, and returns whether that was the last level
		// gameCompleted = LoadLevel.LevelCompleted();
		
		// yield return transitionScreen.StartPhaseOne();
		transitionText.UpdateText(tS);
		// yield return transitionScreen.StartPhaseTwo();
		// yield return transitionScreen.StartPhaseThree();
		
		// Debug.Log("Transition Done");
		
		yield return null;
	}
	
	// void FillPlayerGO (GameObject go) {
		// playerGO = go;
		
		// endPointGO = GameObject.Find("EndPoint");
	// }
	
	// void FillEndPointGO (GameObject go) {
		// endPointGO = go;
	// }
	
	void CloseCallUpdate (bool v) {
		closeCall = v;
	}
	
	void OnEnable () {
		// Messenger<TransitionStates, Vector2, Vector2>.AddListener("Transition", BeginTransition);
		Messenger<TransitionStates>.AddListener("Transition", BeginTransition);
		Messenger<bool>.AddListener("CloseCall", CloseCallUpdate);
		
		// Messenger<GameObject>.AddListener("FillPlayerGO", FillPlayerGO);
		// Messenger<GameObject>.AddListener("FillEndPointGO", FillEndPointGO);
	}
	
	void OnDisable () {
		// Messenger<TransitionStates, Vector2, Vector2>.RemoveListener("Transition", BeginTransition);
		Messenger<TransitionStates>.RemoveListener("Transition", BeginTransition);
		Messenger<bool>.RemoveListener("CloseCall", CloseCallUpdate);
		
		// Messenger<GameObject>.RemoveListener("FillPlayerGO", FillPlayerGO);
		// Messenger<GameObject>.RemoveListener("FillEndPointGO", FillEndPointGO);
	}
}

public enum TransitionStates {
	levelSuccess,
	levelFailure,
	levelLoad
}
