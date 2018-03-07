using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class StatsDisplay : MonoBehaviour {
	
	public Text progress;
	public Text currentAttempts;
	public Text timesStarted;
	public Text timesCompleted;
	public Text bestAttempts;
	public Text totalAttempts;
	public Text totalLevelsCompleted;
	
	public Text secondsSpendCurrent;
	public Text secondsSpendTotal;
	public Text secondsSpendBest;
	
	void OnEnable () {
		UpdateText();
	}
	
	public void UpdateText () {
		progress.text = "Level Progress: " + LoadLevel.GetLevelsCompleted() + " / " + LoadLevel.GetNumberOfLevels();
		currentAttempts.text = "Current Attempts: " + LoadLevel.GetCurrentAttempts();
		
		secondsSpendCurrent.text = "Current Seconds: " + LoadLevel.GetCurrentSeconds().ToString("f2");
		
		if (LoadLevel.GetTimesStarted() > 1)
		{
			timesStarted.text = "Times Started: " + LoadLevel.GetTimesStarted();
			timesCompleted.text = "Times Completed: " + LoadLevel.GetTimesCompleted();
			bestAttempts.text = "Best Attempts: " + LoadLevel.GetBestAttempts();
			totalAttempts.text = "Total Attempts: " + LoadLevel.GetTotalAttempts();
			totalLevelsCompleted.text = "Total Levels Completed: " + LoadLevel.GetTotalLevelsCompleted().ToString("f2");
			
			secondsSpendTotal.text = "Total Seconds: " + LoadLevel.GetTotalSeconds();
			secondsSpendBest.text = "Best Seconds: " + LoadLevel.GetBestSeconds();
		}else{
			timesStarted.text = "";
			timesCompleted.text = "";
			bestAttempts.text = "";
			totalAttempts.text = "";
			totalLevelsCompleted.text = "";
			secondsSpendTotal.text = "";
			secondsSpendBest.text = "";
		}
	}
}
