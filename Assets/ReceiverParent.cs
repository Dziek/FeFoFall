using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReceiverParent : MonoBehaviour {
	
	public void WaitForTrigger () {
		for (int i = 0; i < transform.childCount; i++)
		{
			transform.GetChild(i).gameObject.SendMessage("WaitForTrigger");
		}
	}
	
	public void TriggerActivated (float time) {
		TriggerAll(time);
	}
	
	void TriggerAll (float time) {
		for (int i = 0; i < transform.childCount; i++)
		{
			transform.GetChild(i).gameObject.SendMessage("TriggerActivated", time);
		}
	}
}
