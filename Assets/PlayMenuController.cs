using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayMenuController : MonoBehaviour {
	
	public Text modeInfoText;
	
	[TextArea(5, 10)]
	public string mainInfo;
	
	[TextArea(5, 10)]
	public string trickyInfo;
	
	[TextArea(5, 10)]
	public string timeInfo;
	
	[TextArea(5, 10)]
	public string livesInfo;
	
	[TextArea(5, 10)]
	public string oneShotInfo;
	
	private ModeManager modeManager;
	
	void Awake () {
		modeManager = GameObject.Find("ModeManager").GetComponent<ModeManager>();
	}
	
	void OnEnable () {
		Messenger.AddListener("ModeUpdated", ModeUpdated);
	}
	
	void OnDisable () {
		Messenger.RemoveListener("ModeUpdated", ModeUpdated);
	}
	
	void ModeUpdated () {
		switch (modeManager.GetMode())
		{
			case Mode.Main:
				modeInfoText.text = mainInfo;
			break;
			
			case Mode.Tricky:
				modeInfoText.text = trickyInfo;
			break;
			
			case Mode.Time:
				modeInfoText.text = timeInfo;
			break;
			
			case Mode.Lives:
				modeInfoText.text = livesInfo;
			break;
			
			case Mode.OneShot:
				modeInfoText.text = oneShotInfo;
			break;
			
			default:
				modeInfoText.text = "";
			break;
		}
	}
}
