using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Appear : MonoBehaviour {
	
	public bool startActive = false; // whether the object starts active
	
	// public float movementTime = 1; // time it takes to complete a one way journey
	public float pauseTime = 2; // time to pause when reaching either point
	
	[Tooltip("'Trip' is defined as going from one point to the next - 0 means unlimited")] 
	public int noOfTrips; // how many trips to complete before stopping
	
	public bool waitForPlayerMovement; // wait for player movement before beginning object movement
	public bool skipFirstPause;
	
	public bool reverseAtEnd; // only used when multiple points - then it's used to see if object should keep lapping or reverse direction
	
	public bool ignoreTrigger; // ignores waitForTrigger
	
	private bool waitForTrigger; // used for trigger stuff
	
	private bool active;
	public PlayerControl playerScript; // used for waitForPlayerMovement
	
	private Collider2D collider;
	private GameObject graphicsGO;
	
	void Awake () {
		//List protection here
		// if (travelPointss.Count == 0)
		// {
			// travelPointss.Add(Vector3.zero);
		// }
	}
	
	// Use this for initialization
	void Start () {
		
		if (GetComponent<Collider2D>() != null)
		{
			collider = GetComponent<Collider2D>();
		}
		
		// sometimes this won't work, will deal with it when and if that happens
		graphicsGO = transform.GetChild(0).gameObject;
		active = startActive;
		UpdateObject();
		
		if (ignoreTrigger)
		{
			waitForTrigger = false;
		}
	
		// if (!waitForTrigger)
		// {
			// if (waitForPlayerMovement)
			// {
				// StartCoroutine("WaitForGameStart");
			// }else{
				// StartCoroutine("Move");
			// }
		// }
		
		Invoke("DelayedStart", 0.1f);
		
	}
	
	void DelayedStart () {
		// UpdateObject();
		
		if (!waitForTrigger)
		{
			if (waitForPlayerMovement)
			{
				StartCoroutine("WaitForGameStart");
			}else{
				StartCoroutine("Move");
			}
		}
	}
	
	IEnumerator Move () {
		
		// Debug.Log("Move " + waitForPlayerMovement);
		
		// float t = 0; // local timer
		int currentTripNo = 1; // which number trip the object is on
		
		if (skipFirstPause == false)
		{
			yield return new WaitForSeconds(pauseTime);
		}
		
		while (true)
		{	
			
			active = !active;
			UpdateObject();
			
			currentTripNo++;
			if (currentTripNo >= noOfTrips+1 && noOfTrips != 0) // if reached noOfTrips limit, break out of loop
			{
				yield break;
			}
			
			yield return new WaitForSeconds(pauseTime);
			
			// t += Time.deltaTime;	
			yield return null;
		}
	}
	
	
	public void WaitForTrigger () {
		waitForTrigger = true;
	}
	
	public void TriggerActivated (float time) {
		if (waitForTrigger)
		{
			StartCoroutine("Move");
			// Debug.Log("TriggerActivated");
			if (time != 0)
			{
				StartCoroutine("Timer", time);
			}
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
		StopCoroutine("Move");
		// Debug.Log("Timer");
	}
	
	IEnumerator WaitForGameStart () {
		while (true)
		{
			// if (GameStates.GetState() == "Playing")
			{
				playerScript = GameObject.Find("Player").GetComponent<PlayerControl>();
				// Debug.Break();
				StartCoroutine("CheckForPlayerMovement");
				yield break;
			}
			// yield return null;
		}
	}
	
	IEnumerator CheckForPlayerMovement () {
		while (true)
		{
			if (playerScript.GetDirection() != "None")
			{
				// Debug.Log("CheckForPlayerMovement: " + playerScript.GetDirection()); 
				StartCoroutine("Move");
				yield break;
			}
			yield return null;
		}
	}
	
	void UpdateObject () {
		if (GetComponent<Collider2D>() != null)
		{
			collider.enabled = active;
		}
		
		graphicsGO.SetActive(active);
		// Debug.Log("Update! " + active);
	}
}
