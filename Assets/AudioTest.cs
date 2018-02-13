using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioTest : MonoBehaviour {

	public int position = 0;
    public int samplerate = 44100;
    public float frequency = 440;
	
	private AudioSource aud;
	
    void Start()
    {
        // AudioClip myClip = AudioClip.Create("MySinusoid", samplerate * 2, 1, samplerate, true, OnAudioRead, OnAudioSetPosition);
        AudioClip myClip = AudioClip.Create("MySinusoid", 5000, 1, samplerate, true, OnAudioRead, OnAudioSetPosition);
        aud = GetComponent<AudioSource>();
        aud.clip = myClip;
        aud.Play();
    }
	
	void Update () {
		if (Input.GetKeyDown("p"))
		{
			aud.Play();
		}
	}
	
	void OnAudioRead(float[] data)
    {
        int count = 0;
        while (count < data.Length)
        {
            data[count] = Mathf.Sign(Mathf.Sin(2 * Mathf.PI * frequency * position / samplerate));
            position++;
            count++;
        }
    }

    void OnAudioSetPosition(int newPosition)
    {
        position = newPosition;
    }
}
