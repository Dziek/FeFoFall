using UnityEngine;
using System.Collections;

public class PlayerControl : MonoBehaviour {

	public GameObject playerGraphicsGO;
	public bool noLeft = false;
	public bool reverseControls = false; // should reverse controls?

	public float standardSpeed; // standard speed
	public float boostSpeed; // speed when boosting
	public float boostLength; // how long boost lasts
	public float boostCooldown; // how long until boost can be used again
	private float colourFadeTime = 0.2f; // how long it takes to change colour
	
	public Material normalMat; // how the player looks at normal speed
	public Material boostMat; // how the player looks when boosting
	public Material regainMat; // how the player looks when regaining boost
	
	public Color normalColour = new Color32(255, 255, 0, 255); // how the player looks at normal speed
	public Color boostColour = new Color32(250, 75, 60, 255);  // how the player looks when boosting
	public Color regainColour = new Color32(234, 234, 105, 255);  // how the player looks when regaining boost
	
	public ControlledMovement controlledObjectScript; // the object to be controlled as well
	
	private Vector3 startingPos; // where the player starts
	private float speedL; // standard speed (local) 
	private bool boosting; // if boosting
	private bool boostReady = true; // can boost
	// private float boostLengthL; // how long boost lasts (local)
	// private float boostCooldownL; // how long until boost can be used again (local)
	
	private Renderer pR; // player renderer
	private SpriteRenderer sR; // player renderer
	
	private LevelInfoDisplay lvlInfoDisplay; // the level info display, to be removed on first movement
	private bool canGoAway = true; // a check to make sure movement doesn't register multiple game attempts
	
	// private bool canMove = false; // GameStates.Playing check does this too
	
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
			GameObject go = Instantiate(playerGraphicsGO, transform.position, transform.rotation) as GameObject;
			go.transform.SetParent(transform);
			
			go.transform.localScale = Vector2.one;
		}
		
		// Debug.Log("Here");
	}
	
	// Use this for initialization
	void Start () {
		speedL = standardSpeed;
		// boostCooldownL = boostCooldown;
		
		pR = GetComponent<Renderer>();
		sR = GetComponent<SpriteRenderer>();
		
		// pR.material = normalMat;
		sR.color = normalColour;
		
		startingPos = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		// if (GameStates.State == GameStates.States.Playing) 
		if (GameStates.GetState() == "Playing")
		{
			CheckControls();
			// CheckBoost();
			Move();
		}
	}
	
	void CheckControls () {
		// if (Input.GetKeyDown("w")||Input.GetKeyDown("up"))
		if (Input.GetAxisRaw("Vertical") > 0.5f && verticalInputReset == true) 
		{
			RegisterInput("Up");
			verticalInputReset = false;
		}
		// if (Input.GetKeyDown("s")||Input.GetKeyDown("down")) 
		if (Input.GetAxisRaw("Vertical") < -0.5f && verticalInputReset == true) 
		{
			RegisterInput("Down");
			verticalInputReset = false;
		}
		// if (Input.GetKeyDown("d")||Input.GetKeyDown("right")) 
		if (Input.GetAxisRaw("Horizontal") > 0.5f && horizontalInputReset == true) 
		{	
			RegisterInput("Right");
			horizontalInputReset = false;
		}
		// if (Input.GetKeyDown("a")||Input.GetKeyDown("left")) 
		if (Input.GetAxisRaw("Horizontal") < -0.5f && horizontalInputReset == true && noLeft == false) 
		{
			RegisterInput("Left");
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
				
		// #if UNITY_ANDROID
			// if (Input.touchCount > 0)
			// {
				// targetp1 = Camera.main.ScreenToWorldPoint(Input.mousePosition);
				// targetp1.z = transform.position.z;
			// }
			// transform.position = Vector3.MoveTowards(transform.position, targetp1, speed * Time.deltaTime); //Uses normal speed instead of touchSpeed
		// #endif
		
		
	}
	
	IEnumerator Boost () {
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
	
	public void RegisterInput (string dir) {
		
		selectedDirection = (directions) System.Enum.Parse (typeof(directions), dir);
	
		if (direction == selectedDirection && boosting == false && boostReady)
		{
			boosting = true;
			boostReady = false;
			
			// boostLengthL = boostLength;
			// boostCooldownL = boostCooldown;
			
			StartCoroutine("Boost");
		}else{
			direction = selectedDirection;
		}
		
		if (lvlInfoDisplay != null && canGoAway)
		{
			lvlInfoDisplay.GoAway();
			canGoAway = false;
			
			Messenger.Broadcast("FirstMovement");
			Messenger.Broadcast("LevelStarted");
			
			if (Application.loadedLevelName != "LevelTesting")
			{
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
		
		if (reverseControls == false)
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
		}else{
			switch (direction)
			{
				case directions.Up:
					transform.Translate(Vector3.up * (Time.deltaTime * -speedL), Space.World);
				break;
				case directions.Down:
					transform.Translate(Vector3.up * (Time.deltaTime * speedL), Space.World);
				break;
				case directions.Right:
					transform.Translate(Vector3.right * (Time.deltaTime * -speedL), Space.World);
				break;
				case directions.Left:
					transform.Translate(Vector3.right * (Time.deltaTime * speedL), Space.World);
				break;
			}
		}
	}

	public void Reset () {
		direction = directions.None;
		transform.position = startingPos;
	}
	
	public void UpdateLevelInfoDisplayObject (LevelInfoDisplay lID) {
		lvlInfoDisplay = lID;
	}
	
	void OnTriggerEnter2D(Collider2D other)
	{
		if (Application.loadedLevelName == "LevelTesting" || Application.loadedLevelName == "GraphicsTesting" )
		{
			if(other.gameObject.tag == "Wall" || other.gameObject.tag == "Frame")
			{
				Application.LoadLevel(Application.loadedLevel);
			}
		   
			if(other.gameObject.tag == "End")
			{
				Application.LoadLevel(Application.loadedLevel);
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
			   
			    Messenger<TransitionReason>.Broadcast("Transition", TransitionReason.levelFailure);
			   
			    Transform endPoint = GameObject.Find("EndPoint").transform;
			    float distance = Vector2.Distance(transform.position, endPoint.position);
			    float convertedDistance = distance - (transform.localScale.x / 2) - (endPoint.localScale.x / 2);
			   
			    // Debug.Log("Distance " + distance + " Converted Distance " + convertedDistance);
			    Messenger<bool>.Broadcast("CloseCall", convertedDistance < 0.4f);
				
				gameObject.SetActive(false);
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
				
				Vector2 v = other.gameObject.GetComponent<Collider2D>().bounds.ClosestPoint(transform.position);
				
				GameObject.Find("LevelCompletePS").GetComponent<LevelCompletePS>().Fire(v);
				
				gameObject.SetActive(false);
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
	
	// void UpdateCanMove () {
		// canMove = true;
	// }
	
	// void OnEnable () {
		// Messenger.AddListener("TransitionComplete", UpdateCanMove);
	// }
	
	// void OnDisable () {
		// Messenger.RemoveListener("TransitionComplete", UpdateCanMove);
	// }
	
}

