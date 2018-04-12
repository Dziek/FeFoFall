using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewLevelLoad : MonoBehaviour {
	
	public string[] levelFolderNames = new string[] {"Unused"};
	public int activeRange = 5; // how many levels can be 'active' at a time
	
	public static List<GameObject> levelsInQueue = new List<GameObject>();
	
	void Awake () {
		FetchLevels();
	}
	
	public GameObject GetNewLevel () {
		int i = Random.Range(0, levelsInQueue.Count);
		
		return levelsInQueue[i];
	}
	
	void FetchLevels () {
		levelsInQueue = new List<GameObject>(Resources.LoadAll<GameObject>("prefabs"));
		
		
		
		
		// List<GameObject> fullListOfLevels = new List<GameObject>(Resources.LoadAll<GameObject>("prefabs"));
		// numberOfLevels = fullListOfLevels.Count;
		
		// for (int i = 0; i < numberOfLevels; i++)
		// {
			// if (PlayerPrefsX.GetBool("Level"+fullListOfLevels[i].name+"Completed") == false)
			// {
				// levelsInQueue.Add(fullListOfLevels[i]);
			// }else{
				// levelsCompleted++;
			// }
		// }
	}
}
