using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Watermark : MonoBehaviour {
	
	public Sprite[] sprites;
	
	[System.Serializable]
	public struct ColorMark {
		public Color colour;
		
		public float minAlpha;
		public float maxAlpha;
	}
	
	public ColorMark[] colourMarks;
	
	private Image image;
	
	void Awake () {
		image = GetComponent<Image>();
		
		DisableImage();
	}
	
	void SwitchImage () {
		
		EnableImage();
		
		image.sprite = sprites[Random.Range(0, sprites.Length)];
		
		int rand = Random.Range(0, colourMarks.Length);
		
		Color newColour = colourMarks[rand].colour;
		newColour.a = Mathf.InverseLerp(0, 255, Random.Range(colourMarks[rand].minAlpha, colourMarks[rand].maxAlpha));
		
		image.color = newColour;
		
		float x = Random.Range(0, 2) == 0 ? 1 : -1;
		float y = Random.Range(0, 2) == 0 ? 1 : -1;
		transform.localScale = new Vector3(x, y, 1);
	}
	
	void EnableImage () {
		image.enabled = true;
	}
	
	void DisableImage () {
		image.enabled = false;
	}
	
	void OnEnable () {
		Messenger.AddListener("TransitionMiddle", SwitchImage);
		// Messenger.AddListener("FirstMovement", EnableImage);
		Messenger.AddListener("MainMenu", DisableImage);
	}
	
	void OnDisable () {
		Messenger.RemoveListener("TransitionMiddle", SwitchImage);
		// Messenger.RemoveListener("FirstMovement", EnableImage);
		Messenger.RemoveListener("MainMenu", DisableImage);
	}
}
