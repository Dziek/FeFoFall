using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowIntersectPoints : MonoBehaviour {
	
	private Collider2D collider;
	
	// Use this for initialization
	void Start () {
		collider = GetComponent<Collider2D>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	void OnCollisionStay2D (Collision2D other) {
		
		ContactPoint2D[] contacts = new ContactPoint2D[10];
		
		collider.GetContacts(contacts);
		
		foreach (ContactPoint2D con in contacts)
		{
			Debug.Log(con.point);
		}
		
		Debug.Log(other);
		
	}
}
