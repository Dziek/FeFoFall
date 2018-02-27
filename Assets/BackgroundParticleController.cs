using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundParticleController : MonoBehaviour {

	public ParticleSystem[] particleSystems;
	
	private ParticleSystem currentPS;
	
	private bool manualTesting;
	private int arrayPos;
	
	void Update () {
		
		if (Input.GetButtonDown("Fire1"))
		{
			manualTesting = true;
			
			DisableEffect();
			
			arrayPos--;
			
			if (arrayPos < 0)
			{
				arrayPos = particleSystems.Length-1;
			}
			
			ParticleSystem nextPS = particleSystems[arrayPos];
			
			currentPS = nextPS;
			currentPS.Play();
		}
		
		if (Input.GetButtonDown("Fire2"))
		{
			manualTesting = true;
			
			DisableEffect();
			
			arrayPos++;
			
			if (arrayPos >= particleSystems.Length)
			{
				arrayPos = 0;
			}
			
			ParticleSystem nextPS = particleSystems[arrayPos];
			
			currentPS = nextPS;
			currentPS.Play();
		}
	}
	
	void Awake () {
		if (GetComponentsInChildren<ParticleSystem>().Length > 0)
		{
			particleSystems = GetComponentsInChildren<ParticleSystem>();
		}
		
		// Debug.Log(particleSystems.Length);
	}
	
	void SwitchEffect () {
		
		if (manualTesting == false)
		{
			DisableEffect();
			
			ParticleSystem nextPS = particleSystems[Random.Range(0, particleSystems.Length)];
			
			while (currentPS == nextPS)
			{
				nextPS = particleSystems[Random.Range(0, particleSystems.Length)];
			}
			
			currentPS = nextPS;
			currentPS.Play();
		}
		
	}
	
	// void EnableEffect () {
		
	// }
	
	void DisableEffect () {
		
		if (currentPS != null)
		{
			currentPS.Stop();
			currentPS.Clear();
		}
	}
	
	void OnEnable () {
		Messenger.AddListener("TransitionMiddle", SwitchEffect);
		// Messenger.AddListener("FirstMovement", EnableEffect);
		Messenger.AddListener("MainMenu", DisableEffect);
	}
	
	void OnDisable () {
		Messenger.RemoveListener("TransitionMiddle", SwitchEffect);
		// Messenger.RemoveListener("FirstMovement", EnableEffect);
		Messenger.RemoveListener("MainMenu", DisableEffect);
	}
}
