using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Promotional : MonoBehaviour {
    // Capture frames as a screenshot sequence. Images are
    // stored as PNG files in a folder - these can be combined into
    // a movie using image utility software (eg, QuickTime Pro).
    // The folder to contain our screenshots.
    // If the folder exists we will append numbers to create an empty folder.
    string folder = "Promotional/ScreenshotFolder";
    int frameRate = 25;
	
	private int sNum = 0;
	private string sFolder = "Promotional/InGame/";
    
    void Update () {
        if (Input.GetKeyDown("f9"))
		{
			TakeScreenshot();
		}
		
		if (Input.GetKeyDown("f5"))
		{
			StartCoroutine("Record");
		}
		
		if (Input.GetKeyDown("f6"))
		{
			StopCoroutine("Record");
		}
    }
	
	// void StartRecording () {
		
	// }
	
	IEnumerator Record () {
		
		Debug.Log("Starting Recording!");
		
		// Set the playback framerate (real time will not relate to game time after this).
        Time.captureFramerate = frameRate;
		
		// Create the folder
        System.IO.Directory.CreateDirectory(folder);
		
		while (true)
		{
			// Append filename to folder name (format is '0005 shot.png"')
			string name = string.Format("{0}/{1:D04} shot.png", folder, Time.frameCount );
			
			// Capture the screenshot to the specified file.
			ScreenCapture.CaptureScreenshot(name, 2);
			
			yield return null;
		}
		
		Debug.Log("Stopped Recording!");
	}
	
	public void TakeScreenshot () {
		string screenshotFilename = sFolder + "Screenshot" + sNum + ".png";
		while (System.IO.File.Exists(screenshotFilename))
		{
			sNum++;
			screenshotFilename = sFolder + "Screenshot" + sNum + ".png";
		}
	
		ScreenCapture.CaptureScreenshot(screenshotFilename, 2);
		Debug.Log("Screenshot " + sNum + " Captured!");
	}
}

