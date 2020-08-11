using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecoyEndPoint : MonoBehaviour {

	private LevelCompletePS levelCompletePS;
	
	private bool active = true;

	void Awake () {
		levelCompletePS = GameObject.Find("LevelCompletePS").GetComponent<LevelCompletePS>();
	}
	
	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.tag == "Player" && active)
		{
			levelCompletePS.Fire(transform.position);
			
			active = false;
			gameObject.SetActive(false);
		}
		
	}
	
	// private LevelCompletePS levelCompletePS;
	
	// private bool active = true;

	// void Awake () {
		// levelCompletePS = GameObject.Find("LevelCompletePS").GetComponent<LevelCompletePS>();
	// }
	
	// void OnTriggerEnter2D(Collider2D other)
	// {
		// if (other.tag == "Player" && active)
		// {
			// levelCompletePS.Fire(transform.position);
			
			// active = false;
			// gameObject.SetActive(false);
		// }
		
	// }
}
