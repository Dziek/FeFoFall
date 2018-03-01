using UnityEngine;
using System.Collections;

public class LevelInfo : MonoBehaviour {
	
	public string name;
	public string description;
	
	public bool canFlip = true;
	
	public Color background;
	
	void OnEnable () {
		// if (background != null)
		// {
			// Camera.main.backgroundColor = background;
		// }
		
		if (canFlip == true)
		{
			// float x = Random.Range(0, 2) == 0 ? 1 : -1;
			// float y = Random.Range(0, 2) == 0 ? 1 : -1;
			// transform.localScale = new Vector3(x, y, 1);
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
