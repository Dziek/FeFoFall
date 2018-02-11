using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionScreen : MonoBehaviour {
	
	public GameObject expandPanel;
	
	public GameObject levelInfoScreenGO;
	public GameObject completeScreenGO;
	
	public GameObject transitionTextGO;
	// public Text transitionText;
	
	private TransitionController controller;
	
	private float startingXScale;
	private float growthSpeed = 0.1f;
	
	void Awake () {
		controller = GetComponent<TransitionController>();
	}
	
	public IEnumerator StartPhaseOne () {
		
		GameObject startPoint;
		
		if (controller.transitionState != TransitionState.levelLoad)
		{
			// startPoint = GameObject.Find("Player").transform.position;
			startPoint = GameObject.Find("Player");
		}else{
			// startPoint = GameObject.Find("Play").transform.position;
			startPoint = GameObject.Find("Play");
		}
		
		startingXScale = startPoint.transform.localScale.x * 0.058f;
		
		expandPanel.transform.localScale = new Vector3 (startingXScale, startPoint.transform.localScale.y * 0.1f, 1);
		expandPanel.transform.position = new Vector3 (startPoint.transform.position.x, startPoint.transform.position.y, expandPanel.transform.position.z);
		
		float target = 2f;
		
		// float t = 0;
		// float timeToGrow = 0.2f
		
		while (expandPanel.transform.localScale.x < target && expandPanel.transform.localScale.y < target)
		{
			expandPanel.transform.localScale = new Vector3(expandPanel.transform.localScale.x + growthSpeed, expandPanel.transform.localScale.y + growthSpeed, 1); 
			yield return null;
		}
		
		yield return null;
	}
	
	public IEnumerator StartPhaseTwo () {
		
		if (controller.gameCompleted == false && controller.transitionState != TransitionState.levelTest)
		{
			LoadLevel.GetLevel();
		}else{
			LoadLevel.ClearLevel();
			completeScreenGO.SetActive(true);
		}
		
		transitionTextGO.SetActive(true);
		
		yield return new WaitForSeconds(0.25f);
		
		// Messenger.Broadcast("UpdateColour");
		Messenger.Broadcast("TransitionMiddle");
		transitionTextGO.SetActive(false);
		
		yield return null;
	}
	
	public IEnumerator StartPhaseThree () {
		
		GameObject endPoint;
		
		if (controller.gameCompleted == false)
		{
			
			completeScreenGO.SetActive(false);
			levelInfoScreenGO.SetActive(true);
			endPoint = GameObject.Find("Player");
		}else{
			endPoint = GameObject.Find("Play");	
		}		
		
		expandPanel.transform.position = new Vector3 (endPoint.transform.position.x, endPoint.transform.position.y, expandPanel.transform.position.z);
		
		while (expandPanel.transform.localScale.x > startingXScale)
		{
			expandPanel.transform.localScale = new Vector3(expandPanel.transform.localScale.x - growthSpeed, expandPanel.transform.localScale.y - growthSpeed, 1); 
			yield return null;
		}
		
		yield return null;
	}
}
