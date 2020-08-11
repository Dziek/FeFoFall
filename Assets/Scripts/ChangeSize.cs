using UnityEngine;
using System.Collections;

public class ChangeSize : MonoBehaviour {
	
	// private float startZ;
	private	Vector3 startSize;
	public Vector3 endSize;
	
	public float speed;
	public float pause; // time to pause when reaching either point
	public float delay;
	
	public bool waitForPlayerMovement;
	public bool oneWay; // if want to use this pause must be at least 0.1f
	public bool oneCircuit; // if want to use this pause must be at least 0.1f
	public bool skipFirstPause;
	public bool waitForTransition; // wait for Transition to have finished
	
	private float timer;
	private float oneWayCutOff;
	private float oneCircuitCutOff;
	private float skipPauseCutOff;
	private bool waitForTrigger;
	private bool partOneCompleteForOneCircuit;
	private bool partOneCompleteForSkipPause;
	
	private int waitForThisNumberOfTriggers;
	private int noOfTriggersTripped;
	
	// private GameObject player;
	private PlayerControl playerScript;
	
	void OnEnable () {
		Messenger<PlayerControl>.AddListener("SetPlayer", UpdatePlayerControl);
	}
	
	void OnDisable () {
		Messenger<PlayerControl>.RemoveListener("SetPlayer", UpdatePlayerControl);
	}
	
	void UpdatePlayerControl (PlayerControl pC) {
		playerScript = pC;
	}
	
	void Awake () {
		endSize = new Vector3(endSize.x, endSize.y, 1);
	}
	
	// Use this for initialization
	void Start () {
		startSize = transform.localScale;
		// startZ = transform.position.z;
		// player = GameObject.Find("Player");
		// playerScript = GameObject.Find("Player").GetComponent<PlayerControl>();
		
		oneWayCutOff = 1 + (pause/2);
		oneCircuitCutOff = 0 - (pause/2);
		skipPauseCutOff = 1 + (pause*0.99f);
		
		if (!waitForTrigger)
		{
			if (waitForPlayerMovement == true || waitForTransition == true)
			{
				StartCoroutine("WaitForGameStart");
			}else{
				StartCoroutine("Change");
				// Debug.Log("Hmm");
			}
		}
		
	}
	
	IEnumerator Change () {
		// while (GameStates.GetState() == "Playing")
		
		// Debug.Log(Time.time);
		// Debug.Log("Changing");
	
		if (delay > 0)
		{
			yield return new WaitForSeconds(delay);
		}
		
		// Debug.Log(Time.time);
		
		float pos = 0;
		
		while (true)
		{
			if (GameStates.GetState() == "Playing")
			{
				timer += Time.deltaTime;
				pos = 0;
				
				if (!skipFirstPause)
				{
					pos = Mathf.PingPong(timer * speed, 1 + (pause*2)) - pause;
				}else{
					if (partOneCompleteForSkipPause)
					{
						pos = Mathf.PingPong(timer * speed, 1 + (pause*2)) - pause;
					}else{
						pos = Mathf.PingPong(timer * speed, 1 + (pause*2));
					}
				}
				
				if (pos > skipPauseCutOff)
				{
					if (skipFirstPause)
					{
						partOneCompleteForSkipPause = true;
					}
				}
				
				// if (pos > 0.95f && oneWay)
				if (pos > oneWayCutOff)
				{
					if (oneWay)
					{
						transform.localScale = endSize;
						break;
					}
					if (oneCircuit)
					{
						partOneCompleteForOneCircuit = true;
					}
				}
				
				if (pos < oneCircuitCutOff)
				{
					if (oneCircuit && partOneCompleteForOneCircuit)
					{
						transform.localScale = startSize;
						break;
					}
				}
				
				// if (pos > 0.95f && oneWay)
				// if (pos > oneWayCutOff && oneWay)
				// {
					// yield break;
				// }
				
				// transform.localScale = Vector3.Lerp(startSize, startSize + endSize, pos);
				transform.localScale = Vector3.Lerp(startSize, endSize, pos);
			}
			
			yield return null;
		}
		
		// transform.localScale = endSize;
		// Debug.Log(pos);
		yield return null;
	}
	
	public void WaitForTrigger () {
		waitForTrigger = true;
		
		// Debug.Log("Wha?");
	}
	
	public void WaitForNumberTrigger () {
		waitForTrigger = true;
		
		waitForThisNumberOfTriggers++;
		
		// Debug.Log(waitForThisNumberOfTriggers);
	}
	
	public void TriggerActivated (float time) {
		
		noOfTriggersTripped++;
		
		if (noOfTriggersTripped < waitForThisNumberOfTriggers)
		{
			return;
		}
		
		// timer = 0;
		StartCoroutine("Change");
		if (time != 0)
		{
			StartCoroutine("Timer", time);
		}
	}
	
	IEnumerator Timer (float t) {
		
		float timer = 0;
		
		while (timer < t)
		{
			timer += Time.deltaTime;
			yield return null;
		}
		
		yield return null;
		StopCoroutine("Change");
	}
	
	IEnumerator WaitForGameStart () {
		while (true)
		{
			if (GameStates.GetState() == "Playing")
			{
				if (Application.loadedLevelName == "LevelTesting" || Application.loadedLevelName == "GraphicsTesting")
				{
					// Debug.Log("Looking!");
					playerScript = GameObject.Find("Player").GetComponent<PlayerControl>();
				}
				
				if (waitForTransition)
				{
					StartCoroutine("Change");
				}
				
				if (waitForPlayerMovement)
				{
					StartCoroutine("CheckForPlayerMovement");
				}
				
				yield break;
			}
			yield return null;
		}
		
		yield return null;
	}
	
	IEnumerator CheckForPlayerMovement () {
		while (true)
		{
			if (playerScript.GetDirection() != "None")
			{
				StartCoroutine("Change");
				yield break;
			}
			yield return null;
		}
		
		yield return null;
	}
	
}
