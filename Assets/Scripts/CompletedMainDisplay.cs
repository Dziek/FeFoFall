using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CompletedMainDisplay : MonoBehaviour {
	
	public Text completedMsg;
	// public Text completedMsg2;
	public string completedString;
	// static string cA;
	
	private string[] cM;
	
	// void OnAwake () {
		// cM = completedMsg.text.Split("-X-"[0]);
		// Debug.Log("A");
	// }
	
	void OnEnable () {
		// string cA = GameObject.Find("StatsManager").GetComponent<StatsManager>().GetCurrentAttempts(Mode.Main).ToString();
		string cA = GameObject.Find("StatsManager").GetComponent<StatsManager>().GetCurrentAttempts(GameObject.Find("ModeManager").GetComponent<ModeManager>().GetMode()).ToString();
		// string cA = LoadLevel.GetCurrentAttempts().ToString();
		// completedMsg.text = completedString.Replace("-X-", cA);
		
		// if (cM.Length == 0)
		if (cM == null)
		{
			cM = completedMsg.text.Split("$"[0]);
		}
		
		// string[] cM = completedMsg.text.Split("?X"[0]);
		completedMsg.text = cM[0] + cA + cM[1];
		// Debug.Log(cM.Length);
		// Debug.Log(cM[0]);
		
		// Debug.Log("OE");
	}
	
	
}
