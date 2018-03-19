using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class Testing : MonoBehaviour {
	
	public AudioMixerGroup audioMixerGroup;
	
	// private int sNum = 0;
	// private string sFolder = "Promotional/InGame/";
	
	private bool muted;
	
	// Update is called once per frame
	void Update () {
		// if (Input.GetButtonDown("Reset"))
		// {
			// Restart();
		// }
		
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
			MuteLevelAudio();
		}
		
		if (Input.GetButtonDown("Jump"))
		{
			ResetCameraSettings();
		}
	}
	
	public void Restart () {
		// Application.LoadLevel(Application.loadedLevel);
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
