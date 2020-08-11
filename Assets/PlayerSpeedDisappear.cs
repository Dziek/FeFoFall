using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpeedDisappear : MonoBehaviour {
	
	public float topSpeed = 10;
	public float bottomSpeed = 5;
	
	public bool startActive = true;
	
	private bool active;
	private float lastFrameSpeedPercentage;
	
	private GameObject playerGO;
	private PlayerControl playerScript;
	
	private BoxCollider2D col; 
	private GameObject graphicsGO;
	
	void Start () {
		
		active = startActive;
		
		col = GetComponent<BoxCollider2D>();
		graphicsGO = transform.GetChild(0).gameObject;
		
		col.enabled = active;
		graphicsGO.SetActive(active);
		
		// do this because I can't initialize lastFrameSpeedPercentage, and thus first lateUpdate they are always different, and so it always flips
		// active = !active; 
	}
	
	void LateUpdate () {
		
		float speedPercentage = Mathf.InverseLerp(bottomSpeed, topSpeed, playerScript.GetCurrentSpeed());
		if (speedPercentage != lastFrameSpeedPercentage)
		{
			active = !active;
			
			col.enabled = active;
			graphicsGO.SetActive(active);
		}
		
		lastFrameSpeedPercentage = Mathf.InverseLerp(bottomSpeed, topSpeed, playerScript.GetCurrentSpeed());
	}
	
	void SetPlayer (PlayerControl playerControl) {
		playerGO = playerControl.gameObject;
		playerScript = playerGO.GetComponent<PlayerControl>();
		
		// lastFrameSpeedPercentage = Mathf.InverseLerp(bottomSpeed, topSpeed, playerScript.GetCurrentSpeed());
	}
	
	void OnEnable () {
		Messenger<PlayerControl>.AddListener("SetPlayer", SetPlayer);
	}
	
	void OnDisable () {
		Messenger<PlayerControl>.RemoveListener("SetPlayer", SetPlayer);
	}
}
