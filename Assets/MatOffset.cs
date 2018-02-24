using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatOffset : MonoBehaviour {
   
	public float scrollSpeed = 1;
    public Renderer rend;
    
	void Start() {
        rend = GetComponent<Renderer>();
    }
    
	void Update() {
        // float offset = Time.time * scrollSpeed;
        Vector2 offset = transform.position * -scrollSpeed;
		
        rend.material.SetTextureOffset("_MainTex", new Vector2(offset.x, offset.y));
    }
}
