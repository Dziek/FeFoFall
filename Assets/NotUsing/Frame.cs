using UnityEngine;
using System.Collections;

public class Frame : MonoBehaviour {

	public GameObject frameUp;
	public GameObject frameDown;
	public GameObject frameLeft;
	public GameObject frameRight;
	
	private float zPos;
	
	// Use this for initialization
	void Start () {
		// Vector3 pos = new Vector3(0, Screen.height/2, 2);
		// transform.position = Camera.main.ScreenToWorldPoint(pos);
		
		// zPos = 2;
		zPos = Mathf.Abs(Camera.main.transform.position.z - transform.position.z);
		// Debug.Log(zPos);
		
		// UpdateFramePositions();
		
		// Camera.main.aspect = 16f/9f;
	}
	
	// Update is called once per frame
	void Update () {
		// Vector3 pos = new Vector3(0, Screen.height/2, 2);
		// transform.position = Camera.main.ScreenToWorldPoint(pos);
		
		// UpdateFramePositions();
		
		// Debug.Log(Camera.main.aspect);
	}
	
	public void UpdateFramePositions () {
		if (frameUp != null)
		{
			Vector3 pos = new Vector3(Screen.width/2, Screen.height, zPos);
			frameUp.transform.position = Camera.main.ScreenToWorldPoint(pos);
		}
		
		if (frameDown != null)
		{
			Vector3 pos = new Vector3(Screen.width/2, 0, zPos);
			frameDown.transform.position = Camera.main.ScreenToWorldPoint(pos);
		}
		
		if (frameLeft != null)
		{
			Vector3 pos = new Vector3(0, Screen.height/2, zPos);
			frameLeft.transform.position = Camera.main.ScreenToWorldPoint(pos);
		}
		
		if (frameRight != null)
		{
			Vector3 pos = new Vector3(Screen.width, Screen.height/2, zPos);
			frameRight.transform.position = Camera.main.ScreenToWorldPoint(pos);
		}
	}
}
