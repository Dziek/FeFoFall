using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ModeSelectionScrollBar : MonoBehaviour {
	
	public Text text;
	private Slider scrollbar;
	
	private ModeManager modeManager;
	
	void Awake () {
		scrollbar = GetComponent<Slider>();
		modeManager = GameObject.Find("ModeManager").GetComponent<ModeManager>();
		
		scrollbar.onValueChanged.AddListener(OnChange);
	}
	
	void OnChange (float value) {
		
		int v = (int)value;
		
		modeManager.SetModeInt(v);
		Mode m = (Mode)v;
		text.text = "Mode - " + m.ToString();
	}
}
