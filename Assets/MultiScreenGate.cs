using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiScreenGate : MonoBehaviour {
	
	public GameObject viewerGO;
	// public Vector2 direction = new Vector2(-1, 1);
	public float timeToTravel = 0.25f; // time it takes to move gate
	public float playerOffsetAmount = 3.5f; // moves player by this amount
	
	private GameObject playerGO;
	private PlayerControl playerScript;
	
	// void Start () {
		// playerGO = GameObject.Find("Player");
	// }
	
	void OnTriggerEnter2D (Collider2D other) {
		if (other.gameObject.tag == "Player")
		{
			StartCoroutine("Travel");
		}
	}
	
	IEnumerator Travel () {
		float t = 0;
		// float timeToTravel = ;
		
		Vector2 startPos = viewerGO.transform.position;
		// Vector2 endPos = new Vector2(viewerGO.transform.position.x * direction.x, viewerGO.transform.position.y * direction.y);
		Vector2 endPos = new Vector2(viewerGO.transform.position.x * -1, viewerGO.transform.position.y);
		
		playerScript.FreezeInput(true);
		
		while (t < timeToTravel)
		{
			t += Time.deltaTime;
			
			float lerpAmount = t / timeToTravel;
			
			viewerGO.transform.position = Vector2.Lerp(startPos, endPos, lerpAmount);
			
			yield return null;
		}
		
		viewerGO.transform.position = endPos;
		// playerGO.transform.position = (Vector2)playerGO.transform.position + Vector2.right * Vector2.Distance(startPos, endPos);
		
		float newX = playerOffsetAmount * -Mathf.Sign(transform.position.x);
		
		playerGO.transform.position = new Vector2(newX, playerGO.transform.position.y);
		
		playerScript.FreezeInput(false);
	}
	
	void SetPlayer (PlayerControl playerControl) {
		playerGO = playerControl.gameObject;
		playerScript = playerGO.GetComponent<PlayerControl>();
	}
	
	void OnEnable () {
		Messenger<PlayerControl>.AddListener("SetPlayer", SetPlayer);
	}
	
	void OnDisable () {
		Messenger<PlayerControl>.RemoveListener("SetPlayer", SetPlayer);
	}
}
