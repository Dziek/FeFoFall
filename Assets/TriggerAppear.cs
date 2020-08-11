using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerAppear : MonoBehaviour {
	
	public bool active;
	
	private Collider2D collider;
	private GameObject graphicsGO;
	
	void Start () {
		
		if (GetComponent<Collider2D>() != null)
		{
			collider = GetComponent<Collider2D>();
		}
		
		// sometimes this won't work, will deal with it when and if that happens
		graphicsGO = transform.GetChild(0).gameObject;
		
		UpdateObject();
	}
	
	void UpdateObject () {
		if (GetComponent<Collider2D>() != null)
		{
			collider.enabled = active;
		}
		
		graphicsGO.SetActive(active);
	}
	
	public void WaitForTrigger () {
		// something has to be here?
		Debug.Log("Can't Wait Trigger Appear");
	}
	
	public void TriggerActivated (float time) {
		
		// Debug.Log("Trigger Activated");
		active = !active;
		UpdateObject();
	}
}
