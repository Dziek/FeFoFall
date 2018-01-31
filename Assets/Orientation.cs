using UnityEngine;
using System.Collections;

public class Orientation : MonoBehaviour {

	public GameObject landscapeMenus;
	public GameObject portraitMenus;
	
	// public static orientation;
	
	void Awake () {
		landscapeMenus.SetActive(true);
		portraitMenus.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
		// if (Screen.orientation == ScreenOrientation.LandscapeLeft)
		// if (Input.deviceOrientation == DeviceOrientation.Portrait)
		if (Input.acceleration.x >= 0.5f)
		// if (Screen.height > Screen.width)
		{
			Camera.main.backgroundColor = Color.green;
			
			// landscapeMenus.SetActive(false);
			// portraitMenus.SetActive(true);
			// orientation = portrait;
			
			
		// }else if (Input.deviceOrientation == DeviceOrientation.LandscapeLeft){
		}else{
			Camera.main.backgroundColor = Color.red;
			
			// landscapeMenus.SetActive(true);
			// portraitMenus.SetActive(false);
			
		}
	}
}
