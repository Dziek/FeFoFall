using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LoadLevel : MonoBehaviour {
		 
	 static LoadLevel instance;
	
	public static List<GameObject> levelsInQueue = new List<GameObject>();
	static bool[] completedLevels;
	
	static GameObject currentLevel;
	
	static int nextLevelQueuePos;
	static int lastLevelQueuePos = -1;
	static int beforeLastLevelQueuePos = -1;
	
	static int lastLevelID = -1;
	
	public int activeRange; // how many levels can be 'active' at a time
	
	static int levelsCompleted;
	static int currentAttempts;
	static int numberOfLevels;
	static int timesCompleted;
	static int bestAttempts;
	static int totalAttempts;
	static int totalLevelsCompleted;
	
	public struct LevelStats {
		public int currentAttempts;
		public int totalAttempts;
		public int timesCompleted;
	}
	
	private static LevelStats[] individualLevelStats;
	
	void Awake () {
	
		// PlayerPrefs.DeleteAll();
		
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
				}else{
					levelsCompleted++;
				}
				
				individualLevelStats[i].currentAttempts = PlayerPrefs.GetInt("Level"+fullListOfLevels[i].name+"CurrentAttempts");
				individualLevelStats[i].totalAttempts = PlayerPrefs.GetInt("Level"+fullListOfLevels[i].name+"TotalAttempts");
				individualLevelStats[i].timesCompleted = PlayerPrefs.GetInt("Level"+fullListOfLevels[i].name+"TimesCompleted");
			}
			
			currentAttempts = PlayerPrefs.GetInt("CurrentAttempts");
			timesCompleted = PlayerPrefs.GetInt("TimesCompleted");
			bestAttempts = PlayerPrefs.GetInt("BestAttempts");
			totalAttempts = PlayerPrefs.GetInt("TotalAttempts");
			totalLevelsCompleted = PlayerPrefs.GetInt("TotalLevelsCompleted");
		#endif
		
		#if UNITY_WEBGL && !UNITY_EDITOR
			Application.ExternalCall("UpdateNumberOfLevels", numberOfLevels);
			Application.ExternalCall("GetStats");
			
			for (int i = 0; i < completedLevels.Length; i++)
			{
				if (completedLevels[i] == false)
				{
					levelsInQueue.Add(fullListOfLevels[i]);
				}else{
					levelsCompleted++;
				}
			}
		#endif
	}
	
	public static void GetLevel () {
		
		Debug.Log("GetLevel");
		
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
							Debug.Log("Set to early level override");
						}else{
							nextLevelQueuePos = Random.Range(0, instance.activeRange);
						}
					}else{
						nextLevelQueuePos = Random.Range(0, instance.activeRange);
					}
					
					Debug.Log("Finding suitable level " + "BeforeLast: " + beforeLastLevel.ToString() + 
								" Last: " + lastLevelQueuePos.ToString() + " Next: " + nextLevelQueuePos.ToString());
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
	
	public static bool LevelCompleted () {
		
		totalLevelsCompleted++;
		individualLevelStats[lastLevelID].timesCompleted++;
		
		#if UNITY_STANDALONE || UNITY_EDITOR || UNITY_ANDROID || UNITY_WSA_10_0
			PlayerPrefsX.SetBool("Level"+levelsInQueue[lastLevel].name+"Completed", true);
			
			PlayerPrefs.SetInt("TotalLevelsCompleted", totalLevelsCompleted);
			PlayerPrefs.SetInt("Level"+levelsInQueue[lastLevel].name+"TimesCompleted", individualLevelStats[lastLevelID].timesCompleted);
		#endif
		
		#if UNITY_WEBGL && !UNITY_EDITOR
			string levelName = (levelsInQueue[lastLevel].name.Remove(0,5)).TrimStart('0');
			int levelID = levelName.Length > 0 ? int.Parse(levelName) : 0;
			Application.ExternalCall("UpdateCompletedLevels", levelID, true);
		#endif
		
		levelsInQueue.RemoveAt(lastLevel);
		
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
			
			// PlayerPrefsX.SetBool("Level"+i+"Completed", false);
			#if UNITY_STANDALONE || UNITY_EDITOR || UNITY_ANDROID || UNITY_WSA_10_0
				PlayerPrefsX.SetBool("Level"+levelsInQueue[i].name+"Completed", false);
				PlayerPrefs.SetInt("Level"+levelsInQueue[i].name+"CurrentAttempts", individualLevelStats[i].currentAttempts);
			#endif
	
			#if UNITY_WEBGL && !UNITY_EDITOR
				string levelName = (levelsInQueue[i].name.Remove(0,5)).TrimStart('0');
				int levelID = levelName.Length > 0 ? int.Parse(levelName) : 0;
				Application.ExternalCall("UpdateCompletedLevels", levelID, false);
			#endif
		}
		
		levelsCompleted = 0;
		currentAttempts = 0;
		
		// difficultyStage = 0;
		
		// PlayerPrefs.SetInt("LevelsCompleted", 0);
		
		
		#if UNITY_STANDALONE || UNITY_EDITOR || UNITY_ANDROID || UNITY_WSA_10_0
			PlayerPrefs.SetInt("CurrentAttempts", 0);
		#endif

		#if UNITY_WEBGL && !UNITY_EDITOR
			Application.ExternalCall("UpdateCurrentAttempts", 0);
		#endif
		
		Messenger.Broadcast("UpdateColour");
	}
	
	public static void Completed () {
		timesCompleted++;
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
		
		#if UNITY_STANDALONE || UNITY_EDITOR || UNITY_ANDROID || UNITY_WSA_10_0
			PlayerPrefs.SetInt("TimesCompleted", timesCompleted);
		#endif
		
		#if UNITY_WEBGL && !UNITY_EDITOR
			Application.ExternalCall("UpdateTimesCompleted", timesCompleted);
		#endif
	}
	
	public static void AddToCurrentAttempts () {
		
		currentAttempts++;
		totalAttempts++;
		
		individualLevelStats[lastLevelID].currentAttempts++;
		individualLevelStats[lastLevelID].totalAttempts++;
		
		#if UNITY_STANDALONE || UNITY_EDITOR || UNITY_ANDROID || UNITY_WSA_10_0
			PlayerPrefs.SetInt("CurrentAttempts", currentAttempts);
			PlayerPrefs.SetInt("TotalAttempts", totalAttempts);
			
			PlayerPrefs.SetInt("Level"+levelsInQueue[lastLevel].name+"CurrentAttempts", individualLevelStats[lastLevelID].currentAttempts);
			PlayerPrefs.SetInt("Level"+levelsInQueue[lastLevel].name+"TotalAttempts", individualLevelStats[lastLevelID].totalAttempts);
			
			// Debug.Log("Level"+levelsInQueue[lastLevel].name+"CurrentAttempts: " + individualLevelStats[lastLevelID].currentAttempts);
			// Debug.Log("Level"+levelsInQueue[lastLevel].name+"TotalAttempts: " + individualLevelStats[lastLevelID].totalAttempts);
			// Debug.Log("Level"+levelsInQueue[lastLevel].name+"TimesCompleted: " + individualLevelStats[lastLevelID].timesCompleted);
		#endif
		
		#if UNITY_WEBGL && !UNITY_EDITOR
			Application.ExternalCall("UpdateCurrentAttempts", currentAttempts);
		#endif
		
		// Debug.Log("ATTEMPTS - ID: " + lastLevelID + " lastLevelQueuePos: " + lastLevel);
	}
	
	static int ConvertLevelNameToID (string levelName) {
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
	
	public static int GetCurrentAttempts () {
		return currentAttempts;
	}
	
	public static int GetBestAttempts () {
		return bestAttempts;
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
	
	public static float GetPercentageComplete () {
		return (float)levelsCompleted / (float)numberOfLevels;
	}
	
	public static int GetCurrentLevelCurrentAttempts () {
		return individualLevelStats[lastLevelID].currentAttempts;
	}
	
	public static int GetCurrentLevelTotalAttempts () {
		return individualLevelStats[lastLevelID].totalAttempts;
	}
	
	public static int GetCurrentLevelTimesCompleted () {
		return individualLevelStats[lastLevelID].timesCompleted;
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
