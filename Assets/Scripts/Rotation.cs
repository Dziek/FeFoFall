using UnityEngine;
using System.Collections;

public class Rotation : MonoBehaviour {
	
	// public bool antiClockwise;
	public float speed;
	
	private float startAngle;
	private float endAngle;
	public float rotationAmount;
	
	public float pause;
	
	public bool waitForPlayerMovement;
	public bool oneWay; // if want to use this pause must be at least 0.1f
	public bool oneCircuit; // if want to use this pause must be at least 0.1f
	public bool sway;
	public bool skipFirstPause;
	
	public bool useBelowPivot; // can only be 360
	public int dir; // must be 1 or -1, only used when using pivot
	public Vector2 pivot;
	
	private float timer;
	private float oneWayCutOff;
	private float oneCircuitCutOff;
	private float skipPauseCutOff;
	private bool waitForTrigger;
	private bool partOneCompleteForOneCircuit;
	private bool partOneCompleteForSkipPause;
	
	private PlayerControl playerScript;
	// private bool triggerActive;
	// private bool canChange = true;
	
	void OnEnable () {
		Messenger<PlayerControl>.AddListener("SetPlayer", UpdatePlayerControl);
	}
	
	void OnDisable () {
		Messenger<PlayerControl>.RemoveListener("SetPlayer", UpdatePlayerControl);
	}
	
	void UpdatePlayerControl (PlayerControl pC) {
		playerScript = pC;
	}
	
	// Use this for initialization
	void Start () {
		startAngle = transform.eulerAngles.z;
		endAngle = startAngle + rotationAmount;
		
		// playerScript = GameObject.Find("Player").GetComponent<PlayerControl>();
		
		oneWayCutOff = 1 + (pause/2);
		oneCircuitCutOff = 0 - (pause/2);
		skipPauseCutOff = 1 + (pause*0.99f);
		
		if (!waitForTrigger)
		{
			if (waitForPlayerMovement)
			{
				StartCoroutine("WaitForGameStart");
			}else{
				StartCoroutine("Rotate");
			}
		}
		
		// Debug.Log(waitForTrigger);
	}
	
	IEnumerator Rotate () {
		// while (GameStates.GetState() == "Playing")
		while (true)
		{
			if (GameStates.GetState() == "Playing")
			{
				// float difference = startAngle - transform.eulerAngles.z;
				
				// if (difference < 0)
				// {
					// difference += 360;
				// }
				
				// Debug.Log(difference);
				
				// if (difference >= rotationAmount || difference <= startAngle)
				// if (CalculateDifference() > endAngle || CalculateDifference() < 0)
				// if (difference >= rotationAmount)
				// {
					// antiClockwise = !antiClockwise;
					// Debug.Log("Pause " + difference);
					
					// transform.Rotate(Vector3.forward * rotationAmount);
					
					// yield return new WaitForSeconds(pause);
				// }
				
				// if (difference <= startAngle)
				// {
					// antiClockwise = !antiClockwise;
					// Debug.Log("Pause " + difference);
					
					// transform.Rotate(Vector3.forward * -difference);
					
					// yield return new WaitForSeconds(pause);
				// }
				
				// Debug.Log("Turn " + difference);
				
				// Debug.Log(difference);
				float pos = 0;
				float angle = 0;
				
				// Debug.Log("D");
				if (rotationAmount > 180)
				{
					if (useBelowPivot == false)
					{
						transform.Rotate(Vector3.forward * Time.deltaTime * (speed*dir), Space.World);
					}else{
						transform.RotateAround(pivot, Vector3.forward, (speed*dir) * Time.deltaTime);
					}
					
					// transform.Rotate(Vector3.forward * Time.deltaTime * (speed*dir), Space.World);
					// transform.Rotate(Vector3.forward * Time.deltaTime * 90, Space.World);
					// Debug.Log("eh");
				}else{
					timer += Time.deltaTime;
					
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
							yield break;
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
							yield break;
						}
					}
					
					// if (pos > 1 && !oneWay && canChange)
					// {
						// dir *= -1;
						// canChange = false;
					// }
					
					// if (pos < 0 && !oneWay && !canChange)
					// {
						// dir *= -1;
						// canChange = true;
					// }
					
					angle = Mathf.LerpAngle(startAngle, endAngle, pos);
					// float angle = Mathf.LerpAngle(0, -90, pos);
					// transform.eulerAngles = new Vector3(0, 0, angle);
					
					if (sway == false)
					{
						transform.eulerAngles = new Vector3(0, 0, angle);
					}else{
						transform.Rotate(Vector3.forward * pos, Space.World);
						// transform.Rotate(Vector3.forward * Mathf.Clamp(pos, 0, 1), Space.World);
					}
				}
				
				// if (useBelowPivot == false)
				// {
					// if (sway == false)
					// {
						// transform.eulerAngles = new Vector3(0, 0, angle);
					// }else{
						// transform.Rotate(Vector3.forward * pos, Space.World);
						// transform.Rotate(Vector3.forward * Mathf.Clamp(pos, 0, 1), Space.World);
					// }
				// }else{
					// transform.RotateAround(pivot, Vector3.forward, (speed*dir) * Time.deltaTime);
				// }
				
				// if (useBelowPivot == false)
				// {
					// transform.Rotate(Vector3.forward * pos, Space.World);
				// }else{
					// transform.RotateAround(pivot, Vector3.forward, pos);
				// }
				
				// if (useBelowPivot == false)
				// {
					// if (antiClockwise)
					// {
						// transform.Rotate(Vector3.forward * Time.deltaTime * speed, Space.World);
					// }else{
						// transform.Rotate(Vector3.forward * Time.deltaTime * -speed, Space.World);
					// }
				// }else{
					// if (antiClockwise)
					// {
						// transform.RotateAround(pivot, Vector3.forward, speed * Time.deltaTime);
					// }else{
						// transform.RotateAround(pivot, Vector3.forward, -speed * Time.deltaTime);
					// }
				// }
				
				// if (difference >= rotationAmount || difference <= startAngle)
				// if (difference >= rotationAmount)
				// {
					// antiClockwise = !antiClockwise;
					// Debug.Log("Correction " + (difference-rotationAmount));
					
					// transform.Rotate(Vector3.forward * (difference-rotationAmount));
					
					// yield return new WaitForSeconds(pause);
				// }
				
			}
			
			yield return null;
		}
		
		Debug.Log("End");
		yield return null;
	}
	
	float CalculateDifference () {
		// float difference = startAngle - transform.eulerAngles.y;
				
		// if (difference < 0)
		// {
			// difference += 360;
		// }
		
		// Debug.Log(difference);
		// return difference;
		
		return Vector3.Angle(Vector3.up, transform.up);
	}
	
	public void WaitForTrigger () {
		waitForTrigger = true;
	}
	
	public void TriggerActivated (float time) {
		// timer = 0;
		StartCoroutine("Rotate");
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
		StopCoroutine("Rotate");
	}
	
	IEnumerator WaitForGameStart () {
		while (true)
		{
			if (GameStates.GetState() == "Playing")
			{
				if (Application.loadedLevelName == "LevelTesting" || Application.loadedLevelName == "GraphicsTesting")
				{
					playerScript = GameObject.Find("Player").GetComponent<PlayerControl>();
				}
				
				StartCoroutine("CheckForPlayerMovement");
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
				StartCoroutine("Rotate");
				yield break;
			}
			yield return null;
		}
		
		yield return null;
	}

}
