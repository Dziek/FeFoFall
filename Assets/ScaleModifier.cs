using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleModifier : MonoBehaviour {
	
	public GameObject modifierGO; // the GameObject to base the modification off of
	
	public float topRangePos = 6; // what to go between range wise
	public float bottomRangePos = -6;
	
	public float topRangeScale = 2;
	public float bottomRangeScale = 1;
	
	public ScaleOptions getAxis; // what axis to get position from
	public ScaleOptions scaleAxis; // what axis to get scale from
	
	public bool useAbsolutePosition; // whether to use Mathf.Abs on positionValue
	public bool offsetScale; // if true make it so only "shrinking" from one side. Need to make more robust so can choose which side (Only works in X)
	
	private Vector2 previousFrameScale; // for use in offset
	
	void Awake () {
		previousFrameScale = modifierGO.transform.localScale;
	}
	
	// Update is called once per frame
	void Update () {
		previousFrameScale = modifierGO.transform.localScale;
		Convert();
	}
	
	/////BAD OFFSET CODE/////
	void LateUpdate () {
		if (offsetScale)
		{
			float offsetValue = 0;
			
			offsetValue = previousFrameScale.y - modifierGO.transform.localScale.y;
			modifierGO.transform.Translate(Vector2.down * offsetValue/2);
		}
	}
	
	void Convert () {
		float positionValue = 0;
		
		switch (getAxis)
		{
			case ScaleOptions.X:
				positionValue = modifierGO.transform.position.x;
			break;
			
			case ScaleOptions.Y:
				positionValue = modifierGO.transform.position.y;
			break;
		}
		
		if (useAbsolutePosition)
		{
			positionValue = Mathf.Abs(positionValue);
		}
		
		float tempValue = Mathf.InverseLerp(bottomRangePos, topRangePos, positionValue);
		float scaleValue = Mathf.Lerp(bottomRangeScale, topRangeScale, tempValue);
		
		Vector2 newScaleVector = Vector2.zero;
		
		switch (scaleAxis)
		{
			case ScaleOptions.X:
				newScaleVector = new Vector2(scaleValue, modifierGO.transform.localScale.y);
			break;
			
			case ScaleOptions.Y:
				newScaleVector = new Vector2(modifierGO.transform.localScale.x, scaleValue);
			break;
		}
		
		modifierGO.transform.localScale = newScaleVector;
	}
	
	public enum ScaleOptions {
		X,
		Y
	}
}
