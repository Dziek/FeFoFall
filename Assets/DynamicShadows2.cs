using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class DynamicShadows2 : MonoBehaviour {
	
	// this is each point to cast to
	private struct MarkerData {
		public Vector2 pos;
		public float angle;
	}
	
	private List<MarkerData> hitPointDataList = new List<MarkerData>();
	
	// how many in array, because I'm doing that for some reason
	// private int amount = 640;
	
	// public Vector2[] positions; // all the actual positions
	// private int positionsArrayPos; // cornerPositions position, because using an array instead of a List for some reason
	
	// public Vector3[] newVertices;
    // public Vector2[] newUV;
    // public int[] newTriangles;
	
	public List<Vector2> positions = new List<Vector2>(); // all the actual positions
	
	public List<Vector3> calculatedVertices = new List<Vector3>();
	public List<Vector2> calculatedUV = new List<Vector2>();
	public List<int> calculatedTriangles = new List<int>();
	
	// public Vector3[] calculatedVertices;
	// public Vector2[] calculatedUV;
	// public int[] calculatedTriangles;
	
	private Mesh mesh;
	
	private GameObject frame;
	private GameObject level;
	
	private Collider2D[] levelColliders;
	
	private Vector2 screenBounds = new Vector2(8.86f, 4.969642f);
	private Vector2 screenBoundsSimplified = new Vector2(8.9f, 5);
	
	private Transform parentTransform;
	
    void Start() {
        
		Transform playerParent = transform.parent.parent;
		
		Vector2 adjustedScale = new Vector2(1 / playerParent.localScale.x, 1 / playerParent.localScale.y);
		transform.localScale = adjustedScale;
		
		mesh = new Mesh();
		
        GetComponent<MeshFilter>().mesh = mesh;
		
        // mesh.vertices = newVertices;
        // mesh.uv = newUV;
        // mesh.triangles = newTriangles;
		
		// mesh.RecalculateNormals();
		
		// Debug.Break();
		
		frame = GameObject.Find("FrameSprites"); //TODO: Make it so if can't find GameFrame then get DefaultFrame
		level = gameObject.FindParentWithTag("Level");
		
		// Debug.Log("Shadows " + level);
		
		// levelColliders = level.GetComponentsInChildren<Collider2D>();
		
		// Try to avoid casting to players, might need tweaking for multiple players
		Collider2D[] temp = level.GetComponentsInChildren<Collider2D>();
		
		List<Collider2D> tempList = new List<Collider2D>();
		
		for (int i = 0; i < temp.Length; i++)
		{
			// if (temp[i].gameObject.tag != "Player" && temp[i].gameObject.tag != "Trigger" && temp[i].gameObject.layer != LayerMask.NameToLayer("IgnoreShadow"))
			if (temp[i].gameObject.tag != "Player" && temp[i].gameObject.tag != "Trigger")
			// if (temp[i].gameObject != transform.parent.parent)
			{
				tempList.Add(temp[i]);
				// Debug.Log(LayerMask.LayerToName(temp[i].gameObject.layer));
				// Debug.Log(temp[i].gameObject.tag);
			}
		}
		
		levelColliders = tempList.ToArray();
		
		// four corners of each collider plus screen corners
		// int arrayLength = (levelColliders.Length * 4) + 4;
		
		// hitPoints = new Vector2[arrayLength];
		// positions = new Vector2[arrayLength];
		
		// times 3 for hitPointList then 3 again for whatever
		// arrayLength = arrayLength * 9;
		
		// calculatedVertices = new Vector3[arrayLength];
		// calculatedUV = new Vector2[arrayLength];
		// calculatedTriangles = new int[arrayLength];
		
		AddScreen();
		
		// GetCorners();
		// Cast();
		// SortHitPointsQuick();
		// MakeTriangles();
		// Draw();
		
		// Debug.Log(transform.root);
		transform.parent.parent.gameObject.GetComponent<PlayerControl>().shadowGO = gameObject;
		parentTransform = transform.parent;
		transform.SetParent(transform.root);
		transform.rotation = Quaternion.identity;
		
		// if (level != null)
		// {
			// transform.position = parentTransform.transform.position;
			
			// GetCorners();
			// Cast();
			// SortHitPointsQuick();
			// MakeTriangles();
			// Draw();
		// }
    }
	
	// Update is called once per frame
	void LateUpdate () {
		positions.Clear();
		
		if (level != null)
		{
			transform.position = parentTransform.transform.position;
			
			AddScreen();
			GetCorners();
			Cast();
			SortHitPointsQuick();
			MakeTriangles();
			Draw();
		}
	}
	
	void GetCorners () {
		
		// positionsArrayPos = 4;
		
		GetPositions();
	}
	
	void AddScreen () {
		
		float x = screenBounds.x;
		float y = screenBounds.y;
		
		AddPos(new Vector2(-x, -y));
		AddPos(new Vector2(-x, y));
		AddPos(new Vector2(x, y));
		AddPos(new Vector2(x, -y));	
		
		// positions.Add(new Vector2(-x, -y));
		// positions.Add(new Vector2(-x, y));
		// positions.Add(new Vector2(x, y));
		// positions.Add(new Vector2(x, -y));
		
		// positions[0] = new Vector2(-x, -y);
		// positions[1] = new Vector2(-x, y);
		// positions[2] = new Vector2(x, y);
		// positions[3] = new Vector2(x, -y);
	}
	
	void GetPositions () {
		
		Vector2 pos;
		Collider2D[] cols = levelColliders;
		
		Vector2[] cornerPositions = new Vector2[4];
		
		for (int i = 0; i < cols.Length; i++)
		{
			Collider2D box = cols[i];
			Quaternion q = box.transform.rotation;
			
			if (box.gameObject.tag == "Player")
			{
				continue;
			}
			
			// bottom left
			box.transform.rotation = Quaternion.identity;
			
			pos = (Vector2)box.bounds.center - new Vector2(box.bounds.extents.x, box.bounds.extents.y);
			pos = box.transform.InverseTransformPoint(pos);
			
			box.transform.rotation = q;
			
			pos = box.transform.TransformPoint(pos);
			AddPos(pos);
			cornerPositions[0] = pos;
			
			// bottom right
			box.transform.rotation = Quaternion.identity;
			
			pos = (Vector2)box.bounds.center - new Vector2(-box.bounds.extents.x, box.bounds.extents.y);
			pos = box.transform.InverseTransformPoint(pos);
			
			box.transform.rotation = q;
			
			pos = box.transform.TransformPoint(pos);
			AddPos(pos);
			cornerPositions[1] = pos;
			
			// top right
			box.transform.rotation = Quaternion.identity;
			
			pos = (Vector2)box.bounds.center - new Vector2(-box.bounds.extents.x, -box.bounds.extents.y);
			pos = box.transform.InverseTransformPoint(pos);
			
			box.transform.rotation = q;
			
			pos = box.transform.TransformPoint(pos);
			AddPos(pos);
			cornerPositions[2] = pos;
			
			// top left
			box.transform.rotation = Quaternion.identity;
			
			pos = (Vector2)box.bounds.center - new Vector2(box.bounds.extents.x, -box.bounds.extents.y);
			pos = box.transform.InverseTransformPoint(pos);
			
			box.transform.rotation = q;
			
			pos = box.transform.TransformPoint(pos);
			AddPos(pos);
			cornerPositions[3] = pos;
			
			// now all corners are got, get collisions between those corners
			GetCollisions(cornerPositions, box);
		}
	}
	
	void GetCollisions (Vector2[] cornerPositions, Collider2D colliderToCheck) {
		
		for (int i = 0; i < cornerPositions.Length; i++)
		{
			int a = i;
			int b = i + 1;
			int c = i - 1;
			
			if (b >= cornerPositions.Length)
			{
				b = 0;
			}
			
			if (c < 0)
			{
				c = cornerPositions.Length-1;
			}
			
			CheckBetweenPoints(cornerPositions[a], cornerPositions[b], colliderToCheck);
			CheckBetweenPoints(cornerPositions[a], cornerPositions[c], colliderToCheck);
		}
	}
	
	void CheckBetweenPoints (Vector2 a, Vector2 b, Collider2D ownCollider) {
		Vector2 pos = a;
		Vector2 dir = b - a;
		float distance = Vector2.Distance(b, a);
		int layerMask = LayerMask.GetMask("Player") | LayerMask.GetMask("Trigger") | LayerMask.GetMask("IgnoreShadow");
		
		RaycastHit2D[] hit = Physics2D.RaycastAll(pos, dir, distance, ~layerMask);
		// Debug.DrawRay(pos, dir * distance, Color.green, Mathf.Infinity);
		
		for(int i = 0; i < hit.Length; i++)
		{
			if (hit[i].collider != ownCollider)
			{
				AddPos(hit[i].point);
			}
		}
	}
	
	void AddPos (Vector2 pos) {
		
		// I can do all this sorting here, because GetCollisions uses separate data, and not this one
		
		float x = screenBoundsSimplified.x;
		float y = screenBoundsSimplified.y;
		
		Vector2 clampedPos = new Vector2(Mathf.Clamp(pos.x, -x, x), Mathf.Clamp(pos.y, -y, y));
		
		bool checkForDuplicate = false;
		// for (int i = 0; i < positions.Length; i++)
		for (int i = 0; i < positions.Count; i++)
		{
			if (positions[i] == clampedPos)
			{
				checkForDuplicate = true;
			}
		}
		
		if (checkForDuplicate == false)
		{
			// positions[positionsArrayPos] = clampedPos;
			// positionsArrayPos++;
			
			positions.Add(clampedPos);
		}		
	}
	
	// void Cast () {
		// Vector2 pos = transform.position;
		// Vector2 dir;
		// // float distance = Mathf.Infinity;
		// float distance = 36;
		// int layerMask = LayerMask.GetMask("Player") | LayerMask.GetMask("Trigger") | LayerMask.GetMask("IgnoreShadow");
		// // int layerMask = LayerMask.GetMask("UI");
		
		// hitPointDataList.Clear();
		
		// for (int i = 0; i < positions.Count; i++)
		// // for (int i = 0; i < positions.Length; i++)
		// {
			
			// // dir = corners[i] - pos;
			// dir = positions[i] - pos;
			
			// // RaycastHit2D hit = Physics2D.Raycast(pos, dir, distance);
			// RaycastHit2D hit = Physics2D.Raycast(pos, dir, distance, ~layerMask);
			
			// MarkerData temp = new MarkerData();		
			// temp.pos = hit.point;
			// temp.angle = pos.AngleTo(hit.point);		
			// hitPointDataList.Add(temp);
			
			// Vector2 debugRayInfo = hit.point - pos;
			// Debug.DrawRay(pos, debugRayInfo, Color.green);
			
			// Vector2 offsetDir = dir;
			// offsetDir = Quaternion.Euler(0, 0, -1f) * offsetDir;
			// // offsetDir = Quaternion.Euler(0, 0, -0.01f) * offsetDir;
			
			// // RaycastHit2D hitAfter2 = Physics2D.Raycast(pos, offsetDir, distance);
			// RaycastHit2D hitAfter2 = Physics2D.Raycast(pos, offsetDir, distance, ~layerMask);
			
			// MarkerData temp2 = new MarkerData();		
			// temp2.pos = hitAfter2.point;
			// temp2.angle = pos.AngleTo(hitAfter2.point);		
			// hitPointDataList.Add(temp2);
			
			// Vector2 debugRayInfo2 = hitAfter2.point - pos;
			// Debug.DrawRay(pos, debugRayInfo2, Color.red);
			
			// Vector2 offsetDir2 = dir;
			// offsetDir2 = Quaternion.Euler(0, 0, 1f) * offsetDir2;
			// // offsetDir2 = Quaternion.Euler(0, 0, 0.01f) * offsetDir2;
			
			// // RaycastHit2D hitAfter = Physics2D.Raycast(pos, offsetDir2, distance);
			// RaycastHit2D hitAfter = Physics2D.Raycast(pos, offsetDir2, distance, ~layerMask);
			
			// MarkerData temp3 = new MarkerData();		
			// temp3.pos = hitAfter.point;
			// temp3.angle = pos.AngleTo(hitAfter.point);		
			// hitPointDataList.Add(temp3);
			
			// Vector2 debugRayInfo3 = hitAfter.point - pos;
			// Debug.DrawRay(pos, debugRayInfo3, Color.red);
		// }
	// }
	
	void Cast () {
		Vector2 pos = transform.position;
		Vector2 dir;
		Vector2 offsetDir; // for follow up rays
		float distance = 36;
		int layerMask = LayerMask.GetMask("Player") | LayerMask.GetMask("Trigger") | LayerMask.GetMask("IgnoreShadow");
		
		Vector2 debugRayInfo; // for Debug.DrawRay
		MarkerData tempData; // to fill with each ray hit if needed
		
		hitPointDataList.Clear();
		
		for (int i = 0; i < positions.Count; i++)
		// for (int i = 0; i < positions.Length; i++)
		{
			dir = positions[i] - pos;
			
			RaycastHit2D centreHit = Physics2D.Raycast(pos, dir, distance, ~layerMask);
			
				debugRayInfo = centreHit.point - pos;
				Debug.DrawRay(pos, debugRayInfo, Color.green);
			
			offsetDir = dir;
			offsetDir = Quaternion.Euler(0, 0, -0.01f) * offsetDir;
			// offsetDir = Quaternion.Euler(0, 0, -1f) * offsetDir;
			
			RaycastHit2D followUpHit1 = Physics2D.Raycast(pos, offsetDir, distance, ~layerMask);

				debugRayInfo = followUpHit1.point - pos;
				Debug.DrawRay(pos, debugRayInfo, Color.red);
			
			offsetDir = dir;
			offsetDir = Quaternion.Euler(0, 0, 0.01f) * offsetDir;
			// offsetDir = Quaternion.Euler(0, 0, 1f) * offsetDir;
			
			RaycastHit2D followUpHit2 = Physics2D.Raycast(pos, offsetDir, distance, ~layerMask);
			
				debugRayInfo = followUpHit2.point - pos;
				Debug.DrawRay(pos, debugRayInfo, Color.red);
			
			// Okay, all rays sent, now let's see what we have, and add anything useful to hitPointData
			
			
			// HA, SO THE PROBLEM WITH THIS, IS I WAS ONLY THINKING OF CONCAVE CORNERS, NOT CONVEX. WHAT I WANT IS SIDES, NOT OBJECTS OR, CHECK IF CENTRE RAY
			// HITS THE EXACT POINT?
			//Firstly check they don't all hit the same thing. If they do, we don't any of it, so we'll continue to the next one
			if (centreHit.collider == followUpHit1.collider && centreHit.collider == followUpHit2.collider && followUpHit1.collider == followUpHit2.collider)
			{
				// check if centreRay hit it's target (using Distance in case just off), in which case it's probably still useful and we'll grab it anyway
				if (Mathf.Abs(Vector2.Distance(centreHit.point, positions[i])) < 0.1f)
				{
					tempData = new MarkerData();
					tempData.pos = centreHit.point;
					tempData.angle = pos.AngleTo(centreHit.point);		
					hitPointDataList.Add(tempData);	
				}
				
				// but then we definitely don't need the other two, so let's go
				continue;
			}
			
			// Otherwise we'll add the centre one, as that will always (well) be useful
			tempData = new MarkerData();
			tempData.pos = centreHit.point;
			tempData.angle = pos.AngleTo(centreHit.point);		
			hitPointDataList.Add(tempData);
			
			//Check for duplicate hits, if the rays hit the same thing we don't need them(?)
			if (centreHit.collider != followUpHit1.collider)
			{
				tempData = new MarkerData();
				tempData.pos = followUpHit1.point;
				tempData.angle = pos.AngleTo(followUpHit1.point);		
				hitPointDataList.Add(tempData);

			}
			if (centreHit.collider != followUpHit2.collider)
			{
				tempData = new MarkerData();
				tempData.pos = followUpHit2.point;
				tempData.angle = pos.AngleTo(followUpHit2.point);		
				hitPointDataList.Add(tempData);
			}
		}
	}
	
	void SortHitPointsQuick () {
		
		// sort clockwise or whatever it is
		hitPointDataList.Sort((a, b) => b.angle.CompareTo(a.angle));
	}
	
	void MakeTriangles () {
		
		calculatedVertices.Clear();
		calculatedUV.Clear();
		calculatedTriangles.Clear();
		
		Vector2 pos = Vector2.zero;
		
		// int arrayPos = 0;
		
		for (int i = 0; i < hitPointDataList.Count; i++)
		{
			calculatedVertices.Add(pos);
			calculatedUV.Add(pos);
			
			calculatedTriangles.Add(calculatedTriangles.Count);
			
			// calculatedVertices[arrayPos] = pos;
			// calculatedUV[arrayPos] = pos;
			
			// calculatedTriangles[arrayPos] = arrayPos;
			// arrayPos++;
			
			Vector2 firstHitPoint = hitPointDataList[i].pos - (Vector2)transform.position;
			
			calculatedVertices.Add(firstHitPoint);
			calculatedUV.Add(firstHitPoint);
			
			calculatedTriangles.Add(calculatedTriangles.Count);
			
			// calculatedVertices[arrayPos] = firstHitPoint;
			// calculatedUV[arrayPos] = firstHitPoint;
			
			// calculatedTriangles[arrayPos] = arrayPos;
			// arrayPos++;
			
			int v = i+1;
			if (v == hitPointDataList.Count)
			{
				v = 0;
			}
			
			Vector2 secondHitPoint = hitPointDataList[v].pos - (Vector2)transform.position;
			
			calculatedVertices.Add(secondHitPoint);
			calculatedUV.Add(secondHitPoint);
			
			calculatedTriangles.Add(calculatedTriangles.Count);
			
			// calculatedVertices[arrayPos] = secondHitPoint;
			// calculatedUV[arrayPos] = secondHitPoint;
			
			// calculatedTriangles[arrayPos] = arrayPos;
			// arrayPos++;
		}
	}
	
	void Draw () {
		
		mesh.Clear();

        // mesh.vertices = newVertices;
        // mesh.uv = newUV;
        // mesh.triangles = newTriangles; 
		
		mesh.vertices = calculatedVertices.ToArray();
		mesh.uv = calculatedUV.ToArray();
        mesh.triangles = calculatedTriangles.ToArray();
		
		// mesh.vertices = calculatedVertices;
		// mesh.uv = calculatedUV;
        // mesh.triangles = calculatedTriangles;
	}
	
	
	// void OnDrawGizmos () {
        // Gizmos.color = Color.red;
		
		// // for (int i = 0; i < positions.Length; i++)
		// for (int i = 0; i < positions.Count; i++)
		// {
			// Gizmos.DrawSphere(positions[i], 0.1f);
		// }
		
		// Gizmos.color = Color.green;
		
		// for (int i = 0; i < hitPointDataList.Count; i++)
		// {
			// Gizmos.DrawSphere(hitPointDataList[i].pos, 0.1f);
		// }
        
    // }
	
	// void OnEnable () {
		// Messenger<GameObject>.AddListener("NewLevelLoaded", StartCastingShadow);
	// }
	
	// void OnDisable () {
		// Messenger<GameObject>.RemoveListener("NewLevelLoaded", StartCastingShadow);
	// }
}
