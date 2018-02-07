using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour {
	
	// private IEnumerator shake;
	private Coroutine shake;
	
	private Vector3 camStartPos;
	
	private float intensity;
	
	private bool shaking;
	
	void Awake () {
		Messenger<float>.AddListener("screenshake", Shake);
		
		camStartPos = Camera.main.transform.position;
	}
	
	void OnDestroy () {
		Messenger<float>.RemoveListener("screenshake", Shake);
	}
	
	void Start () {
		// InvokeRepeating("Shake2", Random.Range(0.5f, 1.5f), Random.Range(1f, 1.5f));
		// InvokeRepeating("Shake2", 1, 1);
	}
	
	public void RepeatShake (int i) {
		intensity = i;
		
		InvokeRepeating("Shake2", Random.Range(0.5f, 1.5f), Random.Range(1f, 1.5f));
	}
	
	void Update () {
		// if (Input.GetKeyDown("s"))
		// {
			// Shake();
		// }
	}
	
	void Shake2 () {
		
		shaking = true;
		
		Shake(intensity);
		// Shake(5, 0.2f);
	}
	
	// public void Shake (float intensity = 2, float time = 0.5f) {
	public void Shake (float intensity = 2) {
		
		float time = 0.5f;
		
		// StopCoroutine(ShakeCamera(intensity, time));
		// StartCoroutine(ShakeCamera(intensity, time));
		
		shake = StartCoroutine(ShakeCamera(intensity, time));
		
		// shake = ShakeCamera(intensity, time);
		
		// StopCoroutine(shake);
		// StartCoroutine(shake);
		
		
	}
	
	IEnumerator ShakeCamera (float intensity, float shakeTime) {
		
		// shaking = true;
		
		camStartPos = Camera.main.transform.position;
		
		float t = 0;
		
		while (t < shakeTime)
		{
			// float quakeAmt = Random.value*shakeAmt*2 - shakeAmt;
            // Vector3 pp = mainCamera.transform.position;
            // pp.y+= quakeAmt; // can also add to x and/or z
            // mainCamera.transform.position = pp;
			
			float shakeAmountX = (Random.value * intensity * 2) - intensity;
			float shakeAmountY = (Random.value * intensity * 2) - intensity;
			float shakeAmountZ = (Random.value * intensity * 2) - intensity;
			
			Camera.main.transform.position = camStartPos + new Vector3(shakeAmountX, shakeAmountY, shakeAmountZ);
			
			t += Time.deltaTime;
			yield return null;
		}
		
		// shaking = false;
		Camera.main.transform.position = camStartPos;
		
	}
}
