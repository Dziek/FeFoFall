using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MenuDisplay : MonoBehaviour {
	
	public Text progress;
	public Text currentAttempts;
	public Text timesCompleted;
	public Text bestAttempts;
	
	void OnEnable () {
		UpdateText();
	}
	
	public void UpdateText () {
		progress.text = LoadLevel.GetLevelsCompleted() + " / " + LoadLevel.GetNumberOfLevels();
		currentAttempts.text = "Current Attempts: " + LoadLevel.GetCurrentAttempts();
		if (LoadLevel.GetTimesCompleted() > 0)
		{
			timesCompleted.text = "Times Completed: " + LoadLevel.GetTimesCompleted();
			bestAttempts.text = "Best Attempts: " + LoadLevel.GetBestAttempts();
		}
	}
}
