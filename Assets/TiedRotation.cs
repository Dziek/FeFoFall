using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TiedRotation : MonoBehaviour {

	public GameObject playerGO;
	
	public float rotationScale = 1;
	
	void SetPlayer (PlayerControl playerControl) {
		playerGO = playerControl.gameObject;
	}
	
	void Update () {
		transform.eulerAngles = new Vector3(0, 0, playerGO.transform.position.x * rotationScale);
	}
	
	void OnEnable () {
		Messenger<PlayerControl>.AddListener("SetPlayer", SetPlayer);
	}
	
	void OnDisable () {
		Messenger<PlayerControl>.RemoveListener("SetPlayer", SetPlayer);
	}
}
