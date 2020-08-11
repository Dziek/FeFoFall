using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModeManager : MonoBehaviour {
	
	public ColorScheme mainCS;
	public ColorScheme trickyCS;
	
	public SpriteRenderer wallSR;
	
	private Mode mode = Mode.Main;
	
	public void SetMode (Mode newMode) {
		mode = newMode;
		Messenger.Broadcast("ModeUpdated");
		
		// this is where I'll do all the art changes for each mode probably
		// UpdateColours();
	}
	
	void UpdateColours () {
		switch (mode)
		{
			default:
				Camera.main.backgroundColor = mainCS.background;
				wallSR.color = mainCS.wall;
			break;
			
			case Mode.Tricky:
				Camera.main.backgroundColor = trickyCS.background;
				wallSR.color = trickyCS.wall;
			break;
		}
	}
	
	public void SetModeString (string s) {
		// mode = (Mode)System.Enum.Parse(typeof(Mode), s);
		
		Mode newMode = (Mode)System.Enum.Parse(typeof(Mode), s);
		SetMode(newMode);
	}
	
	public void SetModeInt (int i) {
		Mode newMode = (Mode)i;
		SetMode(newMode);
	}
	
	public Mode GetMode () {
		return mode;
	}
}

public enum Mode {
	Main,
	Tricky,
	Time,
	Lives,
	OneShot,
	Basement,
	All,
	None,
	Unassigned
}

[System.Serializable]
public struct ColorScheme {
	public Color background;
	public Color wall;
	// public Color player;
	// public Color endPoint;
	// public Color trigger;
	// public Color transition;
}
