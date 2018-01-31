using UnityEngine;
using System.Collections;

public class OrientationAlt : MonoBehaviour {
	
	public GameObject[] goToRotate;
	
	private bool canRotate;
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		// if (Screen.orientation == ScreenOrientation.LandscapeLeft)
		// if (Input.deviceOrientation == DeviceOrientation.Portrait && canRotate)
		if (Input.acceleration.x >= 0.7f && canRotate)
		// if (Screen.height > Screen.width)
		{
			for (int i = 0; i < goToRotate.Length; i++)
			{
				goToRotate[i].transform.eulerAngles = new Vector3(0, 0, 90);
				// goToRotate[i].transform.RotateAround(Vector3.zero, Vector3.forward, 90);
			}
			
			canRotate = false;
		// }else if (Input.deviceOrientation == DeviceOrientation.LandscapeLeft){
		}else if (Input.acceleration.x < 0.3f){
		// }else{
			for (int i = 0; i < goToRotate.Length; i++)
			{
				goToRotate[i].transform.rotation = Quaternion.identity;
			}
			
			canRotate = true;
		}
		
		
		
		// for (int i = 0; i < goToRotate.Length; i++)
		// {
			// // goToRotate[i].transform.eulerAngles = new Vector3(0, 0, 90);
			// // goToRotate[i].transform.RotateAround(Vector3.zero, 180);
			// goToRotate[i].transform.RotateAround(Vector3.zero, Vector3.forward, 90);
		// }
	}
}
