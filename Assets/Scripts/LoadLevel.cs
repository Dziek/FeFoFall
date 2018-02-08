using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LoadLevel : MonoBehaviour {
	
	 // [System.Serializable]
	 // public class LevelClusterInfo
	 // {
		 // public int[] levelInfo = new int[2];
	 
	 // }
	 
	 // public LevelClusterInfo[] levelClusterInfo;
	 
	 static LoadLevel instance;
	
	// public int[][] levelClusterInfo;
	
	// public static GameObject[] levels;
	// public static List<GameObject> levels;
	public static List<GameObject> levels = new List<GameObject>();
	// public static List<bool> completedLevels;
	static bool[] completedLevels;
	
	static GameObject currentLevel;
	
	static int nextLevel;
	static int lastLevel = -1;
	static int beforeLastLevel = -1;
	
	// static int difficultyStage; // plugs into levelClusterInfo to work out when later levels can load
	public int activeRange; // how many levels can be 'active' at a time
	// static int activeLevelCap; // levels
	
	static int levelsCompleted;
	static int currentAttempts;
	static int numberOfLevels;
	static int timesCompleted;
	static int bestAttempts;
	static int totalAttempts;
	static int totalLevelsCompleted;
	
	void Awake () {
	
		// PlayerPrefs.DeleteAll();
		
		instance = this;
		
		// Debug.Log(levelClusterInfo[0].levelInfo[1]);
		
		// levels = new List<GameObject>();
		// levels = Resources.LoadAll<GameObject>("prefabs") as List<GameObject>;
		List<GameObject> listOfLevels = new List<GameObject>(Resources.LoadAll<GameObject>("prefabs"));
		numberOfLevels = listOfLevels.Count;
		
		completedLevels = new bool[numberOfLevels];
		
		// int activeLevelCap = activeRange;
		#if UNITY_STANDALONE || UNITY_EDITOR || UNITY_ANDROID || UNITY_WSA_10_0
			for (int i = 0; i < completedLevels.Length; i++)
			// for (int i = 0; i < activeLevelCap; i++)
			{
				// if (PlayerPrefsX.GetBool("Level"+i+"Completed"))
				if (PlayerPrefsX.GetBool("Level"+listOfLevels[i]+"Completed") == false)
				{
					// Debug.Log("");
					// levels.RemoveAt(i);
					// levelsCompleted++;
					
					levels.Add(listOfLevels[i]);
				}else{
					levelsCompleted++;
					// activeLevelCap++;
				}
			}
			
			// for (int j = 0; j < levelClusterInfo.Length; j++)
			// {
				// // if (levelsCompleted >= LoadLevel.instance.levelClusterInfo[difficultyStage].levelInfo[0])
				// if (levelsCompleted >= levelClusterInfo[difficultyStage].levelInfo[0])
				// {
					// difficultyStage++;
				// }
			// }
			
			// levelClusterInfo[levelClusterInfo.Length-1].levelInfo[1] = numberOfLevels;
			
			// levelsCompleted = PlayerPrefs.GetInt("LevelsCompleted");
			currentAttempts = PlayerPrefs.GetInt("CurrentAttempts");
			// numberOfLevels = PlayerPrefs.GetInt("Number");
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
					levels.Add(listOfLevels[i]);
				}else{
					levelsCompleted++;
				}
			}
		#endif
	}
	
	// public void GetLevelFromClick () {
		// if (currentLevel != null)
		// {
			// Destroy(currentLevel);
		// }
		
		// while (nextLevel == lastLevel) 
		// {
			// nextLevel = Random.Range(0, levels.Count);
		// }
		
		// currentLevel = Instantiate(levels[nextLevel], transform.position, transform.rotation) as GameObject;
		
		// lastLevel = nextLevel;
	// }
	
	public static void GetLevel () {
		// if (c == "Good")
		// {
			// levels.RemoveAt(lastLevel);
		// }
		// currentAttempts++;
		
		Debug.Log("GetLevel");
		
		if (currentLevel != null)
		{
			Destroy(currentLevel);
		}
		
		if (levels.Count == 0)
		{
			// GameStates.ChangeState("Complete");
			nextLevel = -1;
			Debug.Log("D");
			// return null;
		}else if (levels.Count == 1)
		{
			nextLevel = 0;
		}else if (levels.Count == 2)
		{
			nextLevel = Random.Range(0, 2);
		}else if (levels.Count > 2){
			while (nextLevel == lastLevel || nextLevel == beforeLastLevel) 
			{	
				if (levels.Count < instance.activeRange)
				{
					nextLevel = Random.Range(0, levels.Count);
				}else{
				
					string lowLevelName = (levels[0].name.Remove(0,5)).TrimStart('0');
					int lowLevelID = lowLevelName.Length > 0 ? int.Parse(lowLevelName) : 0;
					string highLevelName = (levels[instance.activeRange-1].name.Remove(0,5)).TrimStart('0');
					int highLevelID = highLevelName.Length > 0 ? int.Parse(highLevelName) : 0;
					
					if ((highLevelID - lowLevelID) > 10 && lastLevel != 0)
					{
						int r = Random.Range(0,2);
						if (r == 0)
						{
							nextLevel = 0;
							Debug.Log("Set to early level override");
						}else{
							nextLevel = Random.Range(0, instance.activeRange);
						}
					}else{
						nextLevel = Random.Range(0, instance.activeRange);
					}
					
					Debug.Log("Finding suitable level " + "BeforeLast: " + beforeLastLevel.ToString() + 
								" Last: " + lastLevel.ToString() + " Next: " + nextLevel.ToString());
				}
				
					// nextLevel = Random.Range(0, instance.activeRange);
				// }
				// nextLevel = Random.Range(0, LoadLevel.instance.levelClusterInfo[difficultyStage].levelInfo[1]);
			}
		}
		
		if (nextLevel == lastLevel)
		{
			Debug.Log("Well that shouldn't happen");
			Debug.Break();
		}
		
		if (nextLevel != -1)
		{
			currentLevel = Instantiate(levels[nextLevel], Vector3.zero, Quaternion.identity) as GameObject;
		}
		
		beforeLastLevel = lastLevel;
		lastLevel = nextLevel;
	}
	
	public static void ClearLevel () {
		if (currentLevel != null)
		{
			Destroy(currentLevel);
		}
	}
	
	public static bool LevelCompleted () {
		
		// Debug.Log(levels[lastLevel]);
		// levels.RemoveAt(lastLevel);
		#if UNITY_STANDALONE || UNITY_EDITOR || UNITY_ANDROID || UNITY_WSA_10_0
			PlayerPrefsX.SetBool("Level"+levels[lastLevel]+"Completed", true);
		#endif
		
		#if UNITY_WEBGL && !UNITY_EDITOR
			string levelName = (levels[lastLevel].name.Remove(0,5)).TrimStart('0');
			int levelID = levelName.Length > 0 ? int.Parse(levelName) : 0;
			Application.ExternalCall("UpdateCompletedLevels", levelID, true);
		#endif
		
		levels.RemoveAt(lastLevel);
		
		// Debug.Log(levels.Count);
		
		levelsCompleted++;
		totalLevelsCompleted++;
		
		#if UNITY_STANDALONE || UNITY_EDITOR || UNITY_ANDROID || UNITY_WSA_10_0
				PlayerPrefs.SetInt("TotalLevelsCompleted", totalLevelsCompleted);
		#endif
		
		// if (levelsCompleted >= LoadLevel.instance.levelClusterInfo[difficultyStage].levelInfo[0])
		// {
			// difficultyStage++;
		// }
		
		// PlayerPrefs.SetInt("LevelsComple
		
		// Debug.Log("Broadcast");
		
		// if (levels.Count > 0)
		if (levelsCompleted < numberOfLevels)
		{
			return false;
		}else{
			return true;
		}
	}
	
	public static bool CheckComplete () {
		
		// levels.RemoveAt(lastLevel);
		// Debug.Log(levels[nextLevel]);
		// Debug.Log(levels.Count);
		
		if (levels.Count > 0)
		{
			return false;
		}else{
			return true;
		}
	}
	
	public static void Reset () {
		levels = new List<GameObject>(Resources.LoadAll<GameObject>("prefabs"));
		numberOfLevels = levels.Count;
		completedLevels = new bool[numberOfLevels];
		for (int i = 0; i < completedLevels.Length; i++)
		{
			// PlayerPrefsX.SetBool("Level"+i+"Completed", false);
			#if UNITY_STANDALONE || UNITY_EDITOR || UNITY_ANDROID || UNITY_WSA_10_0
				PlayerPrefsX.SetBool("Level"+levels[i]+"Completed", false);
			#endif
	
			#if UNITY_WEBGL && !UNITY_EDITOR
				string levelName = (levels[i].name.Remove(0,5)).TrimStart('0');
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
		// Reset();
	}
	
	public static int GetLevelsCompleted () {
		return levelsCompleted;
	}
	
	public static int GetNumberOfLevels () {
		return numberOfLevels;
	}
	
	public static void AddToCurrentAttempts () {
		currentAttempts++;
		totalAttempts++;
		#if UNITY_STANDALONE || UNITY_EDITOR || UNITY_ANDROID || UNITY_WSA_10_0
			PlayerPrefs.SetInt("CurrentAttempts", currentAttempts);
			PlayerPrefs.SetInt("TotalAttempts", totalAttempts);
		#endif
		
		#if UNITY_WEBGL && !UNITY_EDITOR
			Application.ExternalCall("UpdateCurrentAttempts", currentAttempts);
		#endif
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
