using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Transition : MonoBehaviour {
	
	public GameObject panel;
	// public GameObject goodText;
	// public GameObject badText;
	
	public GameObject levelInfo;
	public GameObject completeInfo;
	
	public GameObject transitionGO;
	public Text transitionText;
	
	public string[] goodText;
	public string[] badText;
	public string[] loadText;
	public string completeText;
	
	// public Animation transitionWipe;
	
	// public Camera levelCam;
	
	private float zPos;
	
	private float targetAspect;
	private float scaleHeight;
	private float scaleWidth;
	
	// private string lastText;
	
	// Use this for initialization
	void Start () {
		zPos = Mathf.Abs(Camera.main.transform.position.z - transform.position.z);
		
		targetAspect = 16f/9f;
		float windowAspect = (float)Screen.width / (float)Screen.height;
		scaleHeight = windowAspect / targetAspect;
		float scaleWidth = 1.0f / scaleHeight;
		
		// Debug.Log(scaleHeight + " " + scaleWidth);
		// Debug.Log(windowAspect);
	}
	
	// void BeginTransition (TransitionState tS) {
		// StartCoroutine(Go(tS));
	// }
	
	public IEnumerator Go (string e) {
	// public IEnumerator Go () {
		
		// Debug.Log("T");
		
		bool complete = false; // whether the player has completed the main mode
		
		string newTransitionText;
		
		if (e == "Good")
		{
			complete = LoadLevel.LevelCompleted();
			// transitionText.text = complete == true ? completeText : goodText[Random.Range(0, goodText.Length)];
			
		}else if (e == "Bad"){
			// while (lastText)
			// transitionText.text = badText[Random.Range(0, badText.Length)];
		}else if (e == "Load"){
			// transitionText.text = loadText[Random.Range(0, loadText.Length)];
			if (LoadLevel.CheckComplete())
			{
				LoadLevel.Reset();
				// Debug.Log("R");
			}
		}		
		
		GameObject startPoint;
		if (e != "Load")
		{
			startPoint = GameObject.Find("Player");
			// Debug.Log(startPoint.transform.position);
			// GameObject.Find("Player").SetActive(false);
		}else{
			startPoint = GameObject.Find("Play");
		}
		
		
		float startingXScale = startPoint.transform.localScale.x * 0.058f;
		
		panel.transform.localScale = new Vector3 (startingXScale, startPoint.transform.localScale.y * 0.1f, 1);
		panel.transform.position = new Vector3 (startPoint.transform.position.x, startPoint.transform.position.y, panel.transform.position.z);
		
		float target = 2f;
		float growthSpeed = 0.1f;
		
		while (panel.transform.localScale.x < target && panel.transform.localScale.y < target)
		{
			panel.transform.localScale = new Vector3(panel.transform.localScale.x + growthSpeed, panel.transform.localScale.y + growthSpeed, 1); 
			yield return null;
		}
		
		// GameStates.ChangeState("GameOver");
		// if (LoadLevel.CheckLevel())
		if (complete == false && e != "Test")
		{
			LoadLevel.GetLevel();
		
			
			// levelInfo.SetActive(true);
		}else{
			LoadLevel.ClearLevel();
			completeInfo.SetActive(true);
		}
		
		transitionGO.SetActive(true);
		
		yield return new WaitForSeconds(0.25f);
		
		GameObject endPoint;
		
		// if (LoadLevel.CheckLevel())
		if (complete == false)
		{
			
			completeInfo.SetActive(false);
			levelInfo.SetActive(true);
			endPoint = GameObject.Find("Player");
		}else{
			// GameStates.ChangeState("Complete");
			// LoadLevel.ClearLevel();
			// completeInfo.SetActive(false);
			endPoint = GameObject.Find("Play");	
		}
		
		// levelInfo.SetActive(true);
		
		// Messenger.Broadcast("UpdateColour");
		Messenger.Broadcast("TransitionMiddle");
		
		transitionGO.SetActive(false);
		
		
		panel.transform.position = new Vector3 (endPoint.transform.position.x, endPoint.transform.position.y, panel.transform.position.z);
		
		while (panel.transform.localScale.x > startingXScale)
		{
			panel.transform.localScale = new Vector3(panel.transform.localScale.x - growthSpeed, panel.transform.localScale.y - growthSpeed, 1); 
			yield return null;
		}
		
		// if (LoadLevel.CheckLevel())
		if (complete == false)
		{
			GameStates.ChangeState("Playing");
		}else{
			GameStates.ChangeState("Complete");
		}
		
		yield return null;
	}
}
