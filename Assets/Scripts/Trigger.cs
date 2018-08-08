using UnityEngine;
using System.Collections;

public class Trigger : MonoBehaviour {
	
	public GameObject triggerGraphicsGO;
	
	public float timeActive;
	public GameObject receiver;
	public bool stopReceiver = false;
	// public bool reverseControls;
	public bool oneShot;
	public bool oneShotDestroy;
	
	public bool playAudio = true;
	
	public Material deactivated;
	private Material activated;
	
	// private Color notActiveColour = new Color32(0, 251, 106, 255);
	// private Color activeColour = new Color32(69, 217, 212, 255);
	
	private Color activeColour = new Color32(69, 217, 212, 255);
	private Color notActiveColour = new Color32(69, 217, 63, 255);
	
	private SpriteRenderer sR;
	
	private bool active = true;
	
	// Use this for initialization
	void Awake () {
		if (receiver.tag != "Player")
		{
			if (stopReceiver == false)
			{
				receiver.SendMessage("WaitForTrigger");
			}
		}
		
		activated = GetComponent<Renderer>().material;
		
		if (GetComponent<Renderer>().enabled == false)
		{
			playAudio = false;
		}
		
		bool rendererEnabled = GetComponent<SpriteRenderer>().enabled;
		
		if (triggerGraphicsGO)
		{
			GetComponent<SpriteRenderer>().enabled = false;
			
			GameObject go = Instantiate(triggerGraphicsGO, transform.position, transform.rotation) as GameObject;
			go.transform.SetParent(transform);
			
			go.transform.localScale = Vector2.one;
			
			// sR = GetComponentInChildren<SpriteRenderer>();
		}
		
		sR = GetComponentsInChildren<SpriteRenderer>()[1];
		sR.color = activeColour;
		sR.enabled = rendererEnabled;
		
		gameObject.layer = LayerMask.NameToLayer("Trigger");
	}
	
	void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.tag == "Player" && active)
		{
			receiver.SendMessage("TriggerActivated", timeActive);
			GetComponent<Renderer>().material = deactivated;
			sR.color = notActiveColour;
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
		sR.color = activeColour;
		active = true;
	}
}
