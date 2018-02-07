using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Testing : MonoBehaviour {

	// Update is called once per frame
	void Update () {
		// if (Input.GetButtonDown("Reset"))
		// {
			// Restart();
		// }
		
		if (Input.GetKeyDown("escape"))
		{
			Restart();
			// Application.Quit();
		}
	}
	
	public void Restart () {
		Application.LoadLevel(Application.loadedLevel);
	}
}
