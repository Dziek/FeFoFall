using UnityEngine;
using System.Collections;

public class Resize : MonoBehaviour {

	// Use this for initialization
	void Start () {
		// Renderer sr=GetComponent<Renderer>();
		// if(sr==null) return;

		// transform.localScale=new Vector3(1,1,1);

		// float width=sr.bounds.size.x;
		// float height=sr.bounds.size.y;


		// float worldScreenHeight=Camera.main.orthographicSize*2f;
		// float worldScreenWidth=worldScreenHeight/Screen.height*Screen.width;

		// Vector3 xWidth = transform.localScale;
		// xWidth.x=worldScreenWidth / width;
		// transform.localScale=xWidth;

		// Vector3 yHeight = transform.localScale;
		// yHeight.y=worldScreenHeight / height;
		// transform.localScale=yHeight;	
		
		// transform.localScale = transform.localScale * Camera.main.aspect;
		// transform.localScale = new Vector3 (transform.localScale.y, transform.localScale.x / Camera.main.aspect, 1);
		
		// Compensate();
	}
		
	// Update is called once per frame
	void Update () {
		// transform.localScale = new Vector3 (transform.localScale.y, transform.localScale.x / Camera.main.aspect, 1);
	}
	
	void Compensate () {
		float targetRatio = 16f/9f;
		if (targetRatio > Camera.main.aspect)
		{
			float newY = transform.localScale.y / (targetRatio / Camera.main.aspect);
			transform.localScale = new Vector3 (transform.localScale.x, newY, 1);
			// Debug.Log("DID IT");
		}
	}
}
