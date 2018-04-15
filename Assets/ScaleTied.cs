using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleTied : MonoBehaviour {
	
	public GameObject playerGO;
	
	// Use this for initialization
	void Start () {
		playerGO = GameObject.Find("Player");
		// playerGO.transform.position = Vector2.one;
		
		// Invoke("DelayedStart", 0.1f);
	}
	
	// void DelayedStart () {
		// playerGO = GameObject.Find("Player");
	// }
	
	// Update is called once per frame
	void Update () {
		transform.localScale = new Vector2(playerGO.transform.position.x, 1);
	}
}
