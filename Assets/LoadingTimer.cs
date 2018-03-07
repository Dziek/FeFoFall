using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingTimer : MonoBehaviour {
	
	public Text timeText1;
	public Text timeText2;
	private float time;
	
	void Awake () {
		StartCoroutine("Timer");
	}
	
	void Start () {
	// void OnLevelWasLoaded () {
		StopCoroutine("Timer");
		
		timeText1.text = time.ToString() + " seconds";
		timeText2.text = Time.time.ToString() + " seconds";
	}
	
	IEnumerator Timer () {
		while (true)
		{
			time += Time.deltaTime;
			yield return null;
		}
	}
}
