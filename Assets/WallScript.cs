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
			
			// if the sprite is sliced I have to do some boring stuff - note, this won't work with ChangeScale
			if (go.GetComponent<SpriteRenderer>().drawMode == SpriteDrawMode.Sliced)
			{
				// Get the scale from transform
				Vector2 wallSize = transform.localScale;
				
				// set transform to 1
				transform.localScale = Vector2.one;
				
				// set sprite size to wallSize
				go.GetComponent<SpriteRenderer>().size = wallSize;
				
				// set collider size
				GetComponent<BoxCollider2D>().size = wallSize;
				// GetComponent<BoxCollider2D>().autoTiling = true;
				
				// set mask child scale
				go.transform.GetChild(0).localScale = wallSize;
			}
			
			go.transform.SetParent(transform);
			
			go.transform.localScale = Vector2.one;
		}
	}
}
