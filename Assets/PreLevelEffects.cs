using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PostProcessing;

public class PreLevelEffects : MonoBehaviour {
	
	public PostProcessingBehaviour postProcesses;
	
	private PostProcessingProfile profile;
	
	void Awake () {
		profile = postProcesses.profile;
		
		// profile.vignette.color = Color.red;
		// VignetteModel v = profile.vignette;
		
		profile.vignette.enabled = false;
		// VignetteModel.Settings g = profile.vignette.settings;
		// g.color = Color.red;
		// g.intensity = 1;
		// profile.vignette.settings = g;
	}
	
	void TurnOn () {
		// postProcesses.enabled = true;
		
		// profile.vignette.enabled = true;
		StartCoroutine("FadeInVignette");
	}
	
	void TurnOff () {
		profile.vignette.enabled = false;
		// postProcesses.enabled = false;
	}
	
	IEnumerator FadeInVignette () {
		profile.vignette.enabled = true;
		
		float t = 0;
		float timeToFade = 0.3f;
		
		float topIntensity = 0.35f;
		
		while (t < timeToFade)
		{
			VignetteModel.Settings v = profile.vignette.settings;
			v.intensity = Mathf.Lerp(0, topIntensity, t / timeToFade);
			profile.vignette.settings = v;
			
			t += Time.deltaTime;
			yield return null;
		}
		
		VignetteModel.Settings s = profile.vignette.settings;
		s.intensity = topIntensity;
		profile.vignette.settings = s;
	}
	
	void OnEnable () {
		Messenger.AddListener("TransitionMiddle", TurnOn);
		Messenger.AddListener("FirstMovement", TurnOff);
	}
	
	void OnDisable () {
		Messenger.RemoveListener("TransitionMiddle", TurnOn);
		Messenger.RemoveListener("FirstMovement", TurnOff);
		
		TurnOff();
	}
}
