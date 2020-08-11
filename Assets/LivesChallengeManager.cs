using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// I could make this rely solely on Stats, but I think because of the order it does things, it's best to process it locally too

public class LivesChallengeManager : MonoBehaviour {

	public int startingLives = 10;
	public int livesBonus = 3; // for completing a level

	private int currentLives;
	
	private bool livesModeOn;
	private bool needsReset; // tells TransitionController whether to reset
	
	private StatsManager statsManager;
	
	void Awake () {
		statsManager = GameObject.Find("StatsManager").GetComponent<StatsManager>();
	}
	
	void Start () {
		// fetch current lives
		// currentLives = startingLives; // temp because no saving
		currentLives = CalculateLivesFromStats(); // temp because no saving
	}
	
	public void SelectLivesMode () {
		
		if (currentLives <= 0 || needsReset == true)
		{
			currentLives = startingLives;
		}
		
		livesModeOn = true;
		// needsReset = false;
	}
	
	public void DeselectLivesMode () {
		livesModeOn = false;
		
		if (currentLives <= 0)
		{
			needsReset = true;
		}
	}
	
	public void LevelFailed () {
		if (livesModeOn == true)
		{	
			currentLives--;
			
			if (currentLives <= 0)
			{
				needsReset = true;
			}
		}
	}
	
	public void LevelCompleted () {
		if (livesModeOn == true)
		{
			currentLives += livesBonus;
		}
		
		needsReset = false;
	}
	
	public int GetLives () {
		Debug.Log("Current: " + currentLives + " Stats: " + CalculateLivesFromStats());
		return currentLives;
	}
	
	public bool CheckResetNeed () {
		return needsReset;
	}
	
	void OnEnable () {
		Messenger.AddListener("ModeUpdated", DeselectLivesMode);
		Messenger.AddListener("Success", LevelCompleted);
		Messenger.AddListener("Failure", LevelFailed);
	}
	
	void OnDisable () {
		Messenger.RemoveListener("ModeUpdated", DeselectLivesMode);
		Messenger.RemoveListener("Success", LevelCompleted);
		Messenger.RemoveListener("Failure", LevelFailed);
	}
	
	int CalculateLivesFromStats () {
		int levelsFailed;
		int livesBuffer;
		
		levelsFailed = statsManager.GetCurrentFailures(Mode.Lives);
		livesBuffer = startingLives + (statsManager.GetLevelsCompleted(Mode.Lives) * livesBonus);
		
		int cL = livesBuffer - levelsFailed;
		
		return cL;
	}
}
