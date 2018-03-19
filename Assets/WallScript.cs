using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallScript : MonoBehaviour {
	
	public GameObject wallGraphicsGO;
	
	void Awake () {
		if (wallGraphicsGO)
		{
			GetComponent<SpriteRenderer>().enabled = false;
			
			GameObject go = Instantiate(wallGraphicsGO, transform.position, transform.rotation) as GameObject;
			go.transform.SetParent(transform);
			
			go.transform.localScale = Vector2.one;
		}
	}
}
