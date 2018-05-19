using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PostProcessing;

public class PreLevelEffects : MonoBehaviour {
	
	public PostProcessingBehaviour postProcesses;
	
	private PostProcessingProfile profile;
	
	// private float grainTopIntensity = 0.35f;
	private float grainDefaultIntensity = 0.25f;
	
	void Awake () {
		profile = postProcesses.profile;
		
		// TurnOff();
	}
	
	void TurnOn () {
		// postProcesses.enabled = true;
		
		// profile.vignette.enabled = true;
		
		if (profile.vignette.enabled == false)
		{
			StartCoroutine("FadeInVignette");
		}
		
		StartCoroutine("FadeInGrain");
	}
	
	void TurnOff () {
		profile.grain.enabled = false;
		// profile.vignette.enabled = false;
		// postProcesses.enabled = false;
	}
	
	void TurnDown () {
		GrainModel.Settings s = profile.grain.settings;
		s.intensity = grainDefaultIntensity;
		profile.grain.settings = s;
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
	
	IEnumerator FadeInGrain () {
		profile.grain.enabled = true;
		
		float t = 0;
		float timeToFade = 0.3f;
		
		float topIntensity = 0.5f;
		
		while (t < timeToFade)
		{
			GrainModel.Settings v = profile.grain.settings;
			v.intensity = Mathf.Lerp(grainDefaultIntensity, topIntensity, t / timeToFade);
			profile.grain.settings = v;
			
			t += Time.deltaTime;
			yield return null;
		}
		
		GrainModel.Settings s = profile.grain.settings;
		s.intensity = topIntensity;
		profile.grain.settings = s;
	}
	
	void OnEnable () {
		Messenger.AddListener("TransitionMiddle", TurnOn);
		// Messenger.AddListener("FirstMovement", TurnOff);
		Messenger.AddListener("FirstMovement", TurnDown);
		Messenger.AddListener("BackToMenu", TurnDown);
	}
	
	void OnDisable () {
		Messenger.RemoveListener("TransitionMiddle", TurnOn);
		// Messenger.RemoveListener("FirstMovement", TurnOff);
		Messenger.RemoveListener("FirstMovement", TurnDown);
		Messenger.RemoveListener("BackToMenu", TurnDown);
		
		TurnDown();
	}
}
