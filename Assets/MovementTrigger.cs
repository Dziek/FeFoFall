using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementTrigger : MonoBehaviour {
	
	public GameObject targetGO;
	
	public Vector2 direction;
	public float speed;
	
	private bool active = true;
	
	void OnTriggerStay2D(Collider2D collision)
	{
		if (collision.tag == "Player" && active)
		{
			targetGO.transform.Translate(direction * speed * Time.deltaTime);
		}
		
	}
}
