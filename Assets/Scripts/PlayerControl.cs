using UnityEngine;
using System.Collections;

public class PlayerControl : MonoBehaviour {

	public GameObject playerGraphicsGO;
	public bool noLeft = false;
	public bool noUp = false;
	public bool noDown = false;
	public bool noRight = false;
	public bool reverseControls = false; // should reverse controls?
	public bool reverseControlsX = false; // should reverse controls?
	public int turnCount; // number of turns allowed, 0 is unlimited
	public float turnSpeedIncrease; // the amount to increase speed by each turn
	public bool holdControls; // makes it so you need to hold a direction to keep going
	public bool ghostStart; // turns collider off until first move

	public float standardSpeed; // standard speed
	public float boostSpeed; // speed when boosting
	public float boostLength; // how long boost lasts
	public float boostCooldown; // how long until boost can be used again
	public bool nonStopBoosts; // this means the player will boost as soon as cooldown goes down automatically
	private float colourFadeTime = 0.2f; // how long it takes to change colour
	
	public Material normalMat; // how the player looks at normal speed
	public Material boostMat; // how the player looks when boosting
	public Material regainMat; // how the player looks when regaining boost
	
	public Color normalColour = new Color32(255, 255, 0, 255); // how the player looks at normal speed
	public Color boostColour = new Color32(250, 75, 60, 255);  // how the player looks when boosting
	public Color regainColour = new Color32(234, 234, 105, 255);  // how the player looks when regaining boost
	
	public ControlledMovement controlledObjectScript; // the object to be controlled as well
	public GameObject parentOnStartGO; // on first movement change parent to this
	
	private Vector3 startingPos; // where the player starts
	private float speedL; // standard speed (local) 
	private bool boosting; // if boosting
	private bool boostReady = true; // can boost
	// private float boostLengthL; // how long boost lasts (local)
	// private float boostCooldownL; // how long until boost can be used again (local)
	
	// private Renderer pR; // player renderer
	private SpriteRenderer sR; // player renderer
	private GameObject graphicsGO; // local childed graphics GameObject
	[HideInInspector]
	public GameObject shadowGO; // local childed graphics GameObject
	[HideInInspector]
	public bool isMoving; // whether the player is moving
	
	private bool isActive = true; // whether it has control / should move
	private bool stopped; // whether it is stopped
	private bool freezeInput; // whether inputs are registered
	
	private LevelCompletePS levelCompletePS;
	
	private LevelInfoDisplay lvlInfoDisplay; // the level info display, to be removed on first movement
	private bool canGoAway = true; // a check to make sure movement doesn't register multiple game attempts
	
	private Collider2D collider;
	
	// private bool canMove = false; // GameStates.Playing check does this too
	
	public enum TriggerActions{
		None,
		ReverseControls,
		TurnInvisible,
		TurnShadow,
		StopMoving
	}public TriggerActions triggerAction = TriggerActions.None;
	
	public enum directions{
		Up,
		Down,
		Right,
		Left,
		None
	}public directions direction = directions.None, selectedDirection;
	
	private bool horizontalInputReset;
	private bool verticalInputReset;
	
	void Awake () {
		#if UNITY_ANDROID
			TouchControl.UpdatePlayerObject(this);
		#endif
		
		// Messenger<GameObject>.Broadcast("FillPlayerGO", gameObject);
		// lvlInfoDisplay = GameObject.Find("LevelInfo");
		
		// normalMat = Resources.Load("Materials/PlayerTile") as Material;
		// boostMat = Resources.Load("Materials/PlayerBoostTile") as Material;
		// regainMat = Resources.Load("Materials/PlayerNoBoostTile") as Material;
		
		// gameObject.AddComponent<MatOffset>();
		
		// GetComponent<SpriteRenderer>().enabled = false;
		
		if (playerGraphicsGO)
		{
			graphicsGO = Instantiate(playerGraphicsGO, transform.position, transform.rotation) as GameObject;
			graphicsGO.transform.SetParent(transform);
			
			graphicsGO.transform.localScale = Vector2.one;
		}
		
		levelCompletePS = GameObject.Find("LevelCompletePS").GetComponent<LevelCompletePS>();
		// Debug.Log("Here");
		
		collider = GetComponent<Collider2D>();
		
		
	}
	
