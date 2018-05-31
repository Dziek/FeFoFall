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
	
	private StatsManager statsManager;
	private ModeManager modeManager;
	
	void Awake () {
		statsManager = GameObject.Find("StatsManager").GetComponent<StatsManager>();
		modeManager = GameObject.Find("ModeManager").GetComponent<ModeManager>();
	}
	
	void OnEnable () {
		Messenger.AddListener("ModeUpdated", UpdateText);
		
		// UpdateText();
	}
	
	void OnDisable () {
		Messenger.RemoveListener("ModeUpdated", UpdateText);
	}
	
	public void UpdateText () {
		
		Mode currentMode = modeManager.GetMode();
		
		progress.text = "Level Progress: " + statsManager.GetLevelsCompleted(currentMode) + " / " + statsManager.GetNumberOfLevels(currentMode);
		currentAttempts.text = "Current Attempts: " + statsManager.GetCurrentAttempts(currentMode);
		
		secondsSpendCurrent.text = "Current Seconds: " + statsManager.GetCurrentSeconds(currentMode).ToString("f2");
		
		if (statsManager.GetModeTimesStarted(currentMode) > 1)
		{
			timesStarted.text = "Times Started: " + statsManager.GetModeTimesStarted(currentMode);
			timesCompleted.text = "Times Completed: " + statsManager.GetModeTimesCompleted(currentMode);
			bestAttempts.text = "Best Attempts: " + statsManager.GetModeBestAttempts(currentMode);
			totalAttempts.text = "Total Attempts: " + statsManager.GetModeTotalAttempts(currentMode);
			totalLevelsCompleted.text = "Total Levels Completed: " + statsManager.GetModeTotalLevelsCompleted(currentMode).ToString();
			
			secondsSpendTotal.text = "Total Seconds: " + statsManager.GetModeTotalSeconds(currentMode).ToString("f2");
			secondsSpendBest.text = "Best Seconds: " + statsManager.GetModeBestSeconds(currentMode).ToString("f2");
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
