using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenWrap : MonoBehaviour {

	public static float screenWidthInUnits = 18;
	public static float screenHeightInUnits = 10.25f; // without UI border
	// public static float screenHeightInUnits = 7.15f; // with UI border
	
	// Update is called once per frame
	void Update () {
		CheckForLoopingScreen();
	}
	
	void CheckForLoopingScreen () {
		if (Mathf.Abs(transform.position.x) > screenWidthInUnits/2)
		{
			transform.Translate(Vector2.right * Mathf.Sign(transform.position.x) * -(screenWidthInUnits - 0.1f), Space.World);
		}
		
		if (Mathf.Abs(transform.position.y) > screenHeightInUnits/2)
		{
			transform.Translate(Vector2.up * Mathf.Sign(transform.position.y) * -(screenHeightInUnits - 0.05f), Space.World);
		}
	}
}