	// Use this for initialization
	void Start () {
		speedL = standardSpeed;
		// boostCooldownL = boostCooldown;
		
		// pR = GetComponent<Renderer>();
		sR = GetComponent<SpriteRenderer>();
		
		// pR.material = normalMat;
		sR.color = normalColour;
		
		startingPos = transform.position;
		
		// GameObject.Find("TransitionControllers").GetComponent<TransitionScreen>().SetPlayer(this);
		Messenger<PlayerControl>.Broadcast("SetPlayer", this);
		// Debug.Log("BroadcastPlayer");
		
		if (ghostStart == true)
		{
			// isActive = false;
			ChangeActive(false);
		}
	}
	
	// Update is called once per frame
	void Update () {
		// if (GameStates.State == GameStates.States.Playing) 
		if (GameStates.GetState() == "Playing")
		{
			if (freezeInput == false)
			{
				CheckControls();
			}
			// CheckBoost();
			Move();
		}
	}
	
	void CheckControls () {
		// if (Input.GetKeyDown("w")||Input.GetKeyDown("up"))
		if (Input.GetAxisRaw("Vertical") > 0.5f && verticalInputReset == true && noUp == false) 
		{
			if (reverseControls == false)
			{
				RegisterInput("Up");
			}else{
				RegisterInput("Down");
			}
			
			isMoving = true;
			verticalInputReset = false;
		}
		// if (Input.GetKeyDown("s")||Input.GetKeyDown("down")) 
		if (Input.GetAxisRaw("Vertical") < -0.5f && verticalInputReset == true && noDown == false) 
		{
			if (reverseControls == false)
			{
				RegisterInput("Down");
			}else{
				RegisterInput("Up");
			}
			
			isMoving = true;
			verticalInputReset = false;
		}
		// if (Input.GetKeyDown("d")||Input.GetKeyDown("right")) 
		if (Input.GetAxisRaw("Horizontal") > 0.5f && horizontalInputReset == true && noRight == false) 
		{	
			if (reverseControls == false && reverseControlsX == false)
			{
				RegisterInput("Right");
			}else{
				RegisterInput("Left");
			}
			
			isMoving = true;
			horizontalInputReset = false;
		}
		// if (Input.GetKeyDown("a")||Input.GetKeyDown("left")) 
		if (Input.GetAxisRaw("Horizontal") < -0.5f && horizontalInputReset == true && noLeft == false) 
		{
			if (reverseControls == false && reverseControlsX == false)
			{
				RegisterInput("Left");
			}else{
				RegisterInput("Right");
			}
			
			isMoving = true;
			horizontalInputReset = false;
		}
		
		if (Input.GetAxisRaw("Horizontal") == 0)
		{
			horizontalInputReset = true;
		}
		
		if (Input.GetAxisRaw("Vertical") == 0)
		{
			verticalInputReset = true;
		}
		
		if (Input.GetButtonDown("Boost"))
		{
			AttemptBoost();
		}
				
		// #if UNITY_ANDROID
			// if (Input.touchCount > 0)
			// {
				// targetp1 = Camera.main.ScreenToWorldPoint(Input.mousePosition);
				// targetp1.z = transform.position.z;
			// }
			// transform.position = Vector3.MoveTowards(transform.position, targetp1, speed * Time.deltaTime); //Uses normal speed instead of touchSpeed
		// #endif
		
		if (holdControls == true)
		{
			if (Input.GetAxisRaw("Horizontal") == 0 && Input.GetAxisRaw("Vertical") == 0)
			{
				RegisterInput("None");
				isMoving = false;
			}
		}
		
	}
	
	IEnumerator Boost () {
		
		Messenger.Broadcast("Boost");
		
		speedL = boostSpeed;
		// StartCoroutine("ChangeColour", boostMat);
		StartCoroutine("ChangeColour", boostColour);
		
		float boostLengthL = boostLength;
		
		while (boostLengthL > 0) 
		{
			boostLengthL -= Time.deltaTime;
			yield return null;
		}
		
		speedL = standardSpeed;
		// StartCoroutine("ChangeColour", regainMat);
		StartCoroutine("ChangeColour", regainColour);
		boosting = false;
		StartCoroutine("RegainBoost");
	}
	
