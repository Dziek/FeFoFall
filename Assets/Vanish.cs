using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vanish : MonoBehaviour {
	
	private GameObject graphicsGO;
	
	// Use this for initialization
	void Start () {
		graphicsGO = transform.GetChild(0).gameObject;
	}
	
	public void TriggerActivated (float time) {
		// timer = 0;
		Disappear();
		if (time != 0)
		{
			// StartCoroutine("Timer", time);
			Debug.Log("Timer not set up!");
		}
	}
	
	public void WaitForTrigger () {
		// waitForTrigger = true;
	}
	
	void Disappear () {
		graphicsGO.SetActive(false);
		gameObject.layer = LayerMask.NameToLayer("IgnoreShadow");
	}
}
