using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Testing : MonoBehaviour {
	
	private int sNum = 0;
	private string sFolder = "Promotional/InGame/";
	
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
		
		if (Input.GetKeyDown("f9"))
		{
			TakeScreenshot();
		}
	}
	
	public void Restart () {
		// Application.LoadLevel(Application.loadedLevel);
	}
	
	public void TakeScreenshot () {
		string screenshotFilename = sFolder + "Screenshot" + sNum + ".png";
		while (System.IO.File.Exists(screenshotFilename))
		{
			sNum++;
			screenshotFilename = sFolder + "Screenshot" + sNum + ".png";
		}
	
		ScreenCapture.CaptureScreenshot(screenshotFilename, 6);
		Debug.Log("Screenshot " + sNum + " Captured!");
	}
}
