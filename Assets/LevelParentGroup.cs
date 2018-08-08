using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelParentGroup : MonoBehaviour {
	
	public Mode mode;
	public int activeRange = 5;
	
	public LevelRange levelRange;
	
	private StatsManager statsManager;
	
	// private List<GameObject> allLevels = new List<GameObject>();
	// private List<GameObject> currentLevels = new List<GameObject>();
	
	public TopLevelGroup thisGroup = new TopLevelGroup();
	
	void Awake () {
		LevelGroup[] childGroups = GetComponentsInChildren<LevelGroup>();
		
		thisGroup.allLevels = new List<GameObject>();
		
		for (int i = 0; i < childGroups.Length; i++)
		{
			for (int o = 0; o < childGroups[i].levels.Count; o++)
			{
				// Debug.Log(i + " " + o);
				thisGroup.allLevels.Add(childGroups[i].levels[o]);
			}
		}
		
		statsManager = GameObject.Find("StatsManager").GetComponent<StatsManager>();
		statsManager.AddLevelsByMode(thisGroup.allLevels, mode);
	}
	
	void Start () {
		statsManager = GetComponentInParent<LevelManager>().statsManager;
		
		foreach (GameObject level in thisGroup.allLevels)
		{
			if (statsManager.LevelLookUp(level.name).isCompleted == false)
			{
				thisGroup.currentLevels.Add(level);
			}
		}
		
		thisGroup.activeRange = activeRange;
		thisGroup.levelRange = levelRange;
		// thisGroup.allLevels = allLevels;
	}
	
	// void SortCurrentLevels () {
		// statsManager = GetComponentInParent<LevelManager>().statsManager;
		
		// foreach (GameObject level in thisGroup.allLevels)
		// {
			// if (statsManager.LevelLookUp(level.name).isCompleted == false)
			// {
				// thisGroup.currentLevels.Add(level);
			// }
		// }
		
		// thisGroup.activeRange = activeRange;
		// thisGroup.levelRange = levelRange;
		// // thisGroup.allLevels = allLevels;
	// }
	
	// public TopLevelGroup GetGroup () {
		// statsManager = GetComponentInParent<LevelManager>().statsManager;
		
		// foreach (GameObject level in thisGroup.allLevels)
		// {
			// if (statsManager.LevelLookUp(level.name).isCompleted == false)
			// {
				// thisGroup.currentLevels.Add(level);
			// }
		// }
		
		// thisGroup.activeRange = activeRange;
		// thisGroup.levelRange = levelRange;
		// // thisGroup.allLevels = allLevels;
	// }
}
