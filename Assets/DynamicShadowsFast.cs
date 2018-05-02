using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicShadowsFast : MonoBehaviour {
	
	// public GameObject[] frames;
	
	private int amount = 36;
	
	private struct CornerData {
		public Vector2 pos;
		public float angle;
	}
	
	// public List<Vector2> corners = new List<Vector2>();
	public Vector2[] cornerPositions;
	private int cP_POS;
	// private List<CornerData> cornerDataList = new List<CornerData>();
	
	// private Vector2[] hitPoints;
	private List<CornerData> hitPointDataList = new List<CornerData>();
	
	public Vector3[] newVertices;
    public Vector2[] newUV;
    public int[] newTriangles;
	
	// public List<Vector3> calculatedVertices = new List<Vector3>();
	// public List<Vector2> calculatedUV = new List<Vector2>();
	// public List<int> calculatedTriangles = new List<int>();
	
	public Vector3[] calculatedVertices;
	public Vector2[] calculatedUV;
	public int[] calculatedTriangles;
	
	private Mesh mesh;
	
	private GameObject frame;
	private GameObject level;
	
	private Collider2D[] levelColliders;
	
	private Vector2 screenBounds = new Vector2(8.86f, 4.969642f);
	
	void Awake () {
		
		// hitPoints = new Vector2[amount];
		cornerPositions = new Vector2[amount];
		
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
		
		// levelColliders = level.GetComponentsInChildren<Collider2D>();
		
		// Try to avoid casting to players, might need tweaking for multiple players
		Collider2D[] temp = level.GetComponentsInChildren<Collider2D>();
		
		List<Collider2D> tempList = new List<Collider2D>();
		
		for (int i = 0; i < temp.Length; i++)
		{
			if (temp[i].gameObject.tag != "Player" && temp[i].gameObject.tag != "Trigger")
			// if (temp[i].gameObject != transform.parent.parent)
			{
				tempList.Add(temp[i]);
			}
		}
		
		levelColliders = tempList.ToArray();
		
		// four corners of each collider plus screen corners
		int arrayLength = (levelColliders.Length * 4) + 4;
		
		// hitPoints = new Vector2[arrayLength];
		cornerPositions = new Vector2[arrayLength];
		
		// times 3 for hitPointList then 3 again for whatever
		arrayLength = arrayLength * 9;
		
		calculatedVertices = new Vector3[arrayLength];
		calculatedUV = new Vector2[arrayLength];
		calculatedTriangles = new int[arrayLength];
		
		AddScreen();
		
		// GetCorners();
		// Cast();
		// SortHitPointsQuick();
		// MakeTriangles();
		// Draw();
    }
	
	// Update is called once per frame
	void LateUpdate () {
		// corners.Clear();
		
		if (level != null)
		{
			GetCorners();
			Cast();
			SortHitPointsQuick();
			MakeTriangles();
			Draw();
		}
	}
	
	void GetCorners () {
		
		// corners.Clear();
		cP_POS = 4;
		
		// AddScreen();
		// AddFrame();
		GetLevelBits();
	}
	
	void AddScreen () {
		// float x = 8.86f;
		// float y = 4.969642f;
		
		float x = screenBounds.x;
		float y = screenBounds.y;
		
		// corners.Add(new Vector2(-x, -y));
		// corners.Add(new Vector2(-x, y));
		// corners.Add(new Vector2(x, y));
		// corners.Add(new Vector2(x, -y));
		
		cornerPositions[0] = new Vector2(-x, -y);
		cornerPositions[1] = new Vector2(-x, y);
		cornerPositions[2] = new Vector2(x, y);
		cornerPositions[3] = new Vector2(x, -y);
	}
	
	void GetLevelBits () {
		
		Vector2 pos;
		// Collider2D[] cols = level.GetComponentsInChildren<Collider2D>();
		Collider2D[] cols = levelColliders;
		
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
	
	void AddCornerPos (Vector2 pos) {
		
		float x = screenBounds.x;
		float y = screenBounds.y;
		
		// Vector2 clampedPos = pos;
		
		Vector2 clampedPos = new Vector2(Mathf.Clamp(pos.x, -x, x), Mathf.Clamp(pos.y, -y, y));
		
		// corners.Add(clampedPos);
		
		cornerPositions[cP_POS] = clampedPos;
		cP_POS++;
	}
	
	void Cast () {
		Vector2 pos = transform.position;
		Vector2 dir;
		// float distance = Mathf.Infinity;
		float distance = 36;
		int layerMask = LayerMask.GetMask("Player") | LayerMask.GetMask("Trigger");
		// int layerMask = LayerMask.GetMask("UI");
		
		hitPointDataList.Clear();
		
		// for (int i = 0; i < corners.Count; i++)
		for (int i = 0; i < cornerPositions.Length; i++)
		{
			
			// dir = corners[i] - pos;
			dir = cornerPositions[i] - pos;
			
			// RaycastHit2D hit = Physics2D.Raycast(pos, dir, distance);
			RaycastHit2D hit = Physics2D.Raycast(pos, dir, distance, ~layerMask);
			
			// if (Mathf.Abs(Vector2.Distance(cornerPositions[i], pos) - Vector2.Distance(hit.point, pos)) > 1f)
			// {
				// Vector2 debugRayInfoT = hit.point - pos;
				// Debug.DrawRay(pos, debugRayInfoT, Color.green);
				// continue;
			// }
			
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
		
		hitPointDataList.Sort((a, b) => b.angle.CompareTo(a.angle));
	}
	
	void MakeTriangles () {
		
		// calculatedVertices.Clear();
		// calculatedUV.Clear();
		// calculatedTriangles.Clear();
		
		Vector2 pos = Vector2.zero;
		
		int arrayPos = 0;
		
		for (int i = 0; i < hitPointDataList.Count; i++)
		{
			// calculatedVertices.Add(pos);
			// calculatedUV.Add(pos);
			
			// calculatedTriangles.Add(calculatedTriangles.Count);
			
			calculatedVertices[arrayPos] = pos;
			calculatedUV[arrayPos] = pos;
			
			calculatedTriangles[arrayPos] = arrayPos;
			arrayPos++;
			
			Vector2 firstHitPoint = hitPointDataList[i].pos - (Vector2)transform.position;
			
			// calculatedVertices.Add(firstHitPoint);
			// calculatedUV.Add(firstHitPoint);
			
			// calculatedTriangles.Add(calculatedTriangles.Count);
			
			calculatedVertices[arrayPos] = firstHitPoint;
			calculatedUV[arrayPos] = firstHitPoint;
			
			calculatedTriangles[arrayPos] = arrayPos;
			arrayPos++;
			
			int v = i+1;
			if (v == hitPointDataList.Count)
			{
				v = 0;
			}
			
			Vector2 secondHitPoint = hitPointDataList[v].pos - (Vector2)transform.position;
			
			// calculatedVertices.Add(secondHitPoint);
			// calculatedUV.Add(secondHitPoint);
			
			// calculatedTriangles.Add(calculatedTriangles.Count);
			
			calculatedVertices[arrayPos] = secondHitPoint;
			calculatedUV[arrayPos] = secondHitPoint;
			
			calculatedTriangles[arrayPos] = arrayPos;
			arrayPos++;
		}
	}
	
	void Draw () {
		
		mesh.Clear();

        // mesh.vertices = newVertices;
        // mesh.uv = newUV;
        // mesh.triangles = newTriangles; 
		
		// mesh.vertices = calculatedVertices.ToArray();
		// mesh.uv = calculatedUV.ToArray();
        // mesh.triangles = calculatedTriangles.ToArray();
		
		mesh.vertices = calculatedVertices;
		mesh.uv = calculatedUV;
        mesh.triangles = calculatedTriangles;
	}
	
	
	// void OnEnable () {
		// Messenger<GameObject>.AddListener("NewLevelLoaded", StartCastingShadow);
	// }
	
	// void OnDisable () {
		// Messenger<GameObject>.RemoveListener("NewLevelLoaded", StartCastingShadow);
	// }
}
