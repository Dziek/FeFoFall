using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class WallAudio : MonoBehaviour {

	public int position = 0;
    public int sampleRate = 44100;
    public float frequency = 440;
	
	public WaveType waveType = WaveType.Noise;
	
	private float minFrequency = 44;
	private float maxFrequency = 880;
	
	private float minPitch = 0.1f;
	private float maxPitch = 3f;
	
	private Vector2 lowerScreenBounds = new Vector2(-10, -5.5f);
	private Vector2 upperScreenBounds = new Vector2(10, 5.5f);
	
	private float minSizeVolume = 0.5f;
	private float maxSizeVolume = 20;
	
	private AudioSource audioSource;
	
	private AudioMixer mixer;
	private AudioMixerSnapshot[] snapshots;
	
    void Start()
    {
		// Create(string name, int lengthSamples, int channels, int frequency, bool stream, AudioClip.PCMReaderCallback pcmreadercallback, AudioClip.PCMSetPositionCallback pcmsetpositioncallback);
        AudioClip myClip = AudioClip.Create("MySinusoid", sampleRate * 2, 1, sampleRate, true, OnAudioRead, OnAudioSetPosition);
        // AudioClip myClip = AudioClip.Create("MySinusoid", 5000, 1, sampleRate, true, OnAudioRead, OnAudioSetPosition);
        
		audioSource = gameObject.AddComponent<AudioSource>();
		audioSource.loop = true;
        audioSource.clip = myClip;
        audioSource.Play();
		
		
		// mixer = Resources.Load("LevelAudioMixer") as AudioMixer;
		// string _OutputMixer = "Stationary";        
		// audioSource.outputAudioMixerGroup = mixer.FindMatchingGroups(_OutputMixer)[0];
		
		// snapshots = new AudioMixerSnapshot[3];
		// snapshots[0] = mixer.FindSnapshot("PreLevel");
		// snapshots[1] = mixer.FindSnapshot("Level");
		// snapshots[2] = mixer.FindSnapshot("Default");
		
		mixer = Resources.Load("MasterMixer") as AudioMixer;
		string _OutputMixer = "LevelAudioGroup";        
		audioSource.outputAudioMixerGroup = mixer.FindMatchingGroups(_OutputMixer)[0];
		
		snapshots = new AudioMixerSnapshot[2];
		snapshots[0] = mixer.FindSnapshot("Default");
		snapshots[1] = mixer.FindSnapshot("PreLevel");
		
		ChangeSnapshotToPre();
    }
	
	void Update () {	
		PositionAudio();
	}
	
	void OnAudioRead(float[] data)
    {
        for (int i = 0; i < data.Length; i++)
        {
			switch (waveType)
            {
				case WaveType.Sine:
					data[i] = Mathf.Sin(2 * Mathf.PI * frequency * position / sampleRate);
                break;
				
				case WaveType.Sawtooth:
					data[i] = (Mathf.PingPong(frequency * position / sampleRate, 1f));
					// data[i] = Mathf.Repeat(i*frequency/sampleFreq,1)*2f - 1f;
                break;
				
				case WaveType.Square:
					data[i] = Mathf.Sign(Mathf.Sin(2 * Mathf.PI * frequency * position / sampleRate)) * 0.5f;
                break;
				
				case WaveType.Noise:
					data[i] = Mathf.PerlinNoise(frequency * position / sampleRate, 0);
                break;
            }
			 
            position++;
        }
    }

    void OnAudioSetPosition(int newPosition)
    {
        position = newPosition;
    }
	
	void PositionAudio () {
		float xPos = Mathf.InverseLerp(lowerScreenBounds.x, upperScreenBounds.x, transform.position.x);
		float yPos = Mathf.InverseLerp(lowerScreenBounds.y, upperScreenBounds.y, transform.position.y);
		
		// frequency = Mathf.Round(Mathf.Lerp(minFrequency, maxFrequency, xPos));
		audioSource.pitch = Mathf.Lerp(minPitch, maxPitch, yPos);
		
		float size = transform.localScale.x + transform.localScale.y;
		float lerpValue = Mathf.InverseLerp(minSizeVolume, maxSizeVolume, size);
		
		audioSource.volume = Mathf.Lerp(0, 1, lerpValue);
		
	}
	
	void ChangeSnapshotToPre () {
		float[] weights = new float[2];
		
		weights[0] = 0;
		weights[1] = 1;
		// weights[2] = 0;
		
		// weights[0] = 1;
		// weights[1] = 0;
		// weights[2] = 0;
		
		mixer.TransitionToSnapshots(snapshots, weights, 0.5f);
	}
	
	void ChangeSnapshotToLevel () {
		float[] weights = new float[2];
		
		weights[0] = 1;
		weights[1] = 0;
		// weights[2] = 0;
		
		// weights[0] = 0;
		// weights[1] = 1;
		// weights[2] = 0;
		
		mixer.TransitionToSnapshots(snapshots, weights, 0.1f);
	}
	
	void OnEnable () {
		// Messenger.AddListener("TransitionMiddle", CreateAudio);
		Messenger.AddListener("FirstMovement", ChangeSnapshotToLevel);
		// Messenger.AddListener("MainMenu", DisableImage);
	}
	
	void OnDisable () {
		// Messenger.RemoveListener("TransitionMiddle", CreateAudio);
		Messenger.RemoveListener("FirstMovement", ChangeSnapshotToLevel);
		// Messenger.RemoveListener("MainMenu", DisableImage);
	}
	
	[System.Serializable]
	public enum WaveType {
		Sine,
		Sawtooth,
		Square,
		Noise
	}
}
