using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FourCameras : MonoBehaviour {
	
	public float firstWait = 3;
	public float repeatRate = 1;
	
	private Camera[] cameras;
	private int lastR = -10;
	
	// Use this for initialization
	void Start () {
		cameras = GetComponentsInChildren<Camera>();	
	}
	
	void OnEnable () {	
		Messenger.AddListener("FirstMovement", StartShenanigans);
	}
	
	void OnDisable() {	
		Messenger.RemoveListener("FirstMovement", StartShenanigans);
	}
	
	void StartShenanigans () {
		InvokeRepeating("DisableCamera", firstWait, repeatRate);
	}
	
	void DisableCamera () {
		
		for (int i = 0; i < cameras.Length; i++)
		{
			cameras[i].enabled = false;
		}
		
		int r = Random.Range(0, cameras.Length);
		
		while (r == lastR)
		{
			r = Random.Range(0, cameras.Length);
		}
		
		lastR = r;
		
		cameras[r].enabled = true;
	}
	
}
