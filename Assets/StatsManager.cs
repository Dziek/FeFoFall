using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// using System.Runtime.Serialization.Formatters.Binary;
// using System.IO;

// using Microsoft.Xbox.Services.Statistics.Manager;

public class StatsManager : MonoBehaviour {
	
	// public static StatsManager instance;
	
	public Dictionary <Mode, List<string>> levelIDsByMode = new Dictionary <Mode, List<string>>();
	private string currentLevelID; // currently levelGO name, change to something else
	
	private int currentStreak; // positive numbers are success streak, negative is failure streak
	private bool streakBreak; // to be used for TransitionText knowing if a streak was broken
	// private float lastSeconds; // how long it took for the last Timer
	
	private bool isTimerRunning;
	private float timeValue;
	
	// THIS IS TEMPORARY UNTIL PROPER SAVING/LOADING MODE STATS IS IN
	void Awake () {
		
		
		// DataManager.LoadData();
		// DataManager.ClearData();
		
		if (Application.loadedLevelName != "LevelTesting" && Application.loadedLevelName != "GraphicsTesting")
		{
			// Debug.Log("REMEMBER TO TAKE THIS OUT");
			// DataManager.modeStats.Add(Mode.Main, new ModeStats());
			// DataManager.modeStats.Add(Mode.Tricky, new ModeStats());
			// DataManager.modeStats.Add(Mode.GauntletA, new ModeStats());
			// DataManager.modeStats.Add(Mode.GauntletB, new ModeStats());
			// DataManager.modeStats.Add(Mode.Basement, new ModeStats());
			
			
			DataManager.LoadData();
			
			//GOOD? Check if 
			
			CheckForMode(Mode.Main);
			CheckForMode(Mode.Tricky);
			CheckForMode(Mode.Time);
			CheckForMode(Mode.Lives);
			CheckForMode(Mode.OneShot);
			CheckForMode(Mode.Basement);
		}
	}
	
	// Checks if mode is already in the saved data
	void CheckForMode (Mode m) {
		if (DataManager.modeStats.ContainsKey(m) == false)
		{
			DataManager.modeStats.Add(m, new ModeStats());
			// Debug.Log("Added Mode");
		}
		// }else{
			// Debug.Log("Found Mode Already");
		// }
	}
	
	// void Awake () {
		
		// instance = this;
		
		
		
		// DataManager.LoadData();
	// }

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
		
