using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class TouchControl : MonoBehaviour {
	
	public GameObject[] controlSchemes;
	
	static PlayerControl playerScript;
	
	private int currentControls = 0;
	
	// Use this for initialization
	void Awake () {
		#if UNITY_ANDROID
			controlSchemes[0].SetActive(true);
			for (int i = 1; i < controlSchemes.Length; i++)
			{
				controlSchemes[i].SetActive(false);
			}
		#endif
	}
	
	public static void UpdatePlayerObject(PlayerControl pS) {
		playerScript = pS;
	}
	
	public void SendToPlayer (string dir) {
		if (playerScript != null)
		{
			playerScript.RegisterInput(dir);
		}
	}
	
	public void CycleControls () {
		controlSchemes[currentControls].SetActive(false);
		
		if (currentControls == controlSchemes.Length-1)
		{
			currentControls = 0;
		}else{
			currentControls++;
		}
		
		controlSchemes[currentControls].SetActive(true);
		
		// if (currentControls == 2)
		// {
			// Camera.main.transform.position = new Vector3(1.3f, 0, -10);
		// }else if (currentControls == 3)
		// {
			// Camera.main.transform.position = new Vector3(-1.3f, 0, -10);
		// }else{
			// Camera.main.transform.position = new Vector3(0, 0, -10);
		// }
		
		switch (currentControls)
		{
			case 2:
				Camera.main.transform.position = new Vector3(1.3f, 0, -10);
			break;
			
			case 3:
				Camera.main.transform.position = new Vector3(-1.3f, 0, -10);
			break;
			
			default:
				Camera.main.transform.position = new Vector3(0, 0, -10);
			break;
		}
	}
}
