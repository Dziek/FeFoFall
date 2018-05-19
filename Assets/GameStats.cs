using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameStats : MonoBehaviour {
	
	// might make streaks per mode. Might not save them
	private int goodStreak;
	private int badStreak;
	
	// global stats across all modes
	private float timePlayed;
	private int levelsCompleted;
	private int globalAttempts;
}

[System.Serializable]
public class ModeStats {
	public int levelsCompleted;
	public int numberOfSessions;
	
	public bool isCompleted; // possibly redundant
	public int timesCompleted;
	
	public int currentAttempts;
	public int totalAttempts;
	public int bestAttempts;
	
	public float secondsPlayedCurrent;
	public float secondsPlayedTotal;
	public float bestTime;
}

[System.Serializable]
public class LevelStats {	
	public bool isCompleted; //
	public int timesCompleted; //
	
	public int currentAttempts; //
	public int totalAttempts; //
	public int bestAttempts; //
	
	public float secondsPlayedCurrent; //
	public float secondsPlayedTotal; //
	// public float bestTime;
}
