using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CompletedMainDisplay : MonoBehaviour {
	
	public Text completedMsg;
	// public Text completedMsg2;
	// public string completedString;
	// static string cA;
	
	private string rawString;
	
	// private string[] cM;
	
	private ModeManager modeManager;
	
	// void OnAwake () {
		// cM = completedMsg.text.Split("-X-"[0]);
		// Debug.Log("A");
	// }
	
	void Awake () {
		rawString = completedMsg.text;
		
		modeManager = GameObject.Find("ModeManager").GetComponent<ModeManager>();
	}
	
	void OnEnable () {
		// string cA = GameObject.Find("StatsManager").GetComponent<StatsManager>().GetCurrentAttempts(Mode.Main).ToString();
		
		Mode currentMode = modeManager.GetMode();
		
		StatsManager statsManager = GameObject.Find("StatsManager").GetComponent<StatsManager>();
		
		string cA = statsManager.GetCurrentAttempts(currentMode).ToString();
		string cT = statsManager.GetCurrentSeconds(currentMode).ToString("f2");
		string aA = statsManager.GetAverageAttemptsPerLevel(currentMode).ToString("f2");
		string tS = (statsManager.GetModeTimesStarted(currentMode)-1).ToString();
		
		string newText = rawString;
		
		newText = newText.Replace("$", cA); // current attempts
		newText = newText.Replace("£", cT); // current time in seconds
		newText = newText.Replace("€", aA); // average attempts per level
		newText = newText.Replace("^", tS); // time started mode (minus 1)
		
		completedMsg.text = newText;
		
		// if (cM == null)
		// {
			// cM = completedMsg.text.Split("$"[0]);
		// }
		
		// completedMsg.text = cM[0] + cA + cM[1];
	}
	
	
}
