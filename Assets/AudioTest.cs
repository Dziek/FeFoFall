using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioTest : MonoBehaviour {

	public int position = 0;
    public int sampleRate = 44100;
    public float frequency = 440;
	
	public WaveType waveType;
	
	private float minFrequency = 44;
	private float maxFrequency = 880;
	
	private float minPitch = 0.1f;
	private float maxPitch = 3f;
	
	private Vector2 lowerScreenBounds = new Vector2(-10, -5.5f);
	private Vector2 upperScreenBounds = new Vector2(10, 5.5f);
	
	private float minSizeVolume = 0.5f;
	private float maxSizeVolume = 20;
	
	private AudioSource audioSource;
	
	private float transpose = -4;  // transpose in semitones
	
    void Start()
    {
		// Create(string name, int lengthSamples, int channels, int frequency, bool stream, AudioClip.PCMReaderCallback pcmreadercallback, AudioClip.PCMSetPositionCallback pcmsetpositioncallback);
        AudioClip myClip = AudioClip.Create("MySinusoid", sampleRate * 2, 1, sampleRate, true, OnAudioRead, OnAudioSetPosition);
        // AudioClip myClip = AudioClip.Create("MySinusoid", 5000, 1, sampleRate, true, OnAudioRead, OnAudioSetPosition);
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = myClip;
        audioSource.Play();
    }
	
	void Update () {
		if (Input.GetKeyDown("p"))
		{
			audioSource.Play();
		}
		
		float note = -1; // invalid value to detect when note is pressed
		if (Input.GetKeyDown("a")) note = 0;  // C
		if (Input.GetKeyDown("s")) note = 2;  // D
		if (Input.GetKeyDown("d")) note = 4;  // E
		if (Input.GetKeyDown("f")) note = 5;  // F
		if (Input.GetKeyDown("g")) note = 7;  // G
		if (Input.GetKeyDown("h")) note = 9;  // A
		if (Input.GetKeyDown("j")) note = 11; // B
		if (Input.GetKeyDown("k")) note = 12; // C
		if (Input.GetKeyDown("l")) note = 14; // D
		 
		if (note >= 0){ // if some key pressed...
			audioSource.pitch = Mathf.Pow(2, (note + transpose) / 12);
			audioSource.Play();
		}
		
		// PositionAudio();
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
	
	[System.Serializable]
	public enum WaveType {
		Sine,
		Sawtooth,
		Square,
		Noise
	}
}
