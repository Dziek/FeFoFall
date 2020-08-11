using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class Testing : MonoBehaviour {
	
	public static bool t1 = false;
	public static bool t2 = false;
	public static bool t3 = false;
	
	public AudioMixerGroup audioMixerGroup;
	
	// private int sNum = 0;
	// private string sFolder = "Promotional/InGame/";
	
	private bool muted;
	
	// Update is called once per frame
	void Update () {
		if (Input.GetButtonDown("Reset"))
		{
			Restart();
		}
		
		if (Input.GetKeyDown(KeyCode.Alpha1))
		{
			t1 = !t1;
			Debug.Log("Changing t1 to " + t1);
		}
		
		if (Input.GetKeyDown(KeyCode.Alpha2))
		{
			t2 = !t2;
			Debug.Log("Changing t2 to " + t2);
		}
		
		if (Input.GetKeyDown(KeyCode.Alpha3))
		{
			t3 = !t3;
			Debug.Log("Changing t3 to " + t3);
		}
		
		if (Input.GetKeyDown(KeyCode.Alpha0))
		{
			t1 = false;
			t2 = false;
			t3 = false;
			Debug.Log("Changing t's to false");
		}
		
		if (Input.GetKeyDown("escape"))
		{
			// Restart();
			// Application.Quit();
		}
		
		// if (Input.GetKeyDown("f9"))
		// {
			// TakeScreenshot();
		// }
		
		if (Input.GetButtonDown("Fire3"))
		{
			Debug.Log("Muting Level Audio");
			MuteLevelAudio();
		}
		
		if (Input.GetButtonDown("Jump"))
		{
			Debug.Log("Reset Camera Settings");
			ResetCameraSettings();
		}
		
		if (Input.GetKeyDown("l"))
		{
			Debug.Log("Skipping level");
			
			Messenger.Broadcast("Success");
			GameStates.ChangeState("Transition", "Good");
			Messenger<TransitionReason>.Broadcast("Transition", TransitionReason.levelSuccess);
		}
	}
	
	public void Restart () {
		Application.LoadLevel(Application.loadedLevel);
	}
	
	// public void TakeScreenshot () {
		// string screenshotFilename = sFolder + "Screenshot" + sNum + ".png";
		// while (System.IO.File.Exists(screenshotFilename))
		// {
			// sNum++;
			// screenshotFilename = sFolder + "Screenshot" + sNum + ".png";
		// }
	
		// ScreenCapture.CaptureScreenshot(screenshotFilename, 2);
		// Debug.Log("Screenshot " + sNum + " Captured!");
	// }
	
	public void MuteLevelAudio () {
		if (muted == false)
		{
			audioMixerGroup.audioMixer.SetFloat("LevelVolume", -80f);
		}else{
			audioMixerGroup.audioMixer.SetFloat("LevelVolume", 0f);
		}
		
		muted = !muted;
	}
	
	public void ResetCameraSettings () {
		Camera.main.orthographicSize = 5;
	}
}
