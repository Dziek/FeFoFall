using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraViewportChange : MonoBehaviour {
	
	public bool useCurrentSize;
	public float startSize = 0.1f;
	public float endSize = 1f;
	
	public float speed = 1;
	
	public bool getRandomOffset;
	public bool getRandomSpeed;
	
	private float currentSize;
	private Camera camera;
	private Vector2 offset;
	
	private float minOffset = -0.25f;
	// private float minOffset = 0;
	private float maxOffset = 0.25f;
	
	void Awake () {
		camera = GetComponent<Camera>();
	}
	
	// Use this for initialization
	void Start () {
		currentSize = startSize;
		offset = Vector2.zero;
		
		if (useCurrentSize == true)
		{
			currentSize = camera.rect.width;
		}
		
		if (getRandomOffset == true)
		{
			CalculateOffset();
		}
		
		if (getRandomSpeed == true)
		{
			GetNewSpeed();
		}
		
		CalculateRect();
	}
	
	// Update is called once per frame
	void Update () {
		currentSize += speed * Time.deltaTime;
		if (currentSize > endSize)
		{
			currentSize = startSize;
			
			if (getRandomOffset == true)
			{
				CalculateOffset();
			}
			
			if (getRandomSpeed == true)
			{
				GetNewSpeed();
			}
		}
		
		CalculateRect();
	}
	
	void CalculateRect () {
		Rect r = camera.rect;
		
		float newPos = 0.5f - (currentSize / 2);
		// float newPos = 0;
		
		camera.rect = new Rect(newPos + offset.x, newPos + offset.y, currentSize, currentSize);
		
		/*
			0.00 = 0.5
			0.25 = 0.375
			0.5f = 0.25f
			0.75 = 0.125
			1 = 0
		*/
	}
	
	void CalculateOffset () {
		float newX = Random.Range(minOffset, maxOffset);
		float newY = Random.Range(minOffset, maxOffset);
		
		offset = new Vector2(newX, newY);
	}
	
	void GetNewSpeed () {
		speed = Random.Range(0.2f, 0.6f);
	}
}
