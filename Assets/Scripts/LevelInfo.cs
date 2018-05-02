using UnityEngine;
// using UnityEngine.UI;
using System.Collections;

public class LevelInfo : MonoBehaviour {
	
	public string name;
	public string description;
	
	[TextArea(2, 3)]
	public string info;
	
	[TextArea(5, 10)]
	public string notes;
	
	public enum Rating {
		S,
		A,
		B,
		C,
		D,
		F,
		Unrated
	} public Rating levelRating = Rating.Unrated;
	
	public enum Difficulty {
		Beginner,
		BeginnerPlus,
		Easy,
		EasyPlus,
		Intermediate,
		IntermediatePlus,
		Hard,
		Devilish,
		Tricky,
		TooMuch,
		Unrated
	} public Difficulty levelDifficulty = Difficulty.Unrated;
	
	public bool canFlip = false;
	
	public Color background;
	
	void OnEnable () {
		// if (background != null)
		// {
			// Camera.main.backgroundColor = background;
		// }
		
		if (canFlip == true)
		{
			float x = Random.Range(0, 2) == 0 ? 1 : -1;
			float y = Random.Range(0, 2) == 0 ? 1 : -1;
			transform.localScale = new Vector3(x, y, 1);
		}
		
		// Messenger<GameObject>.Broadcast("NewLevelLoaded", gameObject);
		
		// Messenger.AddListener("FirstMovement", ClearCanvas);
	}
	
	// void OnDisable() {
		
		// Messenger.RemoveListener("FirstMovement", ClearCanvas);
		
	// }
	
	// void ClearCanvas () {
		// gameObject.GetComponentInChildren<Canvas>().gameObject.SetActive(false);
		// Debug.Log("Clear");
	// }
	
	public void GetInfo (out string n, out string d) {
		// return description;
		// return "W";
		n = name;
		d = description;
		// return;
	}
}
