using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour {
	
	// private IEnumerator shake;
	private Coroutine shake;
	
	private Vector3 camStartPos;
	
	private float intensity;
	
	// private bool shaking;
	
	// private Vector3 startCameraPos;
	
	private Coroutine constantCoroutine;
	
	void Awake () {
		// Messenger<float>.AddListener("screenshake", Shake);
		Messenger<float, float>.AddListener("screenshake", Shake);
		Messenger<float>.AddListener("StartConstantShake", StartConstantShake);
		Messenger.AddListener("StopConstantShake", StopConstantShake);
		
		camStartPos = Camera.main.transform.position;
		// startCameraPos = Camera.main.transform.position;
	}
	
	void OnDestroy () {
		// Messenger<float>.RemoveListener("screenshake", Shake);
		Messenger<float, float>.RemoveListener("screenshake", Shake);
		Messenger<float>.RemoveListener("StartConstantShake", StartConstantShake);
		Messenger.RemoveListener("StopConstantShake", StopConstantShake);
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
		
		// shaking = true;
		
		Shake(intensity);
		// Shake(5, 0.2f);
	}
	
	public void Shake (float intensity = 2, float time = 0.5f) {
	// public void Shake (float intensity = 2) {
		
		// float time = 0.08f;
		
		// StopCoroutine(ShakeCamera(intensity, time));
		// StartCoroutine(ShakeCamera(intensity, time));
		
		shake = StartCoroutine(ShakeCamera(intensity, time));
		
		// shake = ShakeCamera(intensity, time);
		
		// StopCoroutine(shake);
		// StartCoroutine(shake);
		
		
	}
	
	void StartConstantShake (float intensity) {
		constantCoroutine = StartCoroutine(ShakeCameraConstantly(intensity));
	}
	
	void StopConstantShake () {
		if (constantCoroutine != null)
		{
			StopCoroutine(constantCoroutine);
		}
		
		constantCoroutine = null;
		Camera.main.transform.position = camStartPos;
		// Camera.main.transform.position = startCameraPos;
	}	
	
	IEnumerator ShakeCamera (float intensity, float shakeTime) {
		
		// shaking = true;
		
		// camStartPos = Camera.main.transform.position;
		
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
	
	IEnumerator ShakeCameraConstantly (float intensity) {
		
		// shaking = true;
		
		// camStartPos = Camera.main.transform.position;
		// startCameraPos = Camera.main.transform.position;
		
		while (true)
		{		
			float shakeAmountX = (Random.value * intensity * 2) - intensity;
			float shakeAmountY = (Random.value * intensity * 2) - intensity;
			float shakeAmountZ = (Random.value * intensity * 2) - intensity;
			
			Camera.main.transform.position = camStartPos + new Vector3(shakeAmountX, shakeAmountY, shakeAmountZ);
			yield return null;
		}
		
		// shaking = false;
		Camera.main.transform.position = camStartPos;	
	}
}