	IEnumerator RegainBoost () {
		float boostCooldownL = boostCooldown;
		
		while (boostCooldownL > 0) 
		{
			boostCooldownL -= Time.deltaTime;
			yield return null;
		}
		
		boostReady = true;
		// StartCoroutine("ChangeColour", normalMat);
		StartCoroutine("ChangeColour", normalColour);
		
		// yield return StartCoroutine("ChangeColour", normalMat);
		// boostReady = true;
	}
	
	// IEnumerator ChangeColour (Material mat) {
	IEnumerator ChangeColour (Color newColour) {
		
		float cFT = 0;
		// Material startingMat = pR.material;
		Color startingColour = sR.color;
		// Debug.Log(Time.time);
		
		while (cFT < colourFadeTime) 
		{	
			cFT += Time.deltaTime;
			// cFT += colourFadeTime/10;
			// pR.material.Lerp(startingMat, mat, 0.1f);
			
			float lerpValue = cFT/colourFadeTime;
			// float lerpValue = colourFadeTime/cFT;
			
			sR.color = Color.Lerp(startingColour, newColour, lerpValue);
			
			// Debug.Log(lerpValue);
			
			yield return null;
			
			// yield return new WaitForSeconds(colourFadeTime/10);
		}
		
		// Debug.Log(Time.time);
		// pR.material = mat;
		sR.color = newColour;
		yield return null;
	}
	
	void AttemptBoost () {
		if(boosting == false && boostReady)
		{
			boosting = true;
			boostReady = false;
			
			// boostLengthL = boostLength;
			// boostCooldownL = boostCooldown;
			
			StartCoroutine("Boost");
		}
	}
	
	public void RegisterInput (string dir) {
		
		if (ghostStart == true)
		{
			ghostStart = false;
			// isActive = true;
			ChangeActive(true);
		}
		
		if (isActive == false)
		{
			// quit out and don't process any input
			return;
		}
		
		selectedDirection = (directions) System.Enum.Parse (typeof(directions), dir);
	
		if (direction == selectedDirection)
		{
			AttemptBoost();
		}else{
			if (turnCount > 0)
			{
				turnCount--;
				if (turnCount == 0)
				{
					noLeft = true;
					noRight = true;
					noUp = true;
					noDown = true;
				}
			}
			
			if (turnSpeedIncrease > 0)
			{
				standardSpeed += turnSpeedIncrease;
				boostSpeed += turnSpeedIncrease;
				
				speedL += turnSpeedIncrease;
			}
			
			direction = selectedDirection;
		}
		
		// Debug.Log(gameObject.name);
		
		if (lvlInfoDisplay != null && canGoAway)
		{
			lvlInfoDisplay.GoAway();
			canGoAway = false;
			
			Messenger.Broadcast("FirstMovement");
			// Messenger.Broadcast("LevelStarted");
			
			if (parentOnStartGO != null)
			{
				transform.SetParent(parentOnStartGO.transform);
			}
			
			if (Application.loadedLevelName != "LevelTesting")
			{
				Messenger.Broadcast("LevelStarted");
				// LoadLevel.StartTimer();
			}
		}
		
		if (controlledObjectScript != null)
		{
			controlledObjectScript.UpdateDir(direction.ToString());
			// controlledObjectScript.UpdateDir(direction);
		}
	}
	
	void Move () {
		
		if (nonStopBoosts == true)
		{
			AttemptBoost();
		}
		
		// if (reverseControls == false)
		// {
			
			if (stopped == false)
			{
				switch (direction)
				{
					case directions.Up:
						transform.Translate(Vector3.up * (Time.deltaTime * speedL), Space.World);
					break;
					case directions.Down:
						transform.Translate(Vector3.up * (-Time.deltaTime * speedL), Space.World);
					break;
					case directions.Right:
						transform.Translate(Vector3.right * (Time.deltaTime * speedL), Space.World);
					break;
					case directions.Left:
						transform.Translate(Vector3.right * (-Time.deltaTime * speedL), Space.World);
					break;
				}
			}
			
		// }else{
			// switch (direction)
			// {
				// case directions.Up:
					// transform.Translate(Vector3.up * (Time.deltaTime * -speedL), Space.World);
				// break;
				// case directions.Down:
					// transform.Translate(Vector3.up * (Time.deltaTime * speedL), Space.World);
				// break;
				// case directions.Right:
					// transform.Translate(Vector3.right * (Time.deltaTime * -speedL), Space.World);
				// break;
				// case directions.Left:
					// transform.Translate(Vector3.right * (Time.deltaTime * speedL), Space.World);
				// break;
			// }
		// }
	}
	
