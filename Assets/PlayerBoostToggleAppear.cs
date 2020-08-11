using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBoostToggleAppear : MonoBehaviour
{
	public bool startActive = true;
	public int amountOfBoostsRequired = 0; 
	
	private bool active;
	private bool changeCheck; // to make sure it doesn't change every frame
	private int noOfCurrentBoosts; // number of times boosted this level
	
	private GameObject playerGO;
	private PlayerControl playerScript;
	
	private BoxCollider2D col; 
	private GameObject graphicsGO;
	
	void Start () {
		
		active = startActive;
		
		col = GetComponent<BoxCollider2D>();
		graphicsGO = transform.GetChild(0).gameObject;
		
		col.enabled = active;
		graphicsGO.SetActive(active);

	}
	
	void LateUpdate () {
		
		bool isBoosting = playerScript.CheckIfBoosting();
		if (isBoosting == true)
		{
			if (changeCheck == false)
			{	
				if (amountOfBoostsRequired == 0)
				{
					active = !active;
				
					col.enabled = active;
					graphicsGO.SetActive(active);

				}
				
				if (amountOfBoostsRequired > 0)
				{
					noOfCurrentBoosts++;
					if (noOfCurrentBoosts == amountOfBoostsRequired)
					{
						active = !active;
				
						col.enabled = active;
						graphicsGO.SetActive(active);
					}
				}
								
				changeCheck = true;
			}
			
		}else{
			changeCheck = false;
		}
		
	}
	
	void SetPlayer (PlayerControl playerControl) {
		playerGO = playerControl.gameObject;
		playerScript = playerGO.GetComponent<PlayerControl>();
	}
	
	void OnEnable () {
		Messenger<PlayerControl>.AddListener("SetPlayer", SetPlayer);
	}
	
	void OnDisable () {
		Messenger<PlayerControl>.RemoveListener("SetPlayer", SetPlayer);
	}
}
