using UnityEngine;
using System.Collections;

public class LevelInfo : MonoBehaviour {
	
	public string name;
	public string description;
	public Color background;
	
	void OnEnable () {
		// Debug.Log(description);
		if (background != null)
		{
			Camera.main.backgroundColor = background;
		}
	}
	
	public void GetInfo (out string n, out string d) {
		// return description;
		// return "W";
		n = name;
		d = description;
		// return;
	}
}
