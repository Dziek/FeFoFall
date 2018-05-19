using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// using System.Runtime.Serialization.Formatters.Binary;
// using System.IO;

public class StatsManager : MonoBehaviour {
	
	// public static StatsManager instance;
	
	public Dictionary <Mode, List<string>> levelIDsByMode = new Dictionary <Mode, List<string>>();
	private string currentLevelID; // currently levelGO name, change to something else
	
	private int currentStreak; // positive numbers are success streak, negative is failure streak
	private bool streakBreak; // to be used for TransitionText knowing if a streak was broken
	// private float lastSeconds; // how long it took for the last Timer
	
	private bool isTimerRunning;
	private float timeValue;

	void Awake () {
		
		// instance = this;
		
		// DataManager.levelStatsByMode.Add("Main", new Dictionary <string, LevelStats>());
		// DataManager.levelStatsByMode.Add("Tricky", new Dictionary <string, LevelStats>());
		// DataManager.levelStatsByMode.Add("Basement", new Dictionary <string, LevelStats>());
		// DataManager.levelStatsByMode.Add("GauntletA", new Dictionary <string, LevelStats>());
		// DataManager.levelStatsByMode.Add("GauntletB", new Dictionary <string, LevelStats>());
		
		// DataManager.LoadData();
	}

	void OnEnable () {
		Messenger<GameObject>.AddListener("NewLevel", UpdateCurrentLevel);
		Messenger.AddListener("LevelStarted", LevelStarted);
		Messenger.AddListener("LevelOver", LevelOver);
		Messenger.AddListener("BackToMenu", LevelOver);
		Messenger.AddListener("Success", LevelCompleted);
		Messenger.AddListener("Failure", LevelFailed);
	}
	
	void OnDisable () {
		Messenger<GameObject>.RemoveListener("NewLevel", UpdateCurrentLevel);
		Messenger.RemoveListener("LevelStarted", LevelStarted);
		Messenger.RemoveListener("LevelOver", LevelOver);
		Messenger.RemoveListener("BackToMenu", LevelOver);
		Messenger.RemoveListener("Success", LevelCompleted);
		Messenger.RemoveListener("Failure", LevelFailed);
		
		// DataManager.SaveData();
	}
	
	void UpdateCurrentLevel (GameObject levelGO) {
		currentLevelID = levelGO.name;
	}
	
	void LevelStarted () {
		DataManager.levelStats[currentLevelID].currentAttempts++;
		DataManager.levelStats[currentLevelID].totalAttempts++;
		
		streakBreak = false;
		
		StartTimer();
	}
	
	void LevelOver () {
		StopTimer();
	}
	
	void LevelCompleted () {
		
		DataManager.levelStats[currentLevelID].isCompleted = true;
		DataManager.levelStats[currentLevelID].timesCompleted++;
		
		if (DataManager.levelStats[currentLevelID].bestAttempts > DataManager.levelStats[currentLevelID].currentAttempts)
		{
			DataManager.levelStats[currentLevelID].bestAttempts = DataManager.levelStats[currentLevelID].currentAttempts;
		}
		
		if (currentStreak < 0)
		{
			if (currentStreak < -4)
			{
				streakBreak = true;
			}
			currentStreak = 0;
		}
		
		currentStreak++;
	}
	
	void LevelFailed () {
		if (currentStreak > 0)
		{
			if (currentStreak > 4)
			{
				streakBreak = true;
			}
			currentStreak = 0;
		}
		
		currentStreak--;
	}
	
	void StartTimer () {
		StartCoroutine(Timer());
		// Debug.Log("StartTimer");
	}
	
	void StopTimer () {

		if (isTimerRunning == true)
		{
			isTimerRunning = false;
			// Debug.Log("StopTimer");
			
			// secondsPlayedTotal += timeValue;
			// secondsPlayedCurrent += timeValue;
			
			DataManager.levelStats[currentLevelID].secondsPlayedCurrent += timeValue;
			DataManager.levelStats[currentLevelID].secondsPlayedTotal += timeValue;
			
			// secondsPlayedLast = timeValue;
			// Debug.Log("Setting Seconds Last Played To " + timeValue);
		}
	}
	
	IEnumerator Timer () {
		timeValue = 0;
		
		isTimerRunning = true;
		
		while (isTimerRunning == true)
		{
			timeValue += Time.deltaTime;
			yield return null;
		}
	}
	
	public void AddLevelsByMode (List<GameObject> levelGOs, Mode mode) {
		
		// foreach (string n in levelGOs)
		// {
			// string n = levelGOs[i].name;
			
			// if (DataManager.levelStats.ContainsKey(n) == false)
			// {
				// DataManager.levelStats.Add(n, new LevelStats());
			// }	
		// }
		
		levelIDsByMode.Add(mode, new List<string>());
		
		// for (int i = 0; i < levelGOs.Count; i++)
		foreach (GameObject level in levelGOs)
		{
			// levelIDsByMode[mode].Add(levelGOs[i].name);
			levelIDsByMode[mode].Add(level.name);
		}
	}
	
	public LevelStats LevelLookUp (string n) {
		
		if (DataManager.levelStats.ContainsKey(n) == false)
		{
			DataManager.levelStats.Add(n, new LevelStats());
		}
		
		return DataManager.levelStats[n];
	}
	
	// STATS RETRIEVAL FOR TEXT 
	
	public int GetNumberOfLevels (Mode mode) {
		return levelIDsByMode[mode].Count;
	}
	
	public int GetLevelsCompleted (Mode mode) {
		int levelsCompleted = 0;
		
		for (int i = 0; i < levelIDsByMode[mode].Count; i++)
		{
			string lookUpID = levelIDsByMode[mode][i];
			
			if (DataManager.levelStats[lookUpID].isCompleted == true)
			{
				levelsCompleted++;
			}
		}
		
		return levelsCompleted;
	}
	
	public int GetNumberOfLevelsRemaining (Mode mode ) {
		
		int nOL = GetNumberOfLevels(mode);
		int lC = GetLevelsCompleted(mode);
		
		return nOL - lC;
	}
	
	public int GetCurrentAttempts (Mode mode) {
		int currentAttempts = 0;
		
		for (int i = 0; i < levelIDsByMode[mode].Count; i++)
		{
			string lookUpID = levelIDsByMode[mode][i];
			currentAttempts += DataManager.levelStats[lookUpID].currentAttempts;
		}
		
		return currentAttempts;
	}
	
	public float GetCurrentSeconds (Mode mode) {
		float currentSeconds = 0;
		
		for (int i = 0; i < levelIDsByMode[mode].Count; i++)
		{
			string lookUpID = levelIDsByMode[mode][i];
			currentSeconds += DataManager.levelStats[lookUpID].secondsPlayedCurrent;
		}
		
		return currentSeconds;
	}
	
	public int GetCurrentLevelCurrentAttempts () {
		Debug.Log(currentLevelID);
		return DataManager.levelStats[currentLevelID].currentAttempts;
	}
	
	public float GetPercentageComplete (Mode mode) {
		return (float)GetLevelsCompleted(mode) / (float)GetNumberOfLevels(mode);
	}
	
	public int GetCurrentStreak () {
		return currentStreak;
	}
	
	public bool GetStreakBreak () {
		return streakBreak;
	}
	
	public float GetLastSeconds () {
		return timeValue;
	}
	
	public float GetCurrentLevelCurrentSeconds() {
		return DataManager.levelStats[currentLevelID].secondsPlayedCurrent;
	}
}
