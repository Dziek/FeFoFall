using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndPointScript : MonoBehaviour {
	
	public GameObject endPointGraphicsGO;
	
	void Awake () {
		
		// bool rendererEnabled = GetComponent<SpriteRenderer>().enabled;
		
		if (endPointGraphicsGO)
		{
			// GetComponent<SpriteRenderer>().enabled = false;
			
			GameObject go = Instantiate(endPointGraphicsGO, transform.position, transform.rotation) as GameObject;
			go.transform.SetParent(transform);
			
			go.transform.localScale = Vector2.one;
			
			DoParticleSystemStuff();
		}
	}
	
	void DoParticleSystemStuff () {
		ParticleSystem ps = GetComponentInChildren<ParticleSystem>();
		var sh = ps.shape;
		sh.scale = new Vector2(transform.localScale.x * 2, transform.localScale.y * 2);
		
		float volume = transform.localScale.x * transform.localScale.y;
		if (volume < 1) volume = 1;
		
		var em = ps.emission;
		em.rateOverTime = volume * 5f;
		
		// var main = ps.main;
		// main.
		
		var sz = ps.sizeOverLifetime;
		sz.sizeMultiplier = Mathf.Max(transform.localScale.x, transform.localScale.y);
	}
}
