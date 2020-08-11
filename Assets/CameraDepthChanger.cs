using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Quick script used to fix CameraViewportChange. Script set up to work alongside that, and probably can't function outside of that very specific usecase :(

public class CameraDepthChanger : MonoBehaviour {

	public bool useCurrentDepth;
	public float lowestDepth = 1;
	public float highestDepth = 3;
	
	public float speed = 1;
	
	public float counter = 0.1f;
	private float currentDepth;
	private Camera camera;
	
	void Awake () {
		camera = GetComponent<Camera>();
	}
	
	// Use this for initialization
	void Start () {
		currentDepth = lowestDepth;
		
		if (useCurrentDepth == true)
		{
			
			currentDepth = camera.depth;
		}
		
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
		currentDepth--;
			
		if (currentDepth < lowestDepth)
		{
			currentDepth = highestDepth;
		}
		
		// currentDepth++;
			
		// if (currentDepth > highestDepth)
		// {
			// currentDepth = lowestDepth;
		// }
		
		SetDepth();
	}
	
	void SetDepth () {
		camera.depth = currentDepth;
	}
}
