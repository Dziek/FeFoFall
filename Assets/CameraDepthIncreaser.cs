using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// One time quick fix script to fix a stackedcamera level. Fixes depth glitches that are introduced by CameraViewportChange.

public class CameraDepthIncreaser : MonoBehaviour {
	
	public float speed = 1;
	public float increaseAmount = 1;
	
	public float counter = 0.1f;
	private float currentDepth;
	private Camera camera;
	
	void Awake () {
		camera = GetComponent<Camera>();
	}
	
	// Use this for initialization
	void Start () {

		currentDepth = camera.depth;
		
		SetDepth();
	}
	
	// Update is called once per frame
	void Update () {
			
		counter += speed * Time.deltaTime;
		if (counter > 1)
		{
			counter = 0.1f;
			
			ChangeDepth();
		}
		
		
	}
	
	public void ChangeDepth () {
		currentDepth += increaseAmount;
		
		SetDepth();
	}
	
	void SetDepth () {
		camera.depth = currentDepth;
	}
}
