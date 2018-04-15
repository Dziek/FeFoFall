using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveBasic : ObjectManipulation<Vector2> {
	
	private float zPos;
	
	public override void DestinationPointsSetUp () {
		ReverseProtection(destinationPoints, Vector2.zero);
		
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
		
		zPos = transform.position.z;
		
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
			
			transform.position = Vector2.Lerp(currentPoint, nextPoint, lerpAmount); 
			transform.position = transform.position + Vector3.forward * zPos;
		}
		
		lastFramePoint = GetRelevantValue();
	}
	
	// called when an external script moves object (eg teleporter)
	public override void RecalculatePosition () {
		Vector2 difference = (lastFramePoint - GetRelevantValue());
		
		currentPoint = currentPoint - difference;
		nextPoint = nextPoint - difference;
	}
	
	public override void CalculateSpeed () {
		speed = Vector2.Distance(currentPoint, nextPoint) / executeTime;
	}
	
	public override void CalculateTime () {
		executeTime = Mathf.Abs(Vector2.Distance(currentPoint, nextPoint) / speed);
	}
	
	public Vector2 GetRelevantValue () {
		return transform.position;
	}
}
