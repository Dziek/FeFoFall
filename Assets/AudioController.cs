using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour {
	
	public AudioClip successNoise;
	public AudioClip failureNoise;
	public AudioClip triggerNoise;
	
	private AudioSource audioSource;
	private AudioSource audioSourceBackUp;
	
	void Awake () {
		audioSource = GetComponent<AudioSource>();
		audioSourceBackUp = gameObject.AddComponent<AudioSource>();
	}
	
	void PlaySuccess () {
		// audioSource.clip = successNoise;
		// audioSource.Play();
		PlayClip(successNoise);
	}
	
	void PlayFailure () {
		// audioSource.clip = failureNoise;
		// audioSource.Play();
		PlayClip(failureNoise);
	}
	
	void PlayTrigger () {
		// audioSource.clip = triggerNoise;
		// audioSource.Play();
		PlayClip(triggerNoise);
	}
	
	void PlayClip (AudioClip clip) {
		
		if (audioSource.isPlaying == false)
		{
			audioSource.clip = clip;
			audioSource.Play();
		}else{
			audioSourceBackUp.clip = clip;
			audioSourceBackUp.Play();
		}
	}
	
	void OnEnable () {
		Messenger.AddListener("Success", PlaySuccess);
		Messenger.AddListener("Failure", PlayFailure);
		Messenger.AddListener("Trigger", PlayTrigger);
	}
	
	void OnDisable () {
		Messenger.RemoveListener("Success", PlaySuccess);
		Messenger.RemoveListener("Failure", PlayFailure);
		Messenger.RemoveListener("Trigger", PlayTrigger);
	}
}
