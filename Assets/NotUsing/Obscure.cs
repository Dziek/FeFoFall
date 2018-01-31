using UnityEngine;
using System.Collections;

public class Obscure : MonoBehaviour {

	// Use this for initialization
	void Start () {
		// get all renderers in this object and its children:
		Renderer[] renders = GetComponentsInChildren<Renderer>();
		
		foreach (Renderer r in renders)
		{
			r.material.renderQueue = 4002; // set their renderQueue
			Debug.Log("D");
		}
		
		CanvasRenderer[] canvasRenders = GetComponentsInChildren<CanvasRenderer>();
		
		foreach (CanvasRenderer r in canvasRenders)
		{
			// r.material.renderQueue = 4002; // set their renderQueue
			// r.relativeDepth = 0;
			Debug.Log("F");
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