		DataManager.SaveData();
	}
	
	void UpdateCurrentLevel (GameObject levelGO) {
		currentLevelID = levelGO.name;
	}
	
	void LevelStarted () {
		if (Application.loadedLevelName != "LevelTesting" && Application.loadedLevelName != "GraphicsTesting")
		{
			DataManager.levelStats[currentLevelID].currentAttempts++;
			DataManager.levelStats[currentLevelID].totalAttempts++;
			
			streakBreak = false;
			
			StartTimer();
		}
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
			if (currentStreak <= -6)
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
			if (currentStreak >= 3)
			{
				streakBreak = true;
			}
			currentStreak = 0;
		}
		
		currentStreak--;
	}
	
	// void ModeStarted (Mode mode) {
		// DataManager.modeStats[mode].timesStarted++;
		
		// DataManager.SaveData();
	// }
	
	public void ModeComplete (Mode mode) {
		DataManager.modeStats[mode].timesCompleted++;
		
		// do best attempts
		
		int totalAttempts = 0;
		
		foreach (string id in levelIDsByMode[mode])
		{
			totalAttempts += DataManager.levelStats[id].currentAttempts;
		}
		
		if (totalAttempts < DataManager.modeStats[mode].bestAttempts || DataManager.modeStats[mode].bestAttempts == 0)
		{
			DataManager.modeStats[mode].bestAttempts = totalAttempts;
			
			// #if WINDOWS_UWP
			
				// StatisticManager.SetStatisticIntegerData(1, "bestAttempts", totalAttempts);
				// XboxLive.Instance.StatsManager.SetStatisticIntegerData(1, "bestAttempts", totalAttempts);
				
				// when I do this quote teat my cautious optimism
			
			// #endif
		}
		
		// do best time
		
		float totalTime = 0;
		
		foreach (string id in levelIDsByMode[mode])
		{
			totalTime += DataManager.levelStats[id].secondsPlayedCurrent;
		}
		
		if (totalTime < DataManager.modeStats[mode].bestTime || DataManager.modeStats[mode].bestTime == 0)
		{
			DataManager.modeStats[mode].bestTime = totalTime;
		}
		
		DataManager.SaveData();
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
		
		// Debug.Log(levelGOs.Count);
		
		levelIDsByMode.Add(mode, new List<string>());
		
		
		
		// for (int i = 0; i < levelGOs.Count; i++)
		foreach (GameObject level in levelGOs)
		{
			// levelIDsByMode[mode].Add(levelGOs[i].name);
			
			string s = level.name;
			levelIDsByMode[mode].Add(s);
			
			// levelIDsByMode[mode].Add(level.name);
		}
	}
	
	public LevelStats LevelLookUp (string n) {
		
		if (DataManager.levelStats.ContainsKey(n) == false)
		{
			DataManager.levelStats.Add(n, new LevelStats());
		}
		
		return DataManager.levelStats[n];
	}
	
	public void ClearModeStats (Mode mode) {
		// go through every level, making them not completed and setting current attempts / whatever to 0
		
		foreach (string id in levelIDsByMode[mode])
		{
			DataManager.levelStats[id].isCompleted = false;
			DataManager.levelStats[id].currentAttempts = 0;
			DataManager.levelStats[id].secondsPlayedCurrent = 0;
		}
		
		// increase timesStarted here (although not technically correct I can probably get away with it)
		DataManager.modeStats[mode].timesStarted++;
		
		DataManager.SaveData();
		// Application.LoadLevel(Application.loadedLevel);
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
	
	public int GetCurrentFailures (Mode mode) {
		int currentFailures;
		
		currentFailures = GetCurrentAttempts(mode) - GetLevelsCompleted(mode);
		
		return currentFailures;
	}
	
	public float GetAverageAttemptsPerLevel (Mode mode) {
		float currentAttempts = (float)GetCurrentAttempts(mode);
		float noOfLevels = (float)GetNumberOfLevels(mode);
		
		float averageAttempts = currentAttempts / noOfLevels;
		
		return averageAttempts;
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
		// Debug.Log(currentLevelID);
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
	
	public int GetModeTimesCompleted (Mode mode) {
		return DataManager.modeStats[mode].timesCompleted;
	}
	
	public int GetModeTimesStarted (Mode mode) {
		return DataManager.modeStats[mode].timesStarted;
	}
	
	public int GetModeBestAttempts (Mode mode) {
		return DataManager.modeStats[mode].bestAttempts;
	}
	
	public int GetModeTotalAttempts (Mode mode) {
		int totalAttempts = 0;
		
		for (int i = 0; i < levelIDsByMode[mode].Count; i++)
		{
			string lookUpID = levelIDsByMode[mode][i];
			totalAttempts += DataManager.levelStats[lookUpID].totalAttempts;
		}
		
		return totalAttempts;
	}
	
	public int GetModeTotalLevelsCompleted (Mode mode) {
		int levelsCompleted = 0;
		
		for (int i = 0; i < levelIDsByMode[mode].Count; i++)
		{
			string lookUpID = levelIDsByMode[mode][i];
			levelsCompleted += DataManager.levelStats[lookUpID].timesCompleted;
		}
		
		return levelsCompleted;
	}
	
	public float GetModeTotalSeconds (Mode mode) {
		float totalSeconds = 0;
		
		for (int i = 0; i < levelIDsByMode[mode].Count; i++)
		{
			string lookUpID = levelIDsByMode[mode][i];
			totalSeconds += DataManager.levelStats[lookUpID].secondsPlayedTotal;
		}
		
		return totalSeconds;
	}
	
	public float GetModeBestSeconds (Mode mode) {
		return DataManager.modeStats[mode].bestTime;
	}
}
