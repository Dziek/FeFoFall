using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleTied : MonoBehaviour {
	
	public GameObject playerGO;
	
	public bool useX = true;
	public bool useY;
	
	// public bool flipX;
	// public bool flipY;
	
	public bool tieXToY;
	
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
		
		float x, y;
		x = y = 1;
		
		if (useX == true)
		{
			x = playerGO.transform.position.x;
			
			// if (flipX == true)
			// {
				// x = -x;
			// }
			
			if (tieXToY == true)
			{
				x = playerGO.transform.position.y;
			}
		}
		
		if (useY == true)
		{
			y = playerGO.transform.position.y;
			
			// if (flipY == true)
			// {
				// y = -y;
			// }
		}
		
		transform.localScale = new Vector2(x, y);
	}
}
