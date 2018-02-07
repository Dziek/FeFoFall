using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class StatsDisplay : MonoBehaviour {
	
	public Text progress;
	public Text currentAttempts;
	public Text timesCompleted;
	public Text bestAttempts;
	public Text totalAttempts;
	public Text totalLevelsCompleted;
	
	void OnEnable () {
		UpdateText();
	}
	
	public void UpdateText () {
		progress.text = "Level Progress: " + LoadLevel.GetLevelsCompleted() + " / " + LoadLevel.GetNumberOfLevels();
		currentAttempts.text = "Current Attempts: " + LoadLevel.GetCurrentAttempts();
		if (LoadLevel.GetTimesCompleted() > 0)
		{
			timesCompleted.text = "Times Completed: " + LoadLevel.GetTimesCompleted();
			bestAttempts.text = "Best Attempts: " + LoadLevel.GetBestAttempts();
			totalAttempts.text = "Total Attempts: " + LoadLevel.GetTotalAttempts();
			totalLevelsCompleted.text = "Total Levels Completed: " + LoadLevel.GetTotalLevelsCompleted();
		}else{
			timesCompleted.text = "";
			bestAttempts.text = "";
			totalAttempts.text = "";
			totalLevelsCompleted.text = "";
		}
	}
}
