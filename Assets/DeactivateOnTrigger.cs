using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeactivateOnTrigger : MonoBehaviour {

	public void WaitForTrigger () {
		
	}
	
	public void TriggerActivated (float time) {
		gameObject.SetActive(false);
		
		if (time != 0)
		{
			Debug.Log("Timer not set up!");
		}
	}
}
