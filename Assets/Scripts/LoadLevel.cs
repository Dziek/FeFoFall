using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LoadLevel : MonoBehaviour {
		 
	public static LoadLevel instance;
	
	public static List<GameObject> levelsInQueue = new List<GameObject>();
	static bool[] completedLevels;
	
	static GameObject currentLevel;
	
	static int nextLevelQueuePos;
	static int lastLevelQueuePos = -1;
	static int beforeLastLevelQueuePos = -1;
	
	static int lastLevelID = -1;
	static bool timerRunning;
	static float timeValue;
	
	public int activeRange = 5; // how many levels can be 'active' at a time
	
	static bool goodStreakBreak;
	static bool badStreakBreak;
	
	static int bestAttemptsLast;
	static float secondsPlayedLast;
	static float bestTimeLast;
	
	static int levelsCompleted;
	static int currentAttempts;
	static int numberOfLevels;
	static int timesStarted;
	static int timesCompleted;
	static int bestAttempts;
	static int totalAttempts;
	static int totalLevelsCompleted;
	
	static int numberOfSessions;
	
	// static bool onStreak;
	static int currentGoodStreak;
	static int currentBadStreak;
	static int bestGoodStreak;
	static int bestBadStreak;
	
	static float secondsPlayedTotal;
	static float secondsPlayedCurrent;
	static float secondsPlayedBest;
	
	public struct LevelStats {
		public int currentAttempts;
		public int totalAttempts;
		public int timesCompleted;
		public int bestAttempts;
		
		public float secondsPlayedCurrent;
		public float secondsPlayedTotal;
		public float bestTime;
	}
	
	private static LevelStats[] individualLevelStats;
	
	void Awake () {
	
		// PlayerPrefs.DeleteAll();
		
		// Debug.Log(Time.time);
		
		instance = this;
		
		List<GameObject> fullListOfLevels = new List<GameObject>(Resources.LoadAll<GameObject>("prefabs"));
		numberOfLevels = fullListOfLevels.Count;
		
		completedLevels = new bool[numberOfLevels];
		individualLevelStats = new LevelStats[numberOfLevels];
		
		#if UNITY_STANDALONE || UNITY_EDITOR || UNITY_ANDROID || UNITY_WSA_10_0
			for (int i = 0; i < numberOfLevels; i++)
			{
				if (PlayerPrefsX.GetBool("Level"+fullListOfLevels[i].name+"Completed") == false)
				{
					levelsInQueue.Add(fullListOfLevels[i]);
					
					// GameObject levelGO = fullListOfLevels[i];
					
					// levelGO.name = "Level" + i.ToString("00000");
					
					// levelsInQueue.Add(levelGO);
				}else{
					levelsCompleted++;
				}
				
				individualLevelStats[i].currentAttempts = PlayerPrefs.GetInt("Level"+fullListOfLevels[i].name+"CurrentAttempts");
				individualLevelStats[i].totalAttempts = PlayerPrefs.GetInt("Level"+fullListOfLevels[i].name+"TotalAttempts");
				individualLevelStats[i].timesCompleted = PlayerPrefs.GetInt("Level"+fullListOfLevels[i].name+"TimesCompleted");
				individualLevelStats[i].bestAttempts = PlayerPrefs.GetInt("Level"+fullListOfLevels[i].name+"BestAttempts");
				
				individualLevelStats[i].secondsPlayedCurrent = PlayerPrefs.GetFloat("Level"+fullListOfLevels[i].name+"SecondsPlayedCurrent");
				individualLevelStats[i].secondsPlayedTotal = PlayerPrefs.GetFloat("Level"+fullListOfLevels[i].name+"SecondsPlayedTotal");
				individualLevelStats[i].bestTime = PlayerPrefs.GetFloat("Level"+fullListOfLevels[i].name+"BestTime");
			}
			
			currentAttempts = PlayerPrefs.GetInt("CurrentAttempts");
			timesStarted = PlayerPrefs.GetInt("TimesStarted", 1);
			timesCompleted = PlayerPrefs.GetInt("TimesCompleted");
			bestAttempts = PlayerPrefs.GetInt("BestAttempts");
			totalAttempts = PlayerPrefs.GetInt("TotalAttempts");
			totalLevelsCompleted = PlayerPrefs.GetInt("TotalLevelsCompleted");
			
			numberOfSessions = PlayerPrefs.GetInt("NumberOfSessions");
			
			currentGoodStreak = PlayerPrefs.GetInt("CurrentGoodStreak");
			currentBadStreak = PlayerPrefs.GetInt("CurrentBadStreak");
			bestGoodStreak = PlayerPrefs.GetInt("BestGoodStreak");
			bestBadStreak = PlayerPrefs.GetInt("BestBadStreak");
			
			secondsPlayedTotal = PlayerPrefs.GetFloat("SecondsPlayedTotal");
			secondsPlayedCurrent = PlayerPrefs.GetFloat("SecondsPlayedCurrent");
			secondsPlayedBest = PlayerPrefs.GetFloat("SecondsPlayedBest");
		#endif
		
		#if UNITY_WEBGL && !UNITY_EDITOR
			Application.ExternalCall("UpdateNumberOfLevels", numberOfLevels);
			Application.ExternalCall("GetStats");
			
			for (int i = 0; i < completedLevels.Length; i++)
			{
				if (completedLevels[i] == false)
				{	
					// GameObject levelGO = fullListOfLevels[i];
					
					// levelGO.name = "Level" + i.ToString("00000");
					
					// levelsInQueue.Add(levelGO);
					// Debug.Log(levelsInQueue[i].name);
				}else{
					levelsCompleted++;
				}
			}
		#endif
		
		// Debug.Log(Time.time);
		
		numberOfSessions++;
		PlayerPrefs.SetInt("NumberOfSessions", numberOfSessions);
	}
	
	public static void GetLevel () {
		
		// Debug.Log("GetLevel");
		
		if (currentLevel != null)
		{
			Destroy(currentLevel);
		}
		
		if (levelsInQueue.Count == 0)
		{
			// GameStates.ChangeState("Complete");
			nextLevelQueuePos = -1;
			Debug.Log("D");
			// return null;
		}else if (levelsInQueue.Count == 1)
		{
			nextLevelQueuePos = 0;
		}else if (levelsInQueue.Count == 2)
		{
			nextLevelQueuePos = Random.Range(0, 2);
		}else if (levelsInQueue.Count > 2){
			while (nextLevelQueuePos == lastLevelQueuePos || nextLevelQueuePos == beforeLastLevelQueuePos) 
			{	
				if (levelsInQueue.Count < instance.activeRange)
				{
					nextLevelQueuePos = Random.Range(0, levelsInQueue.Count);
				}else{
				
					int bottomOfRangeID = ConvertLevelNameToID(levelsInQueue[0].name);
					int topOfRangeID = ConvertLevelNameToID(levelsInQueue[instance.activeRange-1].name);
					
					if ((topOfRangeID - bottomOfRangeID) > 10 && lastLevelQueuePos != 0)
					{
						int r = Random.Range(0,2);
						if (r == 0)
						{
							nextLevelQueuePos = 0;
							// Debug.Log("Set to early level override");
						}else{
							nextLevelQueuePos = Random.Range(0, instance.activeRange);
						}
					}else{
						nextLevelQueuePos = Random.Range(0, instance.activeRange);
					}
					
					// Debug.Log("Finding suitable level " + "BeforeLast: " + beforeLastLevelQueuePos.ToString() + 
								// " Last: " + lastLevelQueuePos.ToString() + " Next: " + nextLevelQueuePos.ToString());
				}
			}
		}
		
		if (nextLevelQueuePos == lastLevelQueuePos)
		{
			Debug.Log("Well that shouldn't happen");
			Debug.Break();
		}
		
		if (nextLevelQueuePos != -1)
		{
			currentLevel = Instantiate(levelsInQueue[nextLevelQueuePos], Vector3.zero, Quaternion.identity) as GameObject;
			
			// Debug.Log("Created New Level");
			
			// float x = Random.Range(0, 2) == 0 ? 1 : -1;
			// float y = Random.Range(0, 2) == 0 ? 1 : -1;
			// currentLevel.transform.localScale = new Vector3(x, y, 1);
		}
		
		beforeLastLevelQueuePos = lastLevelQueuePos;
		lastLevelQueuePos = nextLevelQueuePos;
		
		lastLevelID = ConvertLevelNameToID(levelsInQueue[nextLevelQueuePos].name);
	}
	
	public static void ClearLevel () {
		if (currentLevel != null)
		{
			Destroy(currentLevel);
		}
	}
	
	public static void LevelFailed () {
		currentBadStreak++;
		
		if (currentGoodStreak > 4)
		{
			goodStreakBreak = true;
		}
		
		currentGoodStreak = 0;
		
		if (currentBadStreak > bestBadStreak)
		{
			bestBadStreak = currentBadStreak;
		}
		
		#if UNITY_STANDALONE || UNITY_EDITOR || UNITY_ANDROID || UNITY_WSA_10_0
			PlayerPrefs.SetInt("CurrentGoodStreak", currentGoodStreak);
			PlayerPrefs.SetInt("CurrentBadStreak", currentBadStreak);
			PlayerPrefs.SetInt("bestBadStreak", bestBadStreak);
		#endif
	}
	
	public static bool LevelCompleted () {
		
		// StopTimer();
		
		currentGoodStreak++;
		
		if (currentBadStreak > 9)
		{
			badStreakBreak = true;
		}
		
		currentBadStreak = 0;
		
		if (currentGoodStreak > bestGoodStreak)
		{
			bestGoodStreak = currentGoodStreak;
		}
		
		totalLevelsCompleted++;
		individualLevelStats[lastLevelID].timesCompleted++;
		
		bestAttemptsLast = individualLevelStats[lastLevelID].bestAttempts;
		
		// Debug.Log("Recorded Time");
		
		if (individualLevelStats[lastLevelID].bestAttempts == 0 || individualLevelStats[lastLevelID].currentAttempts < individualLevelStats[lastLevelID].bestAttempts)
		{
			individualLevelStats[lastLevelID].bestAttempts = individualLevelStats[lastLevelID].currentAttempts;
		}
		
		bestTimeLast = individualLevelStats[lastLevelID].bestTime;
		
		if (individualLevelStats[lastLevelID].bestTime == 0 || secondsPlayedLast < individualLevelStats[lastLevelID].bestTime)
		{
			individualLevelStats[lastLevelID].bestTime = secondsPlayedLast;
			Debug.Log("Setting Level " + lastLevelID + "'s Best Time To " + secondsPlayedLast);
		}
		
		// Debug.Log(secondsPlayedLast);
		
		#if UNITY_STANDALONE || UNITY_EDITOR || UNITY_ANDROID || UNITY_WSA_10_0
			PlayerPrefsX.SetBool("Level"+levelsInQueue[lastLevelQueuePos].name+"Completed", true);
			
			PlayerPrefs.SetInt("TotalLevelsCompleted", totalLevelsCompleted);
			PlayerPrefs.SetInt("Level"+levelsInQueue[lastLevelQueuePos].name+"TimesCompleted", individualLevelStats[lastLevelID].timesCompleted);
			PlayerPrefs.SetInt("Level"+levelsInQueue[lastLevelQueuePos].name+"BestAttempts", individualLevelStats[lastLevelID].bestAttempts);
			
			PlayerPrefs.SetFloat("Level"+levelsInQueue[lastLevelQueuePos].name+"BestTime", individualLevelStats[lastLevelID].bestTime);
			
			PlayerPrefs.SetInt("CurrentGoodStreak", currentGoodStreak);
			PlayerPrefs.SetInt("CurrentBadStreak", currentBadStreak);
			PlayerPrefs.SetInt("bestGoodStreak", bestBadStreak);
		#endif
		
		#if UNITY_WEBGL && !UNITY_EDITOR
			string levelName = (levelsInQueue[lastLevelQueuePos].name.Remove(0,5)).TrimStart('0');
			int levelID = levelName.Length > 0 ? int.Parse(levelName) : 0;
			Application.ExternalCall("UpdateCompletedLevels", levelID, true);
		#endif
		
		levelsInQueue.RemoveAt(lastLevelQueuePos);
		
		levelsCompleted++;
		if (levelsCompleted < numberOfLevels)
		{
			return false;
		}else{
			return true;
		}
	}
	
	public static bool CheckComplete () {
		
		// levelsInQueue.RemoveAt(lastLevel);
		// Debug.Log(levelsInQueue[nextLevel]);
		// Debug.Log(levelsInQueue.Count);
		
		if (levelsInQueue.Count > 0)
		{
			return false;
		}else{
			return true;
		}
	}
	
	public static void Reset () {
		levelsInQueue = new List<GameObject>(Resources.LoadAll<GameObject>("prefabs"));
		numberOfLevels = levelsInQueue.Count;
		completedLevels = new bool[numberOfLevels];
		for (int i = 0; i < completedLevels.Length; i++)
		{
			
			individualLevelStats[i].currentAttempts = 0;
			individualLevelStats[i].secondsPlayedCurrent = 0;
			
			// PlayerPrefsX.SetBool("Level"+i+"Completed", false);
			#if UNITY_STANDALONE || UNITY_EDITOR || UNITY_ANDROID || UNITY_WSA_10_0
				PlayerPrefsX.SetBool("Level"+levelsInQueue[i].name+"Completed", false);
				PlayerPrefs.SetInt("Level"+levelsInQueue[i].name+"CurrentAttempts", individualLevelStats[i].currentAttempts);
				PlayerPrefs.SetFloat("Level"+levelsInQueue[i].name+"SecondsPlayedCurrent", individualLevelStats[i].secondsPlayedCurrent);
			#endif
	
			#if UNITY_WEBGL && !UNITY_EDITOR
				string levelName = (levelsInQueue[i].name.Remove(0,5)).TrimStart('0');
				int levelID = levelName.Length > 0 ? int.Parse(levelName) : 0;
				Application.ExternalCall("UpdateCompletedLevels", levelID, false);
			#endif
		}
		
		levelsCompleted = 0;
		currentAttempts = 0;
		timesStarted++;	
		secondsPlayedCurrent = 0;
		
		currentGoodStreak = 0;
		currentBadStreak = 0;
		
		#if UNITY_STANDALONE || UNITY_EDITOR || UNITY_ANDROID || UNITY_WSA_10_0
			PlayerPrefs.SetInt("CurrentAttempts", 0);
			PlayerPrefs.SetInt("TimesStarted", timesStarted);
			PlayerPrefs.SetFloat("SecondsPlayedCurrent", secondsPlayedCurrent);
			
			PlayerPrefs.SetInt("CurrentGoodStreak", currentGoodStreak);
			PlayerPrefs.SetInt("CurrentBadStreak", currentBadStreak);
		#endif

		#if UNITY_WEBGL && !UNITY_EDITOR
			Application.ExternalCall("UpdateCurrentAttempts", 0);
		#endif
		
		Messenger.Broadcast("UpdateColour");
	}
	
	public static void Completed () {
		
		if (bestAttempts == 0 || currentAttempts < bestAttempts)
		{
			bestAttempts = currentAttempts;
			
			#if UNITY_STANDALONE || UNITY_EDITOR || UNITY_ANDROID || UNITY_WSA_10_0
				PlayerPrefs.SetInt("BestAttempts", bestAttempts);
			#endif
			
			#if UNITY_WEBGL && !UNITY_EDITOR
				Application.ExternalCall("UpdateBestAttempts", bestAttempts);
			#endif
		}
		
		if (secondsPlayedBest == 0 || secondsPlayedCurrent < secondsPlayedBest)
		{
			secondsPlayedBest = secondsPlayedCurrent;
			
			#if UNITY_STANDALONE || UNITY_EDITOR || UNITY_ANDROID || UNITY_WSA_10_0
				PlayerPrefs.SetFloat("SecondsPlayedBest", secondsPlayedBest);
			#endif
			
			#if UNITY_WEBGL && !UNITY_EDITOR
				Application.ExternalCall("UpdateBestAttempts", secondsPlayedBest);
			#endif
		}
		
		timesCompleted++;
		
		#if UNITY_STANDALONE || UNITY_EDITOR || UNITY_ANDROID || UNITY_WSA_10_0
			PlayerPrefs.SetInt("TimesCompleted", timesCompleted);
		#endif
		
		#if UNITY_WEBGL && !UNITY_EDITOR
			Application.ExternalCall("UpdateTimesCompleted", timesCompleted);
		#endif
	}
	
	public static void AddToCurrentAttempts () {
		
		goodStreakBreak = false;
		badStreakBreak = false;
		
		currentAttempts++;
		totalAttempts++;
		
		individualLevelStats[lastLevelID].currentAttempts++;
		individualLevelStats[lastLevelID].totalAttempts++;
		
		#if UNITY_STANDALONE || UNITY_EDITOR || UNITY_ANDROID || UNITY_WSA_10_0
			PlayerPrefs.SetInt("CurrentAttempts", currentAttempts);
			PlayerPrefs.SetInt("TotalAttempts", totalAttempts);
			
			PlayerPrefs.SetInt("Level"+levelsInQueue[lastLevelQueuePos].name+"CurrentAttempts", individualLevelStats[lastLevelID].currentAttempts);
			PlayerPrefs.SetInt("Level"+levelsInQueue[lastLevelQueuePos].name+"TotalAttempts", individualLevelStats[lastLevelID].totalAttempts);
			
			// Debug.Log("Level"+levelsInQueue[lastLevel].name+"CurrentAttempts: " + individualLevelStats[lastLevelID].currentAttempts);
			// Debug.Log("Level"+levelsInQueue[lastLevel].name+"TotalAttempts: " + individualLevelStats[lastLevelID].totalAttempts);
			// Debug.Log("Level"+levelsInQueue[lastLevel].name+"TimesCompleted: " + individualLevelStats[lastLevelID].timesCompleted);
		#endif
		
		#if UNITY_WEBGL && !UNITY_EDITOR
			Application.ExternalCall("UpdateCurrentAttempts", currentAttempts);
		#endif
		
		// Debug.Log("ATTEMPTS - ID: " + lastLevelID + " lastLevelQueuePos: " + lastLevel);
	}
	
	public static void StartTimer () {
		instance.StartCoroutine(Timer());
		// Debug.Log("StartTimer");
	}
	
	public static void StopTimer () {
		// instance.StopAllCoroutines();
		
		if (timerRunning == true)
		{
			timerRunning = false;
			// Debug.Log("StopTimer");
			
			secondsPlayedTotal += timeValue;
			secondsPlayedCurrent += timeValue;
			
			individualLevelStats[lastLevelID].secondsPlayedCurrent += timeValue;
			individualLevelStats[lastLevelID].secondsPlayedTotal += timeValue;
			
			secondsPlayedLast = timeValue;
			// Debug.Log("Setting Seconds Last Played To " + timeValue);
			
			#if UNITY_STANDALONE || UNITY_EDITOR || UNITY_ANDROID || UNITY_WSA_10_0
				PlayerPrefs.SetFloat("SecondsPlayedTotal", secondsPlayedTotal);
				PlayerPrefs.SetFloat("SecondsPlayedCurrent", secondsPlayedCurrent);
						
				PlayerPrefs.SetFloat("Level"+levelsInQueue[lastLevelQueuePos].name+"SecondsPlayedCurrent", individualLevelStats[lastLevelID].secondsPlayedCurrent);
				PlayerPrefs.SetFloat("Level"+levelsInQueue[lastLevelQueuePos].name+"SecondsPlayedTotal", individualLevelStats[lastLevelID].secondsPlayedTotal);
			#endif
		}
	}
	
	public static IEnumerator Timer () {
		timeValue = 0;
		
		timerRunning = true;
		
		while (timerRunning == true)
		{
			timeValue += Time.deltaTime;
			yield return null;
		}
	}
	
	static int ConvertLevelNameToID (string levelName) {
		
		Debug.Log(levelName);
		
		string trimmedName = levelName.Remove(0,5).TrimStart('0');
		int ID = trimmedName.Length > 0 ? int.Parse(trimmedName) : 0;
			
		return ID;
	}
	
	// static bool CheckForLevelRepeats () {
		// return false;
	// }
	
	public static int GetLevelsCompleted () {
		return levelsCompleted;
	}
	
	public static int GetNumberOfLevels () {
		return numberOfLevels;
	}
	
	public static int GetNumberOfLevelsRemaining () {
		return numberOfLevels - levelsCompleted;
	}
	
	public static int GetCurrentAttempts () {
		return currentAttempts;
	}
	
	public static int GetBestAttempts () {
		return bestAttempts;
	}
	
	public static int GetTimesStarted () {
		return timesStarted;
	}
	
	public static int GetTimesCompleted () {
		return timesCompleted;
	}
	
	public static int GetTotalLevelsCompleted () {
		return totalLevelsCompleted;
	}
	
	public static int GetTotalAttempts () {
		return totalAttempts;
	}
	
	public static int GetNumberOfSessions () {
		return numberOfSessions;
	}
	
	public static int GetCurrentGoodStreak () {
		return currentGoodStreak;
	}
	
	public static int GetCurrentBadStreak () {
		return currentBadStreak;
	}
	
	public static int GetBestGoodStreak () {
		return bestGoodStreak;
	}
	
	public static int GetBestBadStreak () {
		return bestBadStreak;
	}
	
	public static bool GetGoodStreakBreak () {
		return goodStreakBreak;
	}
	
	public static bool GetBadStreakBreak () {
		return badStreakBreak;
	}
	
	public static float GetTotalSeconds () {
		return secondsPlayedTotal;
	}
	
	public static float GetCurrentSeconds () {
		return secondsPlayedCurrent;
	}
	
	public static float GetBestSeconds () {
		return secondsPlayedBest;
	}
	
	public static float GetLastSeconds () {
		return secondsPlayedLast;
	}
	
	public static float GetLastBestTime () {
		return bestTimeLast;
	}
	
	// returns difference between best and last time
	public static float GetLastTimeDifference () {
		return bestTimeLast - secondsPlayedLast;
	}
	
	public static float GetPercentageComplete () {
		return (float)levelsCompleted / (float)numberOfLevels;
	}
	
	public static int GetCurrentLevelCurrentAttempts () {
		return individualLevelStats[lastLevelID].currentAttempts;
	}
	
	public static int GetBestAttemptsLast () {
		return bestAttemptsLast;
	}
	
	public static int GetLastAttemptsDifference () {
		return bestAttemptsLast - individualLevelStats[lastLevelID].currentAttempts;
	}
	
	public static int GetCurrentLevelTotalAttempts () {
		return individualLevelStats[lastLevelID].totalAttempts;
	}
	
	public static int GetCurrentLevelTimesCompleted () {
		return individualLevelStats[lastLevelID].timesCompleted;
	}
	
	public static float GetCurrentLevelBestTime () {		
		return individualLevelStats[lastLevelID].bestTime;
	}
	
	public static int GetCurrentLevelBestAttempts () {
		return individualLevelStats[lastLevelID].bestAttempts;
	}
	
	public static float GetCurrentLevelCurrentSeconds () {
		return individualLevelStats[lastLevelID].secondsPlayedCurrent;
	}
	
	public static float GetCurrentLevelTotalSeconds () {
		return individualLevelStats[lastLevelID].secondsPlayedTotal;
	}
	
	#if UNITY_WEBGL
		
		void UpdateLevelStatesFromLocalStorage (string lS) {
			completedLevels[int.Parse(lS)] = true;
		}
		
		void UpdateCurrentAttempsFromLocalStorage (string cA) {
			currentAttempts = int.Parse(cA);
		}
		
		void UpdateBestAttemptsFromLocalStorage (string bA) {
			bestAttempts = int.Parse(bA);
		}
		
		void UpdateTimesCompletedFromLocalStorage (string tC) {
			timesCompleted = int.Parse(tC);
		}
	
	#endif
	
}
