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
	
	// void OnAwake () {
		// cM = completedMsg.text.Split("-X-"[0]);
		// Debug.Log("A");
	// }
	
	void Awake () {
		rawString = completedMsg.text;
	}
	
	void OnEnable () {
		// string cA = GameObject.Find("StatsManager").GetComponent<StatsManager>().GetCurrentAttempts(Mode.Main).ToString();
		
		StatsManager statsManager = GameObject.Find("StatsManager").GetComponent<StatsManager>();
		
		string cA = statsManager.GetCurrentAttempts(GameObject.Find("ModeManager").GetComponent<ModeManager>().GetMode()).ToString();
		string cT = statsManager.GetCurrentSeconds(GameObject.Find("ModeManager").GetComponent<ModeManager>().GetMode()).ToString("f2");
		string aA = statsManager.GetAverageAttemptsPerLevel(GameObject.Find("ModeManager").GetComponent<ModeManager>().GetMode()).ToString("f2");
		
		string newText = rawString;
		
		newText = newText.Replace("$", cA);
		newText = newText.Replace("£", cT);
		newText = newText.Replace("€", aA);
		
		completedMsg.text = newText;
		
		// if (cM == null)
		// {
			// cM = completedMsg.text.Split("$"[0]);
		// }
		
		// completedMsg.text = cM[0] + cA + cM[1];
	}
	
	
}
