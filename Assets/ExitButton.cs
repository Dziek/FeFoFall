using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitButton : MonoBehaviour {
	
	void Awake () {
		#if UNITY_EDITOR || UNITY_ANDROID || UNITY_WSA_10_0 || UNITY_WEBGL
			gameObject.SetActive(false);
		#endif
	}
	
	public void Exit () {
		Application.Quit();
	}
}
