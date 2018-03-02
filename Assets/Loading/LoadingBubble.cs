using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingBubble : MonoBehaviour {
	
	public float speed = 5;
	
	private Vector2 direction;
	
	private Rigidbody2D rb;
	
	void Awake () {
		rb = GetComponent<Rigidbody2D>();
		// transform.Rotate(Vector3.forward * Random.Range(0, 360));
		transform.Rotate(Vector3.forward * 45);
	}
	
	// Use this for initialization
	void Start () {
		direction = transform.right;
		
		rb.velocity = direction * speed;
	}
	
	void OnCollisionEnter2D (Collision2D other) {
		if (other.gameObject.tag == "Frame")
		{			
			Vector2 newVelocity = other.relativeVelocity;
			
			// Debug.Log(other.contacts[0].normal);
			
			if (Mathf.Abs(other.contacts[0].normal.x) == 1)
			{
				newVelocity = new Vector2(newVelocity.x, -newVelocity.y);
				
			}else if (Mathf.Abs(other.contacts[0].normal.y) == 1)
			{
				newVelocity = new Vector2(-newVelocity.x, newVelocity.y);
			}
			
			if (newVelocity.magnitude > 25)
			{
				newVelocity = newVelocity / 2;
			}
			
			rb.velocity = newVelocity;
			
			// Debug.Log("Doing something?");
		}
	}
}
