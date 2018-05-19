using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelCompletePS : MonoBehaviour {

	private ParticleSystem pS;
	
	void Awake () {
		pS = GetComponent<ParticleSystem>();
	}

	public void Fire (Vector2 pos) {
		transform.position = pos;
		pS.Play();
	}
}
