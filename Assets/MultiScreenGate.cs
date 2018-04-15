using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiScreenGate : MonoBehaviour {
	
	public GameObject viewerGO;
	
	private GameObject playerGO;
	
	void Start () {
		playerGO = GameObject.Find("Player");
	}
	
	void OnTriggerEnter2D (Collider2D other) {
		if (other.gameObject.tag == "Player")
		{
			StartCoroutine("Travel");
		}
	}
	
	IEnumerator Travel () {
		float t = 0;
		float timeToTravel = 0.25f;
		
		Vector2 startPos = viewerGO.transform.position;
		Vector2 endPos = new Vector2(viewerGO.transform.position.x * -1, viewerGO.transform.position.y);
		
		while (t < timeToTravel)
		{
			t += Time.deltaTime;
			
			float lerpAmount = t / timeToTravel;
			
			viewerGO.transform.position = Vector2.Lerp(startPos, endPos, lerpAmount);
			
			yield return null;
		}
		
		viewerGO.transform.position = endPos;
		// playerGO.transform.position = (Vector2)playerGO.transform.position + Vector2.right * Vector2.Distance(startPos, endPos);
		
		float newX = 3.5f * -Mathf.Sign(transform.position.x);
		
		playerGO.transform.position = new Vector2(newX, playerGO.transform.position.y);
	}
}
