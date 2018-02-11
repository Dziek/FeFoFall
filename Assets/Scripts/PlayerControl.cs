using UnityEngine;
using System.Collections;

public class PlayerControl : MonoBehaviour {

	public float standardSpeed; // standard speed
	public float boostSpeed; // speed when boosting
	public float boostLength; // how long boost lasts
	public float boostCooldown; // how long until boost can be used again
	public float colourFadeTime; // how long it takes to change colour
	
	public Material normalMat; // how the player looks at normal speed
	public Material boostMat; // how the player looks when boosting
	public Material regainMat; // how the player looks when regaining boost
	
	public ControlledMovement controlledObjectScript; // the object to be controlled as well
	
	private Vector3 startingPos; // where the player starts
	private float speedL; // standard speed (local) 
	private bool boosting; // if boosting
	private bool boostReady = true; // can boost
	// private float boostLengthL; // how long boost lasts (local)
	// private float boostCooldownL; // how long until boost can be used again (local)
	
	private Renderer pR; // player renderer
	
	private LevelInfoDisplay lvlInfoDisplay; // the level info display, to be removed on first movement
	private bool canGoAway = true; // a check to make sure movement doesn't register multiple game attempts
	
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
		
		// lvlInfoDisplay = GameObject.Find("LevelInfo");
	}
	
	// Use this for initialization
	void Start () {
		speedL = standardSpeed;
		// boostCooldownL = boostCooldown;
		
		pR = GetComponent<Renderer>();
		
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
		if (Input.GetAxisRaw("Horizontal") < -0.5f && horizontalInputReset == true) 
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
		StartCoroutine("ChangeColour", boostMat);
		
		float boostLengthL = boostLength;
		
		while (boostLengthL > 0) 
		{
			boostLengthL -= Time.deltaTime;
			yield return null;
		}
		
		speedL = standardSpeed;
		StartCoroutine("ChangeColour", regainMat);
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
		StartCoroutine("ChangeColour", normalMat);
		
		// yield return StartCoroutine("ChangeColour", normalMat);
		// boostReady = true;
	}
	
	IEnumerator ChangeColour (Material mat) {
		
		float cFT = 0;
		Material startingMat = pR.material;
		// Debug.Log(Time.time);
		
		while (cFT < colourFadeTime) 
		{	
			// cFT += Time.deltaTime;
			cFT += colourFadeTime/10;
			pR.material.Lerp(startingMat, mat, 0.1f);

			yield return new WaitForSeconds(colourFadeTime/10);
		}
		
		// Debug.Log(Time.time);
		pR.material = mat;
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
		}
		
		if (controlledObjectScript != null)
		{
			controlledObjectScript.UpdateDir(direction.ToString());
			// controlledObjectScript.UpdateDir(direction);
		}
	}
	
	void Move () {
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

	public void Reset () {
		direction = directions.None;
		transform.position = startingPos;
	}
	
	public void UpdateLevelInfoDisplayObject (LevelInfoDisplay lID) {
		lvlInfoDisplay = lID;
	}
	
	void OnTriggerEnter2D(Collider2D collision)
	{
		if (Application.loadedLevelName == "LevelTesting")
		{
			if(collision.gameObject.tag == "Wall" || collision.gameObject.tag == "Frame")
			{
				Application.LoadLevel(Application.loadedLevel);
			}
		   
			if(collision.gameObject.tag == "End")
			{
				Application.LoadLevel(Application.loadedLevel);
			}
		}else{
		   if(collision.gameObject.tag == "Wall" || collision.gameObject.tag == "Frame")
		   {
			   Messenger.Broadcast("Failure");
			   GameStates.ChangeState("Transition", "Bad");
			   gameObject.SetActive(false);
		   }
		   
		   if(collision.gameObject.tag == "End")
		   {
			   Messenger.Broadcast("Success");
			   GameStates.ChangeState("Transition", "Good");
			   gameObject.SetActive(false);
		   }
		}
	}
	
	public string GetDirection () {
		// Debug.Log(direction.ToString());
		return direction.ToString();
	}
	
}

