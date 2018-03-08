using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Extension {

	public static float AngleTo(this Vector2 this_, Vector2 to)
	{
		 Vector2 direction = to - this_;
		 float angle = Mathf.Atan2(direction.y,  direction.x) * Mathf.Rad2Deg;
		 if (angle < 0f) angle += 360f;
		 return angle;
	}
	
	// public static float AngleTo(this Vector2 this_, Vector2 to)
	// {
		 // Vector2 direction = to - this_;
		 // float angle = Mathf.Atan2(direction.y,  direction.x) * Mathf.Rad2Deg;
		 // if (angle < 0f) angle += 360f;
		 // return angle;
	// }
}
