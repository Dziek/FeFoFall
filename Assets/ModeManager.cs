using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModeManager : MonoBehaviour {
	
	private Mode mode = Mode.Main;
	
	public void SetMode (Mode newMode) {
		mode = newMode;
		Messenger.Broadcast("ModeUpdated");
		
		// this is where I'll do all the art changes for each mode probably
	}
	
	public void SetModeString (string s) {
		// mode = (Mode)System.Enum.Parse(typeof(Mode), s);
		
		Mode newMode = (Mode)System.Enum.Parse(typeof(Mode), s);
		SetMode(newMode);
	}
	
	public Mode GetMode () {
		return mode;
	}
}

public enum Mode {
	Main,
	Tricky,
	GauntletA,
	GauntletB,
	Basement,
	All,
	Unassigned
}
