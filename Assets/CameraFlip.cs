using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFlip : MonoBehaviour {
	
	public bool flipX;
	public bool flipY;
	
	private Camera camera;
	
	// Use this for initialization
	void Start () {
		int x = flipX == false ? 1 : -1;
		int y = flipY == false ? 1 : -1;
		Vector3 flipVector = new Vector3(x, y, 1);
		
		camera = GetComponent<Camera>();
		
		Matrix4x4 mat = camera.projectionMatrix;
		mat *= Matrix4x4.Scale(flipVector);
		camera.projectionMatrix = mat;
	}
}
