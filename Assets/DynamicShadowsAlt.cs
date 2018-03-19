using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicShadowsAlt : MonoBehaviour {
	
	// public GameObject[] frames;
	
	private int amount = 36;
	
	private struct CornerData {
		public Vector2 pos;
		public float angle;
	}
	
	public  List<Vector2> corners = new List<Vector2>();
	private  List<CornerData> cornerDataList = new List<CornerData>();
	
	private Vector2[] hitPoints;
	public List<Vector2> hitPointList = new List<Vector2>();
	private List<CornerData> hitPointDataList = new List<CornerData>();
	
	public Vector3[] newVertices;
    public Vector2[] newUV;
    public int[] newTriangles;
	
	public List<Vector3> calculatedVertices = new List<Vector3>();
	public List<Vector2> calculatedUV = new List<Vector2>();
	public List<int> calculatedTriangles = new List<int>();
	
	private Mesh mesh;
	
	private GameObject frame;
	private GameObject level;
	
	private Vector2 screenBounds = new Vector2(8.86f, 4.969642f);
	
	void Awake () {
		
		hitPoints = new Vector2[amount];
		
		newVertices = new Vector3[amount * 3];
		newUV = new Vector2[amount * 3];
		
		newTriangles = new int[amount * 3];
		
		for (int i = 0; i < newTriangles.Length; i++)
		{
			newTriangles[i] = i;
		}
		
		// Debug.Log(System.Enviroment.Version);
		
		// GetComponent<Renderer>().sortingLayerName = "Background";
	}
	
    void Start() {
        
		Transform playerParent = transform.parent.parent;
		
		Vector2 adjustedScale = new Vector2(1 / playerParent.localScale.x, 1 / playerParent.localScale.y);
		transform.localScale = adjustedScale;
		
		mesh = new Mesh();
		
        GetComponent<MeshFilter>().mesh = mesh;
		
        mesh.vertices = newVertices;
        mesh.uv = newUV;
        mesh.triangles = newTriangles;
		
		mesh.RecalculateNormals();
		
		// Debug.Break();
		
		frame = GameObject.Find("FrameSprites"); //TODO: Make it so if can't find GameFrame then get DefaultFrame
		level = gameObject.FindParentWithTag("Level");
		
		// Debug.Log("Shadows " + level);
		
		GetCorners();
		Cast();
		SortHitPointsQuick();
		MakeTriangles();
		Draw();
    }
	
	// void StartCastingShadow (GameObject levelGO) {
		// // Debug.Break();
		
		// frame = GameObject.Find("FrameSprites"); //TODO: Make it so if can't find GameFrame then get DefaultFrame
		// // level = GameObject.FindWithTag("Level");
		// level = levelGO;
		
		// // Debug.Log("Shadows " + level);
		
		// GetCorners();
		// Cast();
		// SortHitPointsQuick();
		// MakeTriangles();
		// Draw();
	// }
	
	// Update is called once per frame
	void LateUpdate () {
		
		// calculatedVertices.Clear();
		// calculatedUV.Clear();
		// calculatedTriangles.Clear();
		
		// corners.Clear();
		
		if (level != null)
		{
			GetCorners();
			// DummyCast();
			Cast();
			SortHitPointsQuick();
			MakeTriangles();
			Draw();
		}
	}
	
	void GetCorners () {
		
		corners.Clear();
		
		AddScreen();
		// AddFrame();
		GetLevelBits();
		GetFrameInterruptions();
		// SortCorners();
	}
	
	void AddScreen () {
		// float x = 8.86f;
		// float y = 4.969642f;
		
		float x = screenBounds.x;
		float y = screenBounds.y;
		
		corners.Add(new Vector2(-x, -y));
		corners.Add(new Vector2(-x, y));
		corners.Add(new Vector2(x, y));
		corners.Add(new Vector2(x, -y));
	}
	
	void AddFrame () {
				
		Collider2D box;	
		Vector2 pos;
		
		// GameObject[] frames = new GameObject[frame.transform.childCount];
		Collider2D[] cols = frame.GetComponentsInChildren<Collider2D>();
		
		// for (int i = 0; i < frames.Length; i++)
		for (int i = 0; i < cols.Length; i++)
		{
			// frames[i] = frame.transform.GetChild(i).gameObject;
			
			// box = frames[i].GetComponent<Collider2D>();
			box = cols[i];
			
			pos = (Vector2)box.bounds.center - new Vector2(box.bounds.extents.x, box.bounds.extents.y);	
			corners.Add(pos);
			
			pos = (Vector2)box.bounds.center - new Vector2(-box.bounds.extents.x, box.bounds.extents.y);	
			corners.Add(pos);
			
			pos = (Vector2)box.bounds.center - new Vector2(box.bounds.extents.x, -box.bounds.extents.y);	
			corners.Add(pos);
			
			pos = (Vector2)box.bounds.center - new Vector2(-box.bounds.extents.x, -box.bounds.extents.y);	
			corners.Add(pos);
			
		}
		
		// box = frames[2].GetComponent<Collider2D>();
			
		// pos = (Vector2)box.bounds.center - new Vector2(box.bounds.extents.x, box.bounds.extents.y);	
		// corners.Add(pos);
		
	}
	
	// void GetLevelBits () {
		
		// Vector2 pos;
		// Collider2D[] cols = level.GetComponentsInChildren<Collider2D>();
		
		// for (int i = 0; i < cols.Length; i++)
		// {
			// Collider2D box = cols[i];
			
			// pos = (Vector2)box.bounds.center - new Vector2(box.bounds.extents.x, box.bounds.extents.y);	
			// corners.Add(pos);
			
			// pos = (Vector2)box.bounds.center - new Vector2(-box.bounds.extents.x, box.bounds.extents.y);	
			// corners.Add(pos);
			
			// pos = (Vector2)box.bounds.center - new Vector2(box.bounds.extents.x, -box.bounds.extents.y);	
			// corners.Add(pos);
			
			// pos = (Vector2)box.bounds.center - new Vector2(-box.bounds.extents.x, -box.bounds.extents.y);	
			// corners.Add(pos);
		// }
	// }
	
	void GetLevelBits () {
		
		Vector2 pos;
		Collider2D[] cols = level.GetComponentsInChildren<Collider2D>();
		
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
			// corners.Add(pos);
			AddCornerPos(pos);
			
			// top right
			box.transform.rotation = Quaternion.identity;
			
			pos = (Vector2)box.bounds.center - new Vector2(-box.bounds.extents.x, -box.bounds.extents.y);
			pos = box.transform.InverseTransformPoint(pos);
			
			box.transform.rotation = q;
			
			pos = box.transform.TransformPoint(pos);
			// corners.Add(pos);
			AddCornerPos(pos);
			
			// bottom right
			box.transform.rotation = Quaternion.identity;
			
			pos = (Vector2)box.bounds.center - new Vector2(-box.bounds.extents.x, box.bounds.extents.y);
			pos = box.transform.InverseTransformPoint(pos);
			
			box.transform.rotation = q;
			
			pos = box.transform.TransformPoint(pos);
			// corners.Add(pos);
			AddCornerPos(pos);
			
			// top left
			box.transform.rotation = Quaternion.identity;
			
			pos = (Vector2)box.bounds.center - new Vector2(box.bounds.extents.x, -box.bounds.extents.y);
			pos = box.transform.InverseTransformPoint(pos);
			
			box.transform.rotation = q;
			
			pos = box.transform.TransformPoint(pos);
			// corners.Add(pos);
			AddCornerPos(pos);
			
		}
	}
	
	void GetFrameInterruptions () {
		// Debug.Log("D");
	}
	
	void AddCornerPos (Vector2 pos) {
		
		float x = screenBounds.x;
		float y = screenBounds.y;
		
		// Vector2 clampedPos = pos;
		
		Vector2 clampedPos = new Vector2(Mathf.Clamp(pos.x, -x, x), Mathf.Clamp(pos.y, -y, y));
		
		corners.Add(clampedPos);
	}
	
	void SortCorners () {
		for (int i = 0; i < corners.Count; i++)
		{
			CornerData temp = new CornerData();
			
			temp.pos = corners[i];
			temp.angle = Vector2.Angle(transform.position, corners[i]);
			
			cornerDataList.Add(temp);
		}
		
		corners.Clear();
		
		cornerDataList.Sort((a, b) => a.angle.CompareTo(b.angle));
		// list.Sort((a, b) => a.x.CompareTo(b.x));
		
		for (int i = 0; i < cornerDataList.Count; i++)
		{
			corners.Add(cornerDataList[i].pos);
		}
	}
	
	void DummyCast () {
		Vector2 pos = transform.position;
		Vector2 dir;
		// float distance = Mathf.Infinity;
		float distance = 36;
		int layerMask = LayerMask.GetMask("Player");
		
		// hitPointList.Clear();
		
		for (int i = 0; i < corners.Count; i++)
		{
			
			dir = corners[i] - pos;
			
			RaycastHit2D hit = Physics2D.Raycast(pos, dir, distance);
			// RaycastHit2D hit = Physics2D.Raycast(pos, dir, distance, ~layerMask);
			
			// hitPoint[i] = hit.point;
			// hitPointList.Add(hit.point);
			
			Vector2 debugRayInfo = hit.point - pos;
			Debug.DrawRay(pos, debugRayInfo, Color.green);
			
			// AddTriangle(i, pos, hit.point);
		}
	}
	
	void Cast () {
		Vector2 pos = transform.position;
		Vector2 dir;
		// float distance = Mathf.Infinity;
		float distance = 36;
		int layerMask = LayerMask.GetMask("Player");
		// int layerMask = LayerMask.GetMask("UI");
		
		hitPointList.Clear();
		hitPointDataList.Clear();
		
		for (int i = 0; i < corners.Count; i++)
		{
			
			dir = corners[i] - pos;
			
			// RaycastHit2D hit = Physics2D.Raycast(pos, dir, distance);
			RaycastHit2D hit = Physics2D.Raycast(pos, dir, distance, ~layerMask);
			
			CornerData temp = new CornerData();		
			temp.pos = hit.point;
			temp.angle = pos.AngleTo(hit.point);		
			hitPointDataList.Add(temp);
			
			Vector2 debugRayInfo = hit.point - pos;
			Debug.DrawRay(pos, debugRayInfo, Color.green);
			
			Vector2 offsetDir = dir;
			offsetDir = Quaternion.Euler(0, 0, -0.01f) * offsetDir;
			
			// RaycastHit2D hitAfter2 = Physics2D.Raycast(pos, offsetDir, distance);
			RaycastHit2D hitAfter2 = Physics2D.Raycast(pos, offsetDir, distance, ~layerMask);
			
			CornerData temp2 = new CornerData();		
			temp2.pos = hitAfter2.point;
			temp2.angle = pos.AngleTo(hitAfter2.point);		
			hitPointDataList.Add(temp2);
			
			Vector2 debugRayInfo2 = hitAfter2.point - pos;
			Debug.DrawRay(pos, debugRayInfo2, Color.red);
			
			Vector2 offsetDir2 = dir;
			offsetDir2 = Quaternion.Euler(0, 0, 0.01f) * offsetDir2;
			
			// RaycastHit2D hitAfter = Physics2D.Raycast(pos, offsetDir2, distance);
			RaycastHit2D hitAfter = Physics2D.Raycast(pos, offsetDir2, distance, ~layerMask);
			
			CornerData temp3 = new CornerData();		
			temp3.pos = hitAfter.point;
			temp3.angle = pos.AngleTo(hitAfter.point);		
			hitPointDataList.Add(temp3);
			
			Vector2 debugRayInfo3 = hitAfter.point - pos;
			Debug.DrawRay(pos, debugRayInfo3, Color.red);
		}
	}
	
	void SortHitPointsQuick () {
		
		// cornerDataList.Sort((a, b) => a.angle.CompareTo(b.angle));
		hitPointDataList.Sort((a, b) => b.angle.CompareTo(a.angle));
		// list.Sort((a, b) => a.x.CompareTo(b.x));
	}
	
	void SortHitPoints () {
		for (int i = 0; i < hitPointList.Count; i++)
		{
			CornerData temp = new CornerData();
			
			temp.pos = hitPointList[i];
			
			// temp.angle = Vector2.Angle(transform.position, hitPointList[i]);
			
			Vector2 pos = transform.position;
			temp.angle = pos.AngleTo(hitPointList[i]);
			
			// temp.angle = transform.position.AngleTo(hitPointList[i]);
			
			// float angle = (Mathf.Atan2(x, y) / Mathf.PI) * 180f;
			// if(angle < 0) 
			// {
				// angle += 360f;
			// }
			
			cornerDataList.Add(temp);
		}
		
		hitPointList.Clear();
		
		// cornerDataList.Sort((a, b) => a.angle.CompareTo(b.angle));
		cornerDataList.Sort((a, b) => b.angle.CompareTo(a.angle));
		// list.Sort((a, b) => a.x.CompareTo(b.x));
		
		for (int i = 0; i < cornerDataList.Count; i++)
		{
			hitPointList.Add(cornerDataList[i].pos);
		}
	}
	
	void MakeTriangles () {
		
		calculatedVertices.Clear();
		calculatedUV.Clear();
		calculatedTriangles.Clear();
		
		Vector2 pos = Vector2.zero;
		
		// for (int i = 0; i < hitPointList.Count; i++)
		for (int i = 0; i < hitPointDataList.Count; i++)
		{
			calculatedVertices.Add(pos);
			calculatedUV.Add(pos);
			
			calculatedTriangles.Add(calculatedTriangles.Count);
			
			// Vector2 firstHitPoint = hitPointList[i] - (Vector2)transform.position;
			Vector2 firstHitPoint = hitPointDataList[i].pos - (Vector2)transform.position;
			
			calculatedVertices.Add(firstHitPoint);
			calculatedUV.Add(firstHitPoint);
			
			calculatedTriangles.Add(calculatedTriangles.Count);
			
			int v = i+1;
			// if (v == hitPointList.Count)
			if (v == hitPointDataList.Count)
			{
				v = 0;
			}
			
			// Vector2 secondHitPoint = hitPointList[v] - (Vector2)transform.position;
			Vector2 secondHitPoint = hitPointDataList[v].pos - (Vector2)transform.position;
			
			calculatedVertices.Add(secondHitPoint);
			calculatedUV.Add(secondHitPoint);
			
			calculatedTriangles.Add(calculatedTriangles.Count);
		}
	}
	
	void AddTriangle (int triangleNum, Vector2 pos, Vector2 hitPoint) {
		
		Vector2 convertedPos = pos - (Vector2)transform.position;
		Vector2 convertedHitPoint = hitPoint - (Vector2)transform.position;
		
		int convertedID = triangleNum * 3;
		
		if (triangleNum == 0)
		{
			newVertices[newVertices.Length-1] = convertedHitPoint;
			newVertices[convertedID] = convertedPos;
			newVertices[convertedID + 1] = convertedHitPoint;
			
			newUV[newUV.Length-1] = convertedHitPoint;
			newUV[convertedID] = convertedPos;
			newUV[convertedID + 1] = convertedHitPoint;
			
		}else{
			
			newVertices[convertedID - 1] = convertedHitPoint;
			newVertices[convertedID] = convertedPos;
			newVertices[convertedID + 1] = convertedHitPoint;
			
			newUV[convertedID - 1] = convertedHitPoint;
			newUV[convertedID] = convertedPos;
			newUV[convertedID + 1] = convertedHitPoint;
		}
	}
	
	void AddTriangle (Vector2 pos, Vector2 hitPoint) {
		
		// Vector2 convertedPos = pos;
		// Vector2 convertedHitPoint = hitPoint;
		
		Vector2 convertedPos = pos - (Vector2)transform.position;
		Vector2 convertedHitPoint = hitPoint - (Vector2)transform.position;
		
		if (calculatedVertices.Count != 0)
		{
			calculatedVertices.Add(convertedHitPoint);
			calculatedUV.Add(convertedHitPoint);
			
			// calculatedTriangles.Add(calculatedTriangles.Count);
		}
		
		calculatedVertices.Add(convertedPos);
		calculatedVertices.Add(convertedHitPoint);
		
		calculatedUV.Add(convertedPos);
		calculatedUV.Add(convertedHitPoint);
		
		// calculatedTriangles.Add(calculatedTriangles.Count);
		// calculatedTriangles.Add(calculatedTriangles.Count);
	}
	
	void Draw () {
		
		mesh.Clear();

        // mesh.vertices = newVertices;
        // mesh.uv = newUV;
        // mesh.triangles = newTriangles; 
		
		mesh.vertices = calculatedVertices.ToArray();
		mesh.uv = calculatedUV.ToArray();
        mesh.triangles = calculatedTriangles.ToArray();
	}
	
	
	// void OnEnable () {
		// Messenger<GameObject>.AddListener("NewLevelLoaded", StartCastingShadow);
	// }
	
	// void OnDisable () {
		// Messenger<GameObject>.RemoveListener("NewLevelLoaded", StartCastingShadow);
	// }
}