	void ChangeActive (bool newActiveState) {
		isActive = newActiveState;
		
		collider.enabled = isActive;
		
		if (isActive == true)
		{
			Color32 t = sR.color;
			t.a = 255;
			sR.color = t;
		}else{
			Color32 t = sR.color;
			t.a = 120;
			sR.color = t;
		}
	}

	public void Reset () {
		direction = directions.None;
		transform.position = startingPos;
	}
	
	public void UpdateLevelInfoDisplayObject (LevelInfoDisplay lID) {
		// Debug.Log(gameObject.name);
		lvlInfoDisplay = lID;
	}
	
	void OnTriggerEnter2D(Collider2D other)
	{
		if (Application.loadedLevelName == "LevelTesting" || Application.loadedLevelName == "GraphicsTesting"  || Application.loadedLevelName == "Aesthetics" )
		{
			if(other.gameObject.tag == "Wall" || other.gameObject.tag == "Frame")
			{
				Application.LoadLevel(Application.loadedLevel);
			}
		   
			if(other.gameObject.tag == "End")
			{
				Application.LoadLevel(Application.loadedLevel);
				Debug.Log("Did It!");
			}
		}else{
		    if(other.gameObject.tag == "Wall" || other.gameObject.tag == "Frame")
		    {
				// LoadLevel.StopTimer();
				
			    Messenger.Broadcast("LevelOver");
			    Messenger.Broadcast("Failure");
			    GameStates.ChangeState("Transition", "Bad");
				
				// modify intensity and shake duration based on playerSpeed
		
				MinMax playerSpeedValues = new MinMax(1, 16);
				MinMax intensityValues = new MinMax(0.05f, 0.16f);
				MinMax durationValues = new MinMax(0.05f, 0.12f);
				
				float l = Mathf.InverseLerp(playerSpeedValues.min, playerSpeedValues.max, speedL);
				float shakeIntensity = Mathf.Lerp(intensityValues.min, intensityValues.max, l);
				float shakeDuration = Mathf.Lerp(durationValues.min, durationValues.max, l);
				
			    Messenger<float, float>.Broadcast("screenshake", shakeIntensity, shakeDuration);
			    // Messenger<float, float>.Broadcast("screenshake", 0.08f, 0.03f);
				
				
				Messenger<PlayerControl>.Broadcast("SetPlayer", this); // Testing new thing - trying to make it recognise which player hit a wall
			    Messenger<TransitionReason>.Broadcast("Transition", TransitionReason.levelFailure);
				
			   
			    Transform endPoint = GameObject.Find("EndPoint").transform;
			    float distance = Vector2.Distance(transform.position, endPoint.position);
			    float convertedDistance = distance - (transform.localScale.x / 2) - (endPoint.localScale.x / 2);
			   
			    // Debug.Log("Distance " + distance + " Converted Distance " + convertedDistance);
			    Messenger<bool>.Broadcast("CloseCall", convertedDistance < 0.4f);
				
				// TRYING NEW THINGS
				// gameObject.SetActive(false);
				
				// graphicsGO.SetActive(false);
				// gameObject.GetComponent<Collider2D>().enabled = false;
		    }
		   
		    if(other.gameObject.tag == "End")
		    {
				// LoadLevel.StopTimer();
				
			    Messenger.Broadcast("LevelOver");
			    Messenger.Broadcast("Success");
			    GameStates.ChangeState("Transition", "Good");
			    
			    // Messenger<float, float>.Broadcast("screenshake", 0.04f, 0.75f);
			    Messenger<float>.Broadcast("StartConstantShake", 0.04f);
			
			    Messenger<TransitionReason>.Broadcast("Transition", TransitionReason.levelSuccess);
				
				Vector2 collisionPoint = other.gameObject.GetComponent<Collider2D>().bounds.ClosestPoint(transform.position);
				
				if (GameObject.Find("LevelCamera") != null)
				{
					Camera cam = GameObject.Find("LevelCamera").GetComponent<Camera>();
					
					collisionPoint = cam.WorldToViewportPoint(collisionPoint);
					
					collisionPoint = Camera.main.ViewportToWorldPoint(collisionPoint);
				}
				
				levelCompletePS.Fire(collisionPoint);
				
				// TRYING NEW THINGS
				// gameObject.SetActive(false);
				
				// graphicsGO.SetActive(false);
				// gameObject.GetComponent<Collider2D>().enabled = false;
		    }
		}
	}
	
