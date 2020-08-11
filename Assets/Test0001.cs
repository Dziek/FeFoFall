using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test0001 : MonoBehaviour {

	private List<Vector2> positions = new List<Vector2>();
	
	// private Vector2 screenBounds = new Vector2(8.86f, 4.969642f);
	private Vector2 screenBounds = new Vector2(8.9f, 5f);
	private Collider2D thisCollider;
	
	void Awake () {
		thisCollider = GetComponent<Collider2D>();
	}
	
	// Use this for initialization
	void Start () {
		FindCorners();
		FindCollisions();
		CullPos();
	}
	
	// void Update () {
		// FindCollisions();
	// }
	
	void FindCorners () {
		
		Vector2 pos;
		Collider2D box = thisCollider;
		Quaternion q = box.transform.rotation;
		
		
		// bottom left
		box.transform.rotation = Quaternion.identity;
		
		pos = (Vector2)box.bounds.center - new Vector2(box.bounds.extents.x, box.bounds.extents.y);
		pos = box.transform.InverseTransformPoint(pos);
		
		box.transform.rotation = q;
		
		pos = box.transform.TransformPoint(pos);
		AddPos(pos);
		
		// bottom right
		box.transform.rotation = Quaternion.identity;
		
		pos = (Vector2)box.bounds.center - new Vector2(-box.bounds.extents.x, box.bounds.extents.y);
		pos = box.transform.InverseTransformPoint(pos);
		
		box.transform.rotation = q;
		
		pos = box.transform.TransformPoint(pos);
		AddPos(pos);
		
		// top right
		box.transform.rotation = Quaternion.identity;
		
		pos = (Vector2)box.bounds.center - new Vector2(-box.bounds.extents.x, -box.bounds.extents.y);
		pos = box.transform.InverseTransformPoint(pos);
		
		box.transform.rotation = q;
		
		pos = box.transform.TransformPoint(pos);
		AddPos(pos);
		
		// top left
		box.transform.rotation = Quaternion.identity;
		
		pos = (Vector2)box.bounds.center - new Vector2(box.bounds.extents.x, -box.bounds.extents.y);
		pos = box.transform.InverseTransformPoint(pos);
		
		box.transform.rotation = q;
		
		pos = box.transform.TransformPoint(pos);
		AddPos(pos);
	}
	
	void FindCollisions () {
		List<Vector2> cornerPositions = new List<Vector2>(positions);
		
		for (int i = 0; i < cornerPositions.Count; i++)
		{
			int a = i;
			int b = i+1;
			int c = i-1;
			
			if (b >= cornerPositions.Count)
			{
				b = 0;
			}
			
			if (c < 0)
			{
				c = cornerPositions.Count-1;
			}
			
			Debug.Log(a + " " + b + " " + i);
			
			CheckBetweenPoints(cornerPositions[a], cornerPositions[b]);
			CheckBetweenPoints(cornerPositions[a], cornerPositions[c]);
		}
		
		// Vector2 bottomLeft = positions[0];
		// Vector2 bottomRight = positions[1];
		// Vector2 topRight = positions[2];
		// Vector2 topLeft = positions[3];
		
		// Vector2 pos = transform.position;
		// Vector2 dir = Vector2.right;
		// float distance = 36;
		// int layerMask = LayerMask.GetMask("Player") | LayerMask.GetMask("Trigger") | LayerMask.GetMask("IgnoreShadow");
		
		// // Bottom Left
		// pos = bottomLeft;
		// dir = bottomRight - bottomLeft;
		// distance = Vector2.Distance(bottomRight, bottomLeft);
		
		// RaycastHit2D[] hit = Physics2D.RaycastAll(pos, dir, distance, ~layerMask);
		// Debug.DrawRay(pos, dir * distance, Color.green, Mathf.Infinity);
		
		// Debug.Log(hit.Length);
		// Debug.Log(hit[0].collider.gameObject.name);
		
		// for(int i = 0; i < hit.Length; i++)
		// {
			// if (hit[i].collider != thisCollider)
			// {
				// AddPos(hit[i].point);
			// }
		// }
	}
	
	void CheckBetweenPoints (Vector2 a, Vector2 b) {
		Vector2 pos = a;
		Vector2 dir = b - a;
		float distance = Vector2.Distance(b, a);
		int layerMask = LayerMask.GetMask("Player") | LayerMask.GetMask("Trigger") | LayerMask.GetMask("IgnoreShadow");
		
		RaycastHit2D[] hit = Physics2D.RaycastAll(pos, dir, distance, ~layerMask);
		Debug.DrawRay(pos, dir * distance, Color.green, Mathf.Infinity);
		
		// Debug.Log(hit.Length);
		// Debug.Log(hit[0].collider.gameObject.name);
		
		for(int i = 0; i < hit.Length; i++)
		{
			if (hit[i].collider != thisCollider)
			{
				AddPos(hit[i].point);
			}
		}
	}
	
	void AddPos (Vector2 pos) {
		
		// if (Mathf.Abs(pos.x) < screenBounds.x && Mathf.Abs(pos.y) < screenBounds.y)
		// if (Mathf.Abs(pos.x) < screenBounds.x)
		// if (Mathf.Abs(pos.y) < screenBounds.y)
		{
			positions.Add(pos);
			Debug.Log("Pos Added: " + pos);
		}
	}
	
	void CullPos () {
		List<Vector2> positionsCopy = new List<Vector2>(positions);
		positions.Clear();
		
		for (int i = 0; i < positionsCopy.Count; i++)
		{
			if (Mathf.Abs(positionsCopy[i].x) < screenBounds.x && Mathf.Abs(positionsCopy[i].y) < screenBounds.y)
			{
				AddPos(positionsCopy[i]);
			}
		}
	}
	
	void OnDrawGizmosSelected() {
        Gizmos.color = Color.green;
		
		for (int i = 0; i < positions.Count; i++)
		{
			Gizmos.DrawSphere(positions[i], 0.1f + (0.01f * i));
		}
        
    }
}
