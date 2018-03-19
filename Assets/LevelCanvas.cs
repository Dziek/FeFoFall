using UnityEngine;
using System.Collections;

public class LevelCanvas : MonoBehaviour {
	
	void Start () {
		Invoke("Broadcast", 0.1f);
	}
	
	void Broadcast () {
		// Messenger.Broadcast("LevelCanvasPresent");
	}
	
	void OnEnable () {	
		Messenger.AddListener("FirstMovement", ClearCanvas);
	}
	
	void OnDisable() {	
		Messenger.RemoveListener("FirstMovement", ClearCanvas);
	}
	
	void ClearCanvas () {
		gameObject.SetActive(false);
	}
}
