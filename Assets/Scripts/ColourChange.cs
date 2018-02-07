using UnityEngine;
using System.Collections;

public class ColourChange : MonoBehaviour {

	public Color colourChange;
	public bool changeColor;
	
	void Awake () {
		GetComponent<Renderer>().material.color = colourChange;
	}
}
