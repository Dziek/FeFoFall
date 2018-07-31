using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour {
	
	public StatsManager statsManager;
	public ModeManager modeManager;
	// public LevelChapter levelChapter;
	
	// private List<GameObject> allLevels = new List<GameObject>();
	// private List<GameObject> currentLevels = new List<GameObject>();
	// private int activeRange = 5;
	
	private List<TopLevelGroup> modeGroups = new List<TopLevelGroup>();
	
	private GameObject lastLevelGO;
	private GameObject lastLastLevelGO;
	private GameObject currentLevelGO;
	
	void Start () {
		// allLevels = levelChapter.levels;
		
		// foreach (GameObject level in allLevels)
		// {
			// if (statsManager.LevelLookUp(level.name).isCompleted == false)
			// {
				// modeGroups[(int)modeManager.GetMode()].currentLevels.Add(level);
			// }
		// }
		
		LevelParentGroup[] levelGroups = GetComponentsInChildren<LevelParentGroup>();
		for (int i = 0; i < levelGroups.Length; i++)
		{
			modeGroups.Add(levelGroups[i].thisGroup);
		}
	}
	
	//checks if game is over
	public bool CheckForGameOver () {
		// Debug.Log(modeGroups[(int)modeManager.GetMode()].currentLevels.Count);
		if (modeGroups[(int)modeManager.GetMode()].currentLevels.Count == 0)
		{
			Debug.Log("GameOver");
			return true;
		}
		
		return false;
	}
	
	public void LevelCompleted () {
		// Debug.Log(modeGroups[(int)modeManager.GetMode()].currentLevels.Count);
		modeGroups[(int)modeManager.GetMode()].currentLevels.Remove(lastLevelGO);
		// Debug.Log(modeGroups[(int)modeManager.GetMode()].currentLevels.Count);
	}
	
	public void ClearLevel () {
		if (currentLevelGO != null)
		{
			Destroy(currentLevelGO);
		}
	}
	
	public void SwitchLevel () {
		Debug.Log("Switching Level");
		// Debug.Break();
		
		// if (CheckForGameOver())
		// {
			// return;
		// }
		
		if (currentLevelGO != null)
		{
			Destroy(currentLevelGO);
		}
		
		// THIS seems unnecessary, I don't think SwitchLevel gets called becaused of an earlier CheckForGameOver
		if (CheckForGameOver())
		{
			return;
		}
		
		// could just Clamp the below, make it one line
		// int range = modeGroups[(int)modeManager.GetMode()].activeRange;
		
		// if (modeGroups[(int)modeManager.GetMode()].currentLevels.Count < 5)
		// {
			// range = modeGroups[(int)modeManager.GetMode()].currentLevels.Count;
		// }
		
		int range = CalculateCurrentRange();
		Debug.Log("Range = " + range);
		
		// 20% chance of choosing the first level in the queue
		GameObject levelGO = Random.Range(0,5) == 0 ? modeGroups[(int)modeManager.GetMode()].currentLevels[0] : modeGroups[(int)modeManager.GetMode()].currentLevels[Random.Range(0, range)];
		
		int g = 0;
		while ((levelGO == lastLevelGO || levelGO == lastLastLevelGO) && range > 2)
		{
			// Debug.Log("Looping");
			levelGO = modeGroups[(int)modeManager.GetMode()].currentLevels[Random.Range(0, range)];
			
			g++;
			
			if (g > 4)
			{
				break;
			}
		}
		
		g = 0;
		while (levelGO == lastLevelGO && range > 1)
		{
			// Debug.Log("Looping");
			levelGO = modeGroups[(int)modeManager.GetMode()].currentLevels[Random.Range(0, range)];
			
			g++;
			
			if (g > 20)
			{
				break;
			}
		}
		
		lastLastLevelGO = lastLevelGO;
		lastLevelGO = levelGO;
		currentLevelGO = Instantiate(levelGO);
		
		Messenger<GameObject>.Broadcast("NewLevel", levelGO);
	}
	
	public void ResetModeLevels () {
		
		Mode currentMode = modeManager.GetMode();
		
		statsManager.ClearModeStats(currentMode);
		modeGroups[(int)currentMode].ResetLevels();
		
		// Debug.Log(modeGroups[(int)currentMode].currentLevels.Count);
	}
	
	void OnEnable () {
		Messenger.AddListener("BackToMenu", ClearLevel);
	}
	
	void OnDisable () {
		Messenger.RemoveListener("BackToMenu", ClearLevel);
	}
	
	int CalculateCurrentRange () {
		
		int range = 0;
		
		if (modeGroups[(int)modeManager.GetMode()].levelRange.rangeTiers.Length == 0)
		{
			range = modeGroups[(int)modeManager.GetMode()].levelRange.constantRange;
			// Debug.Log("Setting to constant range");
		}else{
			for (int i = 0; i < modeGroups[(int)modeManager.GetMode()].levelRange.rangeTiers.Length; i++)
			{
				// set it to value
				range = modeGroups[(int)modeManager.GetMode()].levelRange.rangeTiers[i].range;
				
				if (statsManager.GetLevelsCompleted(modeManager.GetMode()) <= modeGroups[(int)modeManager.GetMode()].levelRange.rangeTiers[i].progress)
				{
					// range = modeGroups[(int)modeManager.GetMode()].levelRange.rangeTiers[i].range;
					// Debug.Log("Setting to tier: " + i);
					
					// break out if necessary
					break;
				}
			}
		}
		
		// if range is zero, set it to all levels
		if (range == 0)
		{
			range = statsManager.GetNumberOfLevelsRemaining(modeManager.GetMode());	
		}
		
		// could just Clamp the below, make it one line
		if (modeGroups[(int)modeManager.GetMode()].currentLevels.Count < range)
		{
			range = modeGroups[(int)modeManager.GetMode()].currentLevels.Count;
		}
		
		return range;
	}
}

public class TopLevelGroup {
	public int activeRange;
	public LevelRange levelRange;
	public List<GameObject> currentLevels = new List<GameObject>();
	public List<GameObject> allLevels = new List<GameObject>();
	
	public void ResetLevels () {
		currentLevels = allLevels;
	}
}

[System.Serializable]
public class LevelRange {
	
	/*
		so this idea of this is to have tiered active ranges. 
		So, for example, if levels completed (progress) is 0, range is 1. 
		This would mean that the first level would always play first.
		Range could then be opened up later, and shrink towards end game if needed.
		
		In case the tiers aren't used, a constant range can be set
	*/
	
	[System.Serializable]
	public struct RangeTier {
		public int progress;
		public int range;
	}
	
	public RangeTier[] rangeTiers;
	public int constantRange;
	
	public int currentActiveRange;
}