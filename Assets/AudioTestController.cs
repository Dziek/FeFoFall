using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioTestController : MonoBehaviour {
	
	
	
	void CreateAudio () {
		// GameObject[] go = GameObject.FindGameObjectsWithTag("Wall");
		
		// for (int i = 0; i < go.Length; i++)
		// {
			// if (go[i].GetComponent<WallAudio>() == null)
			// {
				// WallAudio wAudio = go[i].AddComponent<WallAudio>();
			// }
		// }
	}
	
	void OnEnable () {
		Messenger.AddListener("TransitionMiddle", CreateAudio);
		// Messenger.AddListener("FirstMovement", EnableImage);
		// Messenger.AddListener("MainMenu", DisableImage);
	}
	
	void OnDisable () {
		Messenger.RemoveListener("TransitionMiddle", CreateAudio);
		// Messenger.RemoveListener("FirstMovement", EnableImage);
		// Messenger.RemoveListener("MainMenu", DisableImage);
	}
}
