using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GameSettings : MonoBehaviour {
	
	public float cameraSize = 5; //4.845f is good too!
	
	public GameObject screenBoundsScreenGO;
	public SpriteRenderer borderSR;
	
	private EventSystem mainEventSystem;
	
	// Use this for initialization
	void Start () {
		float savedCameraSize = PlayerPrefs.GetFloat("CameraSize");
		
		// Debug.Log(savedCameraSize);
		
		if (savedCameraSize == 0)
		{
			// SetCameraSize();
		}
	}
	
	public void SetCameraSize () {
		StartCoroutine("ChangeCameraSize");
	}
	
	IEnumerator ChangeCameraSize () {
		screenBoundsScreenGO.SetActive(true);
		borderSR.color = Color.red;
		
		mainEventSystem = EventSystem.current;
		GameObject currentSelected = mainEventSystem.currentSelectedGameObject;
		mainEventSystem.enabled = false;
		
		while (Input.GetButton("Submit"))
		{
			yield return null;
		}
		
		while (true)
		{
			if (Mathf.Abs(Input.GetAxisRaw("Vertical")) > 0.5f)
			{
				Camera.main.orthographicSize += 0.02f * Mathf.Sign(Input.GetAxisRaw("Vertical"));
				Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize, 1.5f, 10);
			}		
			
			if (Input.GetButtonDown("Submit"))
			{
				screenBoundsScreenGO.SetActive(false);
				borderSR.color = Color.white;
				
				mainEventSystem.enabled = true;
				mainEventSystem.SetSelectedGameObject(currentSelected);
				
				yield break;
			}
			
			yield return null;
		}
	}
	
	void SaveCameraSize () {
		PlayerPrefs.SetFloat("CameraSize", cameraSize);
	}
}
