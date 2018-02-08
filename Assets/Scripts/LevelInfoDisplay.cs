using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LevelInfoDisplay : MonoBehaviour {
	
	public Text lvlName;
	public Text lvlDesc;
	public Text lvlID;
	// public string completeDesc;
	
	// private bool canRid = true; // whether can move buttons to get rid of screen
	
	void OnEnable () {
		GameObject levelObject = GameObject.FindWithTag("Level");
		
		if (levelObject != null)
		{
			// lvlDesc.text = levelObject.GetComponent<LevelInfo>().GetDesc();
			string lN, lD;
			levelObject.GetComponent<LevelInfo>().GetInfo(out lN, out lD);
			
			lvlName.text = lN;
			lvlDesc.text = lD;
			lvlID.text = levelObject.name;
		}
		
		// GameObject.Find("Player").GetComponent<PlayerControl>().UpdateLevelInfoDisplayObject(gameObject);
		levelObject.GetComponentInChildren<PlayerControl>().UpdateLevelInfoDisplayObject(gameObject);
		
		// if (lvlDesc.text == null)
		// {
			// lvlDesc.text = completeDesc;
			// canRid = false;
		// }
	}
	
	public void GoAway () {
		LoadLevel.AddToCurrentAttempts();
		gameObject.SetActive(false);
	}
}
