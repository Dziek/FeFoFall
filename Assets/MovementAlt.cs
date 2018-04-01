using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MovementAlt : MonoBehaviour {
	
	private	Vector3 startPoint; // refernce point for local to world calculations
	[Tooltip("Local")] 
	public List<Vector3> travelPointss = new List<Vector3>();
	
	public float movementTime = 1; // time it takes to complete a one way journey
	public float pauseTime; // time to pause when reaching either point
	
	[Tooltip("'Trip' is defined as going from one point to the next - 0 means unlimited")] 
	public int noOfTrips; // how many trips to complete before stopping
	
	public bool waitForPlayerMovement; // wait for player movement before beginning object movement
	public bool skipFirstPause;
	
	public bool reverseAtEnd; // only used when multiple points - then it's used to see if object should keep lapping or reverse direction
	
	public bool ignoreTrigger; // ignores waitForTrigger
	
	private bool waitForTrigger; // used for trigger stuff
	
	private PlayerControl playerScript; // used for waitForPlayerMovement
	
	void Awake () {
		//List protection here
		if (travelPointss.Count == 0)
		{
			travelPointss.Add(Vector3.zero);
		}
	}
	
	// Use this for initialization
	void Start () {
		
		if (ignoreTrigger)
		{
			waitForTrigger = false;
		}
	
		startPoint = transform.position;
		
		travelPointss.Insert(0, Vector3.zero); // adds starting point (local) to beginning of List
		
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
		
		float t = 0; // local timer
		int currentTripNo = 1; // which number trip the object is on
		int currentJourneyNo = 1; // which number journey the object is on - journey is defined as reaching all travelPoints
		int currentListPos = 1; // progress through travelPoints List - starts at 1 because startPoint is added at 0
		
		int listDirection = 1; // positive / negative way to go through List
		
		Vector3 startLerpPoint = startPoint;
		Vector3 worldNextPoint = startPoint + travelPointss[currentListPos]; // turns current travelPoints into world co-ordinates
		
		if (skipFirstPause == false)
		{
			yield return new WaitForSeconds(pauseTime);
		}
		
		while (true)
		{	
			if (t >= movementTime) // a trip has been completed
			{
				startLerpPoint = startPoint + travelPointss[currentListPos]; // save current point as start
				t -= movementTime; // reset movement
				
				currentListPos += listDirection;
				
				if (currentTripNo >= (travelPointss.Count-1) * currentJourneyNo)
				{
					if (reverseAtEnd) // reverse direction of going through List
					{
						listDirection *= -1; // invert List direction
						
						currentListPos = Mathf.Clamp(currentListPos, 0, travelPointss.Count-1);
						startLerpPoint = startPoint + travelPointss[currentListPos];
						
						currentListPos += listDirection;
						
					}else{ // reset it
						currentListPos = 1; 
						startLerpPoint = startPoint;
					}
					
					currentJourneyNo++;
				}
				
				worldNextPoint = startPoint + travelPointss[currentListPos];
				
				currentTripNo++;
				if (currentTripNo >= noOfTrips+1 && noOfTrips != 0) // if reached noOfTrips limit, break out of loop
				{
					yield break;
				}
				
				yield return new WaitForSeconds(pauseTime);
			}
			
			t += Time.deltaTime;	
			float lerpAmount = t/movementTime;
			transform.position = Vector3.Lerp(startLerpPoint, worldNextPoint, lerpAmount);
			
			
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
	}
	
	IEnumerator WaitForGameStart () {
		while (true)
		{
			// if (GameStates.GetState() == "Playing")
			{
				playerScript = GameObject.Find("Player").GetComponent<PlayerControl>();
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
				StartCoroutine("Move");
				yield break;
			}
			yield return null;
		}
	}
	
}
