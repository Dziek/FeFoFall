using UnityEngine;
using System.Collections;

public class Trigger : MonoBehaviour {
	
	public float timeActive;
	public GameObject receiver;
	public bool oneShot;
	public bool oneShotDestroy;
	
	public bool playAudio = true;
	
	public Material deactivated;
	private Material activated;
	
	private bool active = true;
	
	// Use this for initialization
	void Awake () {
		receiver.SendMessage("WaitForTrigger");
		activated = GetComponent<Renderer>().material;
		
		if (GetComponent<Renderer>().enabled == false)
		{
			playAudio = false;
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.tag == "Player" && active)
		{
			receiver.SendMessage("TriggerActivated", timeActive);
			GetComponent<Renderer>().material = deactivated;
			active = false;
			// Debug.Log("H");
			if (oneShotDestroy)
			{
				Destroy(gameObject);
			}
			if (!oneShot)
			{
				// GetComponent.<Renderer>().material = deactivated;
				StartCoroutine("Reactivate");
			}
			
			if (playAudio == true)
			{
				Messenger.Broadcast("Trigger");
			}
		}
		
	}
	
	IEnumerator Reactivate () {
		
		float timer = 0;
		
		while (timer < timeActive)
		{
			timer += Time.deltaTime;
			yield return null;
		}
		
		yield return null;
		GetComponent<Renderer>().material = activated;
		active = true;
	}
}
