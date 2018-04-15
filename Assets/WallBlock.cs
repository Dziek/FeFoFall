using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallBlock : MonoBehaviour {

	public float speed = 5f;

	private Vector2 startPos;
	public Vector2 travelDirection;
	
	private bool canMove;
	
	void Awake () {
		startPos = transform.position;
	}
	
	void Update () {
		if (canMove)
		{
			transform.Translate(-travelDirection * speed * Time.deltaTime);
			
			// if (Vector2.Distance(transform.position, startPos) > 20)
			// {
				// transform.position = startPos;
			// }
		}
	}
	
	void SetCanMove () {
		canMove = true;
		
		// Debug.Log("D");
	}
	
	void OnEnable () {
		Messenger.AddListener("FirstMovement", SetCanMove);
	}
	
	void OnDisable () {
		Messenger.RemoveListener("FirstMovement", SetCanMove);
	}
}
