using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// I could make this rely solely on Stats, but I think because of the order it does things, it's best to process it locally too

public class TimeChallengeManager : MonoBehaviour {

	public float startingTime = 10;
	public float timeBonus = 3; // for completing a level

	private float currentTime;
	
	private bool timeModeOn;
	private bool needsReset; // tells TransitionController whether to reset
	
	private StatsManager statsManager;
	
	void Awake () {
		statsManager = GameObject.Find("StatsManager").GetComponent<StatsManager>();
	}
	
	void Start () {
		// fetch current time
		// currentTime = startingTime; // temp because no saving
		currentTime = CalculateTimeFromStats(); // temp because no saving
	}
	
	public void SelectTimeMode () {
		
		if (currentTime <= 0 || needsReset == true)
		{
			currentTime = startingTime;
		}
		
		timeModeOn = true;
		// needsReset = false;
	}
	
	public void DeselectTimeMode () {
		timeModeOn = false;
		
		if (currentTime <= 0)
		{
			needsReset = true;
		}
	}
	
	// public void LevelFailed () {
		// if (timeModeOn == true)
		// {	
			// currentTime--;
			
			// if (currentTime <= 0)
			// {
				// needsReset = true;
			// }
		// }
	// }
	
	public void LevelStarted () {
		if (timeModeOn == true)
		{
			StartCoroutine("Timer");
		}
	}
	
	public void LevelOver () {
		if (timeModeOn == true)
		{
			StopCoroutine("Timer");
			
			// Debug.Log("Local: " + currentTime + " Stats: " + CalculateTimeFromStats());
		}
	}
	
	IEnumerator Timer () {
		
		while (currentTime > 0)
		{
			currentTime -= Time.deltaTime;
			yield return null;
		}
		
		// make the game be over
		// Debug.Log("Local: " + currentTime + " Stats: " + CalculateTimeFromStats());
		
		Debug.Log("Outta Time Baby");
		needsReset = true;
		
		Messenger.Broadcast("LevelOver");
		Messenger.Broadcast("Failure");
		GameStates.ChangeState("Transition", "Bad");
		
		Messenger<TransitionReason>.Broadcast("Transition", TransitionReason.levelFailure);
		
	}
	
	public void LevelCompleted () {
		if (timeModeOn == true)
		{
			currentTime += timeBonus;
		}
		
		needsReset = false;
	}
	
	public float GetTime () {
		Debug.Log("Local: " + currentTime + " Stats: " + CalculateTimeFromStats());
		return currentTime;
	}
	
	public bool CheckResetNeed () {
		return needsReset;
	}
	
	void OnEnable () {
		Messenger.AddListener("ModeUpdated", DeselectTimeMode);
		Messenger.AddListener("Success", LevelCompleted);
		// Messenger.AddListener("Failure", LevelFailed);
		Messenger.AddListener("LevelStarted", LevelStarted);
		Messenger.AddListener("LevelOver", LevelOver);
	}
	
	void OnDisable () {
		Messenger.RemoveListener("ModeUpdated", DeselectTimeMode);
		Messenger.RemoveListener("Success", LevelCompleted);
		// Messenger.RemoveListener("Failure", LevelFailed);
		Messenger.RemoveListener("LevelStarted", LevelStarted);
		Messenger.RemoveListener("LevelOver", LevelOver);
	}
	
	float CalculateTimeFromStats () {
		float timeSpent; // how much time spent in current game
		float timeOffset; // what to subtract timeSpent from - startingTime + (currentLevelsComplete * timeBonus)
		
		timeSpent = statsManager.GetCurrentSeconds(Mode.Time);
		timeOffset = startingTime + (statsManager.GetLevelsCompleted(Mode.Time) * timeBonus);
		
		float cT = timeOffset - timeSpent;
		
		return cT;
	}
}
