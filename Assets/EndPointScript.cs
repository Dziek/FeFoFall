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
		}
	}
}