	public string GetDirection () {
		// Debug.Log(direction.ToString());
		return direction.ToString();
	}
	
	public float GetSpeed () {
		return speedL;
	}
	
	// void OnDisable () {
		// if (LoadLevel.instance != null)
		// {
			// LoadLevel.StopTimer();
		// }
	// }
	
	public void TriggerActivated (float time) {
		switch (triggerAction)
		{
			case TriggerActions.ReverseControls:
				if (time != 0)
				{
					StartCoroutine("ReverseControlCountdown", time);
				}else{
					reverseControls = !reverseControls;
				}
			break;
			
			case TriggerActions.TurnInvisible:
				if (time != 0)
				{
					// StartCoroutine("ReverseControlCountdown", time);
					Debug.Log("Not coded this in!");
				}else{
					sR.enabled = !sR.enabled;
					graphicsGO.active = !graphicsGO.active;
					shadowGO.active = !shadowGO.active;
				}
			break;
			
			case TriggerActions.TurnShadow:
				if (time != 0)
				{
					// StartCoroutine("ReverseControlCountdown", time);
					Debug.Log("Not coded this in!");
				}else{
					// graphicsGO.active = !graphicsGO.active;
					sR.enabled = !sR.enabled;
					graphicsGO.active = !graphicsGO.active;
				}
			break;
			
			case TriggerActions.StopMoving:
				if (stopped == false)
				{	
					if (time != 0)
					{
						StartCoroutine("StopMoving", time);
					}
					
					stopped = true;
				}else{
					stopped = false;
				}
			break;
		}
	}
	
	IEnumerator StopMoving (float time) {
		
		float t = 0;
		
		while (t < time) 
		{
			t += Time.deltaTime;
			yield return null;
		}
		
		stopped = false;
	}
	
	public void ReverseControls (float time) {
		reverseControls = !reverseControls;
	}
	
	public void ReverseControlsTime (float time) {
		StopCoroutine("ReverseControlCountdown");
		StartCoroutine("ReverseControlCountdown", time);
	}
	
	IEnumerator ReverseControlCountdown (float reverseTime) {
		float t = 0;
		reverseControls = true;
		
		while (t < reverseTime)
		{
			t += Time.deltaTime;
			yield return null;
		}
		
		reverseControls = false;
	}
	
	public void FreezeInput (bool v) {
		freezeInput = v;
	}
	
	public float GetCurrentSpeed () {
		return speedL;
	}
	
	public bool CheckIfBoosting () {
		return boosting;
	}
	
	// void UpdateCanMove () {
		// canMove = true;
	// }
	
	// void OnEnable () {
		// Messenger.AddListener("TransitionComplete", UpdateCanMove);
	// }
	
	// void OnDisable () {
		// Messenger.RemoveListener("TransitionComplete", UpdateCanMove);
	// }
	
	// void ondrawgizmos () {
        // gizmos.color = color.green;
		
		// // camera cam = camera.main;
		// camera cam = gameobject.find("camera").getcomponent<camera>();
		
		// vector3 screenpos = cam.worldtoviewportpoint(transform.position);
		
		// screenpos = camera.main.viewporttoworldpoint(screenpos);
		// // screenpos = cam.viewporttoworldpoint(screenpos);
		
		// // debug.log("sp: " + screenpos + " ap " + transform.position);
        // gizmos.drawsphere(screenpos, 1f);
        
    // }
	
}

