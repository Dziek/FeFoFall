using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TransitionScreen : MonoBehaviour {
	
	public Gradient panelGradient;
	
	public GameObject expandPanelGO;
	
	public GameObject levelInfoScreenGO;
	public GameObject completeScreenGO;
	
	public EndScreenController endScreenController;
	
	public GameObject transitionTextGO;
	// public Text transitionText;
	
	private TransitionController controller;
	
	private float startingXScale;
	private float growthSpeed = 0.1f;
	
	private Vector2 startingScale;
	
	private Image expandPanelImage;
	
	private Color defaultColour;
	private Color playerColour = new Color32(217, 218, 74, 255);
	private Color obstacleColour;
	private Color endColour = new Color32(238, 60, 209, 255);
	
	private Color nextColour;
	
	// public GameObject playerGO;
	private PlayerControl playerControl;
	private float playerSpeed;
	private Vector3 playerScale;
	
	void Awake () {
		controller = GetComponent<TransitionController>();
		
		expandPanelImage = expandPanelGO.GetComponent<Image>();
		defaultColour = expandPanelImage.color;
		
		expandPanelImage.color = playerColour;
	}
	
	public void Cancel () {
		transitionTextGO.SetActive(false);
		expandPanelImage.color = playerColour;
		
		// StopCoroutine("StartPhaseOne");
		// StopCoroutine("StartPhaseTwo");
		// StopCoroutine("StartPhaseThree");
		
		// StopAllCoroutines();
	}
	
	public IEnumerator StartPhaseOne () {
		
		// Debug.Log("Start Of Phase One");
		// Debug.Break();
		
		GameObject startPoint;
		
		if (controller.transitionReason != TransitionReason.levelLoad)
		{
			// // startPoint = GameObject.Find("Player").transform.position;
			// startPoint = GameObject.Find("Player");
			// // startPoint = playerGO;
			// Debug.Log(startPoint);
			// // Debug.Log(startPoint.GetComponent<PlayerControl>());
			// playerSpeed = startPoint.GetComponent<PlayerControl>().GetSpeed();
			// playerScale = startPoint.transform.localScale;
			
			startPoint = playerControl.gameObject;
			playerSpeed = playerControl.GetSpeed();
			playerScale = startPoint.transform.localScale;
			
			if (controller.transitionReason == TransitionReason.levelSuccess)
			{
				expandPanelImage.color = endColour;
				
				// expandPanelGO.transform.localScale = new Vector2(2, 2);
				// expandPanelImage.color = playerColour;
				// yield break;
			}else{
				// int r = Random.Range(0, 3);
				// // int r = 0;
				
				// switch(r)
				// {
					// case 0:
						// expandPanelImage.color = startPoint.GetComponent<Renderer>().material.color * new Color32(255, 255, 255, 40);
						// // expandPanelImage.color = playerColour;
						// Debug.Log("Case 0");
						// break;
						
					// case 1:
						// expandPanelImage.color = startPoint.GetComponent<Renderer>().material.color;
						// // expandPanelImage.color = new Color32(240, 106, 41, 255);
						// // // nextColour = new Color32(240, 106, 41, 255);
						// Debug.Log("Case 1");
						// break;
						
					// // case 2:
						// // expandPanelImage.color = new Color32(52, 87, 134, 255);
						// // Debug.Log("Case 2");
						// // break;
						
					// // case 3:
						// // expandPanelImage.color = new Color32(44, 80, 125, 255);
						// // Debug.Log("Case 3");
						// // break;
						
					// default:
						// expandPanelImage.color = defaultColour;
						// // nextColour = defaultColour;
						// Debug.Log("Case Default");
						// break;
				// }
				
				expandPanelImage.color = Color.white;
			}
			
		}else{
			// startPoint = GameObject.Find("Play").transform.position;
			startPoint = GameObject.Find("Play");
			expandPanelGO.transform.localScale = new Vector2(2, 2);
			
			// Debug.Log("About to yield break");
			// Debug.Break();
			
			yield break;
		}
		
		if (Testing.t1 == true)
		{
			expandPanelGO.transform.localScale = new Vector2(2, 2);
			expandPanelImage.color = playerColour;
			
			yield break;
		}
		
		if (Testing.t2 == true)
		{
			expandPanelGO.transform.localScale = new Vector2(2, 2);
			expandPanelImage.color = defaultColour;
			
			yield break;
		}
		
		if (Testing.t3 == true)
		{
			expandPanelGO.transform.localScale = new Vector2(2, 2);
			expandPanelImage.color = endColour;
			
			yield break;
		}
		
		// Debug.Log("Slightly Later In Phase One");
		// Debug.Break();
		
		// if (startPoint == null)
		// {
			// yield break;
		// }
		
		// Debug.Log(startPoint.transform.position);
		
		startingXScale = startPoint.transform.localScale.x * 0.058f;
		startingScale = startPoint.transform.localScale * 0.058f;
		
		//*******//
		
		Vector2 screenPos = startPoint.transform.position;
		
		if (GameObject.Find("LevelCamera") != null)
		{
			Camera cam = GameObject.Find("LevelCamera").GetComponent<Camera>();
			
			screenPos = cam.WorldToViewportPoint(startPoint.transform.position);
			
			screenPos = Camera.main.ViewportToWorldPoint(screenPos);
			
			// Debug.Break();
		}
		
		//*******//
		
		expandPanelGO.transform.localScale = new Vector3 (startingXScale, startPoint.transform.localScale.y * 0.1f, 1);
		// expandPanelGO.transform.position = new Vector3 (startPoint.transform.position.x, startPoint.transform.position.y, expandPanelGO.transform.position.z);
		expandPanelGO.transform.position = new Vector3 (screenPos.x, screenPos.y, expandPanelGO.transform.position.z);
		
		float target = 2f;
		
		// float t = 0;
		// float timeToGrow = 0.2f
		
		// while (expandPanelGO.transform.localScale.x < target && expandPanelGO.transform.localScale.y < target)
		// {
			// expandPanelGO.transform.localScale = new Vector3(expandPanelGO.transform.localScale.x + growthSpeed, expandPanelGO.transform.localScale.y + growthSpeed, 1); 
			// yield return null;
		// }
		
		float t = 0;
		float timeToGrow = 0.5f;
		
		if (controller.transitionReason != TransitionReason.levelSkip)
		{
			// modify timeToGrow based on playerSpeed
		
			MinMax playerSpeedValues = new MinMax(1, 16);
			MinMax timeToGrowValues = new MinMax(0.1f, 0.7f);
			
			float l = Mathf.InverseLerp(playerSpeedValues.min, playerSpeedValues.max, playerSpeed);
			timeToGrow = Mathf.Lerp(timeToGrowValues.min, timeToGrowValues.max, 1-l);
		}else{
			timeToGrow = 0.2f;
		}
		
		// float changeColourDelay = 0.2f; 0.06f 0.18f
		float changeColourDelay = Random.Range(0, 0.15f);
		// Debug.Log(changeColourDelay);
		
		Vector2 startScale = expandPanelGO.transform.localScale;
		Vector2 targetScale = new Vector2(2, 2);
		
		// Image expandPanelImage = expandPanelImage;
		
		Color startColour = expandPanelImage.color;
		// Color targetColour = panelGradient.Evaluate(LoadLevel.GetPercentageComplete());
		// Color targetColour = panelGradient.Evaluate(Random.Range(0f, 1f));
		Color targetColour = panelGradient.Evaluate(1);
		// Color endColour = endPoint.GetComponent<Renderer>().material.color;
		
		if (startColour == endColour)
		{
			targetColour = endColour;
		}
		
		while (t < timeToGrow)
		{
			t += Time.deltaTime;
			
			float lerpValue = t / timeToGrow;
			
			expandPanelGO.transform.localScale = Vector2.Lerp(startScale, targetScale, lerpValue);
			
			float colourLerpValue = Mathf.InverseLerp(changeColourDelay, timeToGrow, t);
			
			expandPanelImage.color = Color.Lerp(startColour, targetColour, colourLerpValue);
			
			yield return null;
		}
		
		// Debug.Log("One Still running y'all");
		
		yield return null;
	}
	
	public IEnumerator StartPhaseTwo (float waitTime) {
		
		// if (controller.gameCompleted == false)
		// {
			// completeScreenGO.SetActive(false);
			// levelInfoScreenGO.SetActive(true);
		// }
		
		if (controller.gameOver == true)
		{
			endScreenController.GameOver();
			controller.levelManager.ClearLevel();
			
		}else if (controller.gameCompleted == false && controller.transitionReason != TransitionReason.levelTest)
		{
			if (controller.transitionReason == TransitionReason.levelFailure && controller.modeManager.GetMode() == Mode.Tricky)
			{
				controller.levelManager.ReloadLevel();
			}else{
				controller.levelManager.SwitchLevel();
			}
		}else{
			// LoadLevel.ClearLevel();
			// completeScreenGO.SetActive(true);
			endScreenController.Completed();
			controller.levelManager.ClearLevel();
			controller.statsManager.ModeComplete(controller.modeManager.GetMode());
		}
		
		transitionTextGO.SetActive(true);
		
		// yield return new WaitForSeconds(0.25f);
		
		// Messenger.Broadcast("TransitionMiddle");
		
		yield return new WaitForSeconds(waitTime);
		
		// Messenger.Broadcast("UpdateColour");
		Messenger.Broadcast("TransitionMiddle");
		// transitionTextGO.SetActive(false);
		
		// Debug.Log("Two Still running y'all");
		
		yield return null;
	}
	
	public IEnumerator StartPhaseThree () {
		
		GameObject endPoint;
		
		if (controller.gameCompleted == false && controller.gameOver == false)
		{
			completeScreenGO.SetActive(false);
			levelInfoScreenGO.SetActive(true);
			
			// endPoint = GameObject.Find("Player");
			endPoint = playerControl.gameObject;
		}else{
			endPoint = GameObject.Find("Play");
			if (endPoint == null)
			{
				endPoint = GameObject.Find("Levels");
			}
		}		
		
		transitionTextGO.SetActive(false);
		
		expandPanelGO.transform.position = new Vector3 (endPoint.transform.position.x, endPoint.transform.position.y, expandPanelGO.transform.position.z);
		
		// while (expandPanelGO.transform.localScale.x > startingXScale)
		// {
			// expandPanelGO.transform.localScale = new Vector3(expandPanelGO.transform.localScale.x - growthSpeed, expandPanelGO.transform.localScale.y - growthSpeed, 1); 		
			// yield return null;
		// }
		
		float t = 0;
		float timeToGrow = 0.5f;
		
		// float changeColourDelay = 0.2f; 0.06f 0.18f
		float changeColourDelay = Random.Range(0, 0.25f);
		// Debug.Log(changeColourDelay);
		
		Vector2 startScale = expandPanelGO.transform.localScale;
		Vector2 targetScale = new Vector2(endPoint.transform.localScale.x * 9, endPoint.transform.localScale.y * 16) * 0.0058f;
		
		// Image expandPanelImage = expandPanelImage;
		
		Color startColour = expandPanelImage.color;
		// Color endColour = endPoint.GetComponent<Renderer>().material.color;
		
		while (t < timeToGrow)
		{
			t += Time.deltaTime;
			
			float lerpValue = t / timeToGrow;
			
			expandPanelGO.transform.localScale = Vector2.Lerp(startScale, targetScale, lerpValue);
			
			float colourLerpValue = Mathf.InverseLerp(changeColourDelay, timeToGrow, t);
			
			expandPanelImage.color = Color.Lerp(startColour, playerColour, colourLerpValue);
			
			yield return null;
		}
		
		// playerGO = GameObject.Find("Player");
		// Debug.Log(playerGO);
		
		yield return null;
	}
	
	// public void SetPlayer (PlayerControl pC) {
		// playerControl = pC;
	// }
	
	void OnEnable () {
		Messenger<PlayerControl>.AddListener("SetPlayer", UpdatePlayerControl);
	}
	
	void OnDisable () {
		Messenger<PlayerControl>.RemoveListener("SetPlayer", UpdatePlayerControl);
	}
	
	void UpdatePlayerControl (PlayerControl pC) {
		playerControl = pC;
	}
}
