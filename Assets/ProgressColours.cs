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
		Debug.Log(pointValue);
		
		Camera.main.backgroundColor = backgroundGradient.Evaluate(pointValue);
		player.mat.color = player.grad.Evaluate(pointValue);
		trigger.mat.color = trigger.grad.Evaluate(pointValue);
		walls.mat.color = walls.grad.Evaluate(pointValue);
		endPoint.mat.color = endPoint.grad.Evaluate(pointValue);
	}
	
	void OnEnable () {
		Messenger.AddListener("UpdateColour", UpdateColour);
	}
	
	void OnDisable () {
		Messenger.RemoveListener("UpdateColour", UpdateColour);
	}
}

[System.Serializable]
public struct ColourPairing {
	public Material mat;
	public Gradient grad;
}