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
	
	// public Modes mode = Modes.Unassigned;
	
	// [HideInInspector]
	// public LevelStats levelStats = new LevelStats();
	
	void OnEnable () {
		
		if (canFlip == true)
		{
			float x = Random.Range(0, 2) == 0 ? 1 : -1;
			float y = Random.Range(0, 2) == 0 ? 1 : -1;
			transform.localScale = new Vector3(x, y, 1);
		}
		
		// Messenger.AddListener("Success", LevelComplete);
	}
	
	// void OnDisable () {
		// Messenger.RemoveListener("Success", LevelComplete);
	// }
	
	// void LevelComplete () {
		// levelStats.isCompleted = true;
	// }
}
