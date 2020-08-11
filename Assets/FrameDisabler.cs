using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrameDisabler : MonoBehaviour {
	
	private GameObject frameParentGO;
		
	void Awake () {
		frameParentGO = GameObject.Find("FrameSprites");
	}	
		
	void OnEnable () {
		frameParentGO.SetActive(false);
	}
	
	void OnDisable () {
		frameParentGO.SetActive(true);
	}
}
