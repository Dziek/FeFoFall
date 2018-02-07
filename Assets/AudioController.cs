using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour {
	
	public AudioClip successNoise;
	public AudioClip failureNoise;
	
	private AudioSource audioSource;
	
	void Awake () {
		audioSource = GetComponent<AudioSource>();
	}
	
	void PlaySuccess () {
		audioSource.clip = successNoise;
		audioSource.Play();
	}
	
	void PlayFailure () {
		audioSource.clip = failureNoise;
		audioSource.Play();
	}
	
	void OnEnable () {
		Messenger.AddListener("Success", PlaySuccess);
		Messenger.AddListener("Failure", PlayFailure);
	}
	
	void OnDisable () {
		Messenger.RemoveListener("Success", PlaySuccess);
		Messenger.RemoveListener("Failure", PlayFailure);
	}
}
