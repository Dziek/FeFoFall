using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// for if this breaks / is fixed again https://forum.unity.com/threads/render-texture-not-tiling-wrap-mode-issue.634420/
// I used the anti-aliasing change, from none to 2. Who knows what the ramifications are!

public class RawImageScroller : MonoBehaviour {
	
	public float scrollSpeed = 3;
	
	public bool scrollX = true;
	public bool scrollY = false;
	public bool scrollW = false;
	public bool scrollH = false;
	
	private float xLimit = 2;
	private float yLimit = 4;
	private float widthLimit = 3;
	private float heightLimit = 3;
	
	private float xDirection = 1;
	private float yDirection = 1;
	private float widthDirection = 1;
	private float heightDirection = 1;
	
	private RawImage rawImage;
	
	// Use this for initialization
	void Start () {
		rawImage = GetComponent<RawImage>();
	}
	
	// Update is called once per frame
	void Update () {
		if (scrollX == true)
		{
			Rect t = rawImage.uvRect;
			rawImage.uvRect = new Rect(t.x + ((scrollSpeed * Time.deltaTime) * xDirection), t.y, t.width, t.height);
			
			if (t.x > xLimit)
			{
				xDirection = -1;
			}
			
			// if (t.x < 0.5f)
			if (t.x < 0f)
			{
				xDirection = 1;
			}
		}
		
		if (scrollY == true)
		{
			Rect t = rawImage.uvRect;
			rawImage.uvRect = new Rect(t.x, t.y + ((scrollSpeed * Time.deltaTime) * yDirection), t.width, t.height);
			
			if (t.y > yLimit)
			{
				yDirection = -1;
			}
			
			// if (t.y < 0.5f)
			if (t.y < 1f)
			{
				yDirection = 1;
			}
		}
		
		if (scrollW == true)
		{
			Rect t = rawImage.uvRect;
			rawImage.uvRect = new Rect(t.x, t.y, t.width + ((scrollSpeed * Time.deltaTime) * widthDirection), t.height);
			
			if (t.width > widthLimit)
			{
				widthDirection = -1;
			}
			
			// if (t.width < 0.5f)
			if (t.width < 1f)
			{
				widthDirection = 1;
			}
		}
		
		if (scrollH == true)
		{
			Rect t = rawImage.uvRect;
			rawImage.uvRect = new Rect(t.x, t.y, t.width, t.height + ((scrollSpeed * Time.deltaTime) * heightDirection));
			
			if (t.height > heightLimit)
			{
				heightDirection = -1;
			}
			
			// if (t.height < 0.5f)
			if (t.height < 1f)
			{
				heightDirection = 1;
			}
		}
		
	}
}
