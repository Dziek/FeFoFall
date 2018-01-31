using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LevelInfoDisplay : MonoBehaviour {
	
	public Text lvlName;
	public Text lvlDesc;
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
		}
		
		GameObject.Find("Player").GetComponent<PlayerControl>().UpdateLevelInfoDisplayObject(gameObject);
		
		// if (lvlDesc.text == null)
		// {
			// lvlDesc.text = completeDesc;
			// canRid = false;
		// }
	}
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		// if (Input.GetKey("w") || Input.GetKey("a") || Input.GetKey("s") || Input.GetKey("d")
			// || Input.GetKey("up") || Input.GetKey("left") || Input.GetKey("down") || Input.GetKey("right"))
			// // && canRid)
		// {
			// LoadLevel.AddToCurrentAttempts();
			// gameObject.SetActive(false);
		// }
	}
	
	public void GoAway () {
		LoadLevel.AddToCurrentAttempts();
		gameObject.SetActive(false);
	}
}
