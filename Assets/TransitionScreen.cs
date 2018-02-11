using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionScreen : MonoBehaviour {
	
	private TransitionController controller;
	
	void Awake () {
		controller = GetComponent<TransitionController>();
	}
	
	public IEnumerator StartPhaseOne () {
		
		Debug.Log("PhaseOne");
		
		yield return new WaitForSeconds(1);
		
		yield return null;
	}
	
	public IEnumerator StartPhaseTwo () {
		
		Debug.Log("PhaseTwo");
		
		yield return new WaitForSeconds(1);
		
		yield return null;
	}
	
	public IEnumerator StartPhaseThree () {
		
		Debug.Log("PhaseThree");
		
		yield return new WaitForSeconds(1);
		
		yield return null;
	}
}
