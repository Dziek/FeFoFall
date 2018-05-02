using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour {
	
	public TestLevelScript tLS;
	
	private List<GameObject> allLevels = new List<GameObject>();
	private List<GameObject> currentLevels = new List<GameObject>();
	private int activeRange = 5;
	
	private GameObject lastLevelGO;
	private GameObject currentLevelGO;
	
	void Awake () {
		currentLevels = tLS.levels;
	}
	
	//checks if game is over
	public bool CheckForGameOver () {
		if (currentLevels.Count <= 1)
		{
			Debug.Log("GameOver");
			return true;
		}
		
		return false;
	}
	
	public void LevelCompleted () {
		Debug.Log(currentLevels.Count);
		currentLevels.Remove(lastLevelGO);
		Debug.Log(currentLevels.Count);
	}
	
	public void SwitchLevel () {
		
		CheckForGameOver();
		
		if (currentLevelGO != null)
		{
			Destroy(currentLevelGO);
		}
		
		// could just Clamp the below, make it one line
		int range = activeRange;
		
		if (currentLevels.Count < 5)
		{
			range = currentLevels.Count;
		}
		
		GameObject levelGO = currentLevels[Random.Range(0, range)];
		while (levelGO == lastLevelGO && range > 1)
		{
			levelGO = currentLevels[Random.Range(0, range)];
		}
		
		lastLevelGO = levelGO;
		currentLevelGO = Instantiate(levelGO);
	}
	
	// private GameObject GetLevel () {
		
	// }
}
