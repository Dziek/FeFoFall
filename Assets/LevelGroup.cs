using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGroup : MonoBehaviour {
	
	public string folderName = "C0";
	public List<GameObject> levels = new List<GameObject>();	
	
	public void SyncList () {
		List<GameObject> resourcesLevels = new List<GameObject>(Resources.LoadAll<GameObject>("Levels/" + folderName));
		
		// remove levels that aren't in Resources
		for (int i = 0; i < levels.Count; i++)
		{
			if (resourcesLevels.Contains(levels[i]) == false)
			{
				levels.Remove(levels[i]);
			}
		}
		
		// add levels from Resources
		for (int i = 0; i < resourcesLevels.Count; i++)
		{
			if (levels.Contains(resourcesLevels[i]) == false)
			{
				levels.Add(resourcesLevels[i]);
			}
		}
	}
}
