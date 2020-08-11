using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateBasic : ObjectManipulation<float> {
	
	public override void DestinationPointsSetUp () {
		ReverseProtection(destinationPoints, 0);
		
		if (useLocalSpace)
		{
			for (int i = 0; i < destinationPoints.Count; i++)
			{
				destinationPoints[i] += GetRelevantValue();
			}
		}
	}
	
	public override void SetUp () {
		// for setting up starting values
		   
		startPoint = GetRelevantValue();
		   
		currentPoint = GetRelevantValue();
		nextPoint = destinationPoints[currentListPos];
		   
		lastFramePoint = GetRelevantValue();
	}
	
	public override void ExecuteUpdate (float lerpAmount) {
		// what actually happens every tick
		
		if (GetRelevantValue() != lastFramePoint)
		{
			RecalculatePosition();
		}else{
			
			transform.eulerAngles = new Vector3(0, 0, Mathf.Lerp(currentPoint, nextPoint, lerpAmount));
		}
		
		lastFramePoint = GetRelevantValue();
	}
	
	// called when an external script moves object (eg teleporter)
	public override void RecalculatePosition () {
		float difference = (lastFramePoint - GetRelevantValue());
		
		currentPoint = currentPoint - difference;
		nextPoint = nextPoint - difference;
	}
	
	public override void CalculateSpeed () {
		speed = (currentPoint - nextPoint) / executeTime;
	}
	
	public override void CalculateTime () {
		executeTime = Mathf.Abs((currentPoint - nextPoint) / speed);
	}
	
	public float GetRelevantValue () {
		return transform.eulerAngles.z;
	}
}
