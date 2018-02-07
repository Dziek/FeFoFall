using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleSystemShot : MonoBehaviour {
	
	private float spawnTime;
	private ParticleSystem ps;
	
	// Use this for initialization
	void Awake () {
		ps = GetComponent<ParticleSystem>();
		spawnTime = ps.startLifetime;
	}
	
	void OnEnable () {
		Invoke("Deactivate", spawnTime);
	}
	
	void OnDisable () {
		CancelInvoke("Deactivate");
	}
	
	void Deactivate () {
		gameObject.SetActive(false);
	}
}
