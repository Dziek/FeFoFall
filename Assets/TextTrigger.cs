using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextTrigger : MonoBehaviour {
	
	public string textToAdd; // can makes it "" if I want to reset overrideText so it does normal text
	public OverrideCondition overrideCondition = OverrideCondition.none;
	public bool exitTriggerReset; // if you exit the trigger, reset overrideText
	
	private TransitionText transitionText;
	private bool active = true;
	
	// Use this for initialization
	void Start () {
		transitionText = GameObject.Find("TransitionControllers").GetComponent<TransitionText>();
		GetComponent<SpriteRenderer>().enabled = false;
		
		if (Application.loadedLevelName == "LevelTesting" || Application.loadedLevelName == "GraphicsTesting")
		{
			this.enabled = false;
		}
	}
	
	void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.tag == "Player" && active)
		{	
			SendTextToTransition(textToAdd);
			active = false;
		}	
	}
	
	void OnTriggerExit2D(Collider2D collision)
	{
		if (collision.tag == "Player" && active == false && exitTriggerReset == true)
		{	
			SendTextToTransition("");
			active = true;
		}	
	}
	
	void SendTextToTransition (string t) {
		transitionText.SetText(t, overrideCondition);
	}
}

public enum OverrideCondition {
	levelSuccess,
	levelFailure,
	none
}
