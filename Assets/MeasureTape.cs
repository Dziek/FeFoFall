using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeasureTape : MonoBehaviour {

	// Use this for initialization
	void Start () {
		StartCoroutine("Measure");
	}
	
	// Update is called once per frame
	void Update () {
		// Cast();
		// transform.Rotate(transform.forward * -0.01f);
	}
	
	IEnumerator Measure () {
		
		float angleIncrease = 0;
		float targetIncrease = 10;
		
		float startAngle = transform.rotation.z;
		float endAngle = startAngle + targetIncrease;
		
		List<Vector2> hitPoints = new List<Vector2>();
		
		while (angleIncrease < targetIncrease)
		{
			RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.up, 10);
			hitPoints.Add(hit.point);
			
			transform.Rotate(transform.forward * -0.01f);
			angleIncrease += 0.01f;
			
			yield return null;
		}
		
		Vector2 furthestVector = Vector2.zero;
		float furthestDistance = 0;
		
		for (int i = 0; i < hitPoints.Count; i++)
		{
			float d = Vector2.Distance(hitPoints[i], transform.position);
			
			if (d > furthestDistance)
			{
				furthestDistance = d;
				furthestVector = hitPoints[i];
			}
		}
		
		Vector2 v = furthestVector;
		Debug.Log(v.x+", "+v.y);
	}
	
	void Cast () {
		RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.up, 10);
		
		if (hit != null)
		{
			// Debug.Log(hit.point);
			
			Vector3 v = hit.point;
			
			Debug.Log(v.x+", "+v.y+", "+v.z);
		}
	}
}
