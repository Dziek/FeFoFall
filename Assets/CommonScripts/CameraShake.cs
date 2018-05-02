using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour {
	
	private Coroutine shake;
	
	private Vector3 camStartPos;
	
	private float intensity;
	
	private Coroutine constantCoroutine;
	
	void Awake () {
		// Messenger<float>.AddListener("screenshake", Shake);
		Messenger<float, float>.AddListener("screenshake", Shake);
		Messenger<float>.AddListener("StartConstantShake", StartConstantShake);
		Messenger.AddListener("StopConstantShake", StopConstantShake);
		
		camStartPos = Camera.main.transform.position;
	}
	
	void OnDestroy () {
		// Messenger<float>.RemoveListener("screenshake", Shake);
		Messenger<float, float>.RemoveListener("screenshake", Shake);
		Messenger<float>.RemoveListener("StartConstantShake", StartConstantShake);
		Messenger.RemoveListener("StopConstantShake", StopConstantShake);
	}
	
	public void Shake (float intensity = 2, float time = 0.5f) {
		shake = StartCoroutine(ShakeCamera(intensity, time));
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
	}	
	
	IEnumerator ShakeCamera (float intensity, float shakeTime) {
		
		float t = 0;
		
		while (t < shakeTime)
		{
			float shakeAmountX = (Random.value * intensity * 2) - intensity;
			float shakeAmountY = (Random.value * intensity * 2) - intensity;
			float shakeAmountZ = (Random.value * intensity * 2) - intensity;
			
			Camera.main.transform.position = camStartPos + new Vector3(shakeAmountX, shakeAmountY, shakeAmountZ);
			
			t += Time.deltaTime;
			yield return null;
		}
		
		Camera.main.transform.position = camStartPos;
	}
	
	IEnumerator ShakeCameraConstantly (float intensity) {
		
		while (true)
		{		
			float shakeAmountX = (Random.value * intensity * 2) - intensity;
			float shakeAmountY = (Random.value * intensity * 2) - intensity;
			float shakeAmountZ = (Random.value * intensity * 2) - intensity;
			
			Camera.main.transform.position = camStartPos + new Vector3(shakeAmountX, shakeAmountY, shakeAmountZ);
			yield return null;
		}
		
		Camera.main.transform.position = camStartPos;	
	}
}
