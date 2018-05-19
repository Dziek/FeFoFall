using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ModeButton : MonoBehaviour, ISelectHandler {
	
	public Mode mode = Mode.Unassigned;
	
	private ModeManager modeManager;
	
	void Awake () {
		modeManager = GameObject.Find("ModeManager").GetComponent<ModeManager>();
	}
	
	void Select () {
		modeManager.SetMode(mode);
	}
	
	public void OnSelect (BaseEventData eventData) {
		Select();
    }
}
