using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgressColours : MonoBehaviour {
	
	//I DO NOT LIKE USING MATERIALS FOR THIS
	public Gradient backgroundGradient;
	
	public ColourPairing player;
	public ColourPairing trigger;
	public ColourPairing walls;
	public ColourPairing endPoint;
	
	void Start () {
		UpdateColour();
	}
	
	void UpdateColour () {
		
		float pointValue = LoadLevel.GetPercentageComplete();
		// Debug.Log(pointValue);
		//TEST
		pointValue = 0.5f;
		
		Camera.main.backgroundColor = backgroundGradient.Evaluate(pointValue);
		
		// I believe this is causing the slowdown. Only happens when you complete a level, and also, doesn't do anything because everything uses sprites now
		
		// player.mat.color = player.grad.Evaluate(pointValue);
		// trigger.mat.color = trigger.grad.Evaluate(pointValue);
		// walls.mat.color = walls.grad.Evaluate(pointValue);
		// endPoint.mat.color = endPoint.grad.Evaluate(pointValue);
	}
	
	void OnEnable () {
		Messenger.AddListener("TransitionMiddle", UpdateColour);
		Messenger.AddListener("UpdateColour", UpdateColour);
	}
	
	void OnDisable () {
		Messenger.RemoveListener("TransitionMiddle", UpdateColour);
		Messenger.RemoveListener("UpdateColour", UpdateColour);
	}
}

[System.Serializable]
public struct ColourPairing {
	public Material mat;
	public Gradient grad;
}