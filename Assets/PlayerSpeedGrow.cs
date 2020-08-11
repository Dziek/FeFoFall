using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpeedGrow : MonoBehaviour {
	
	public float topSize = 2;
	public float bottomSize = 1;
	
	public float topSpeed = 10;
	public float bottomSpeed = 5;
	
	private GameObject playerGO;
	private PlayerControl playerScript;
	
	void LateUpdate () {
		
		float speedPercentage = Mathf.InverseLerp(bottomSpeed, topSpeed, playerScript.GetCurrentSpeed());
		float v = Mathf.Lerp(bottomSize, topSize, speedPercentage);
		
		transform.localScale = new Vector3(v, v, 1);
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
