using UnityEngine;
using System.Collections;

public class Trigger : MonoBehaviour {
	
	public GameObject triggerGraphicsGO;
	
	public bool waitForNumberOfTriggers; // use when needing to wait for a group of triggers to all be hit, instead of working out timings
	public float timeActive;
	public GameObject receiver;
	public GameObject[] receivers;
	public bool stopReceiver = false;
	// public bool reverseControls;
	public bool oneShot;
	public bool oneShotDestroy;
	public bool requireBoost; // only works if boost over Trigger
	public int boostTriggerAmountRequired = 0; // if this is more than 0 then it is triggered by boosting instead of collision
	
	public bool playAudio = true;
	
	public Material deactivated;
	private Material activated;
	
	// private Color notActiveColour = new Color32(0, 251, 106, 255);
	// private Color activeColour = new Color32(69, 217, 212, 255);
	
	private Color activeColour = new Color32(69, 217, 212, 255);
	private Color notActiveColour = new Color32(69, 217, 63, 255);
	
	private PlayerControl playerScript;
	private SpriteRenderer sR;
	
	private bool changeCheck; // to make sure it doesn't change every frame
	private int noOfCurrentBoosts; // number of times boosted this level
	
	private bool active = true;
	
	// Use this for initialization
	void Awake () {
		
		if (receiver != null)
		{
			if (receiver.tag != "Player")
			{
				if (stopReceiver == false)
				{
					if (waitForNumberOfTriggers == true)
					{
						receiver.SendMessage("WaitForNumberTrigger");
						// Debug.Log("T");
					}else{
						receiver.SendMessage("WaitForTrigger");
					}
				}
			}
		}
		
		if (receivers.Length > 0)
		{
			for (int i = 0; i < receivers.Length; i++)
			{
				receivers[i].SendMessage("WaitForTrigger");
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
			go.transform.SetSiblingIndex(0);
			
			go.transform.localScale = Vector2.one;
			
			// sR = GetComponentInChildren<SpriteRenderer>();
		}
		
		sR = GetComponentsInChildren<SpriteRenderer>()[1];
		sR.color = activeColour;
		sR.enabled = rendererEnabled;
		
		gameObject.layer = LayerMask.NameToLayer("Trigger");
		
		if (boostTriggerAmountRequired > 0)
		{
			GetComponent<Collider2D>().enabled = false;
		}
	}
	
	void LateUpdate () {
		
		if (boostTriggerAmountRequired > 0)
		{
			bool isBoosting = playerScript.CheckIfBoosting();
			if (isBoosting == true && noOfCurrentBoosts < boostTriggerAmountRequired)
			{
				if (changeCheck == false)
				{	
					noOfCurrentBoosts++;
					
					if (playAudio == true)
					{
						Messenger.Broadcast("Trigger");
					}
					
					if (noOfCurrentBoosts == boostTriggerAmountRequired)
					{
						ActivateTrigger();
					}
									
					changeCheck = true;
				}
				
			}else{
				changeCheck = false;
			}
		}
		
	}
	
	void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.tag == "Player" && active)
		{	
			if (requireBoost == true)
			{
				if (playerScript.CheckIfBoosting() == false)
				{
					return;
				}
			}
	
			ActivateTrigger();
		}
		
	}
	
	void ActivateTrigger () {
		if (receiver != null)
		{
			receiver.SendMessage("TriggerActivated", timeActive);
		}
		
		if (receivers.Length > 0)
		{
			for (int i = 0; i < receivers.Length; i++)
			{
				receivers[i].SendMessage("TriggerActivated", timeActive);
			}
		}
		
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
		
		if (playAudio == true && boostTriggerAmountRequired == 0)
		{
			Messenger.Broadcast("Trigger");
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
	
	void SetPlayer (PlayerControl playerControl) {
		// playerGO = playerControl.gameObject;
		// playerScript = playerGO.GetComponent<PlayerControl>();
		playerScript = playerControl;
	}
	
	void OnEnable () {
		Messenger<PlayerControl>.AddListener("SetPlayer", SetPlayer);
	}
	
	void OnDisable () {
		Messenger<PlayerControl>.RemoveListener("SetPlayer", SetPlayer);
	}
}
