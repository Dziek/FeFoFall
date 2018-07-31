using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// public class ObjectTransformParent : MonoBehaviour {
public class ObjectManipulation <T> : MonoBehaviour{
	
	// [Tooltip("Whether vectors and local or world space")] 
	public bool useLocalSpace = true; // whether vectors are local or world space
	
	public List<T> destinationPoints; // the data from which to manipulate this object (Positions, angles, scaling data etc)
	
	public bool convertTimeToSpeed; // this keeps uniform speed across different distances
	
	public float executeTimeDefault = 1; // time to execute next step
	public float pauseTimeDefault; // time to pause when reaching either point
	public float delay = 0; // time to wait before executing
	
	[Header ("Options")]
	
	[Tooltip("'Loop' is defined as going from destinationPoint 0 to Count - 0 means unlimited")] 
	public int noOfLoops; // how many Loops to complete before stopping
	
	public bool reverseAtEnd; // only used when multiple Loops - then it's used to see if object should keep lapping or reverse direction
	
	public bool waitForPlayerMovement; // wait for player movement before beginning object movement
	
	// [HideInInspector()]
	public bool skipFirstPause;
	
	[HideInInspector()] public T startPoint;
	[HideInInspector()] public T currentPoint;
	[HideInInspector()] public T nextPoint;

	[HideInInspector()] public T lastFramePoint; // lastPosition check to see if the object has been moved independently of this script since last frame
	
	[HideInInspector()] public bool pause; // whether the program is temporarily paused (by itself or something else)
	[HideInInspector()] public bool dead; // whether the program has been killed by another source
	
	[HideInInspector()] public float executeTime; // local executeTime, should probably make that clearer...
	[HideInInspector()] public float speed; // used for converting time to speed, ensures smooth lerps
	
	[HideInInspector()] public float pauseTimer;
	
	[HideInInspector()] public int currentListPos; // current place in the data list, used to get correct next points
	[HideInInspector()] public int listDirection = 1; // the direction the list is advancing in, can be 1 or -1
	[HideInInspector()] public int noOfStepsCompleted;
	[HideInInspector()] public int noOfLoopsCompleted;
	
	void OnEnable () {
		Messenger.AddListener("FirstMovement", LevelStarted);
		Messenger<bool>.AddListener("levelOver", LevelOver);
	}
	
	void OnDisable () {
		Messenger.RemoveListener("FirstMovement", LevelStarted);
		Messenger<bool>.RemoveListener("levelOver", LevelOver);
		
		StopCoroutine("CalculateUpdate");
	}
	
	// public void ReverseProtection <T>(List<T> paramList, T localData) {
	public void ReverseProtection <U>(List<U> paramList, U localData) {
		paramList.Insert(0, localData);
	}
	
	void Start () {
		
		DestinationPointsSetUp();
		
		executeTime = executeTimeDefault;
		
		// does first pause
		if (skipFirstPause == false && pauseTimeDefault > 0)
		{
			Pause(pauseTimeDefault);
		}
		
		if (waitForPlayerMovement == false)
		{
			StartCoroutine("CalculateUpdate");
		}
	}
	
	void LevelStarted () {
		if (waitForPlayerMovement == true)
		{
			StartCoroutine("CalculateUpdate");
		}
	}
	
	void LevelOver (bool b) {
		StopCoroutine("CalculateUpdate");
	}
	
	IEnumerator CalculateUpdate () {
		float t = 0;
		float pauseT = 0;
		
		currentListPos = 1;
		
		SetUp();
		
		if (delay > 0)
		{
			yield return new WaitForSeconds(delay);
		}
		
		if (convertTimeToSpeed == true)
		{
			// CalculateSpeed();
			speed = executeTimeDefault;
			CalculateTime();
		}
		
		while (dead == false) {
			// Debug.Log("D");
			
				if (t >= executeTime)
				{
					if (pauseTimeDefault > 0)
					{
						Pause(pauseTimeDefault);
						yield return null;
					}
					
					// StepFinished();
					// t = 0;
					
					t -= executeTime;
					StepFinished();
				}
				
			if (pause == false && dead == false)
			{	
				t += Time.deltaTime;
				float lerpAmount = t / executeTime;
				ExecuteUpdate(lerpAmount);
				// Debug.Log("NEW " + Time.time);
			}
			yield return null;
		}
	}
	
	void Pause (float pT) {
		// Debug.Log("Pause");
		pauseTimer += pT;
		pause = true;
		
		// Debug.Log("P " + Time.time);
	}
	
	void Update () {
		// check for pauses and times out
		if(pause == true)
		{
			// Debug.Log(pauseTimer);
			pauseTimer -= Time.deltaTime;
			if (pauseTimer <= 0)
			{
				pause = false;
				// Debug.Log("E " + Time.time);
			}
		}
	}
	
	void StepFinished () {
		
		noOfStepsCompleted++;
		
		currentPoint = destinationPoints[currentListPos];
		
		currentListPos += listDirection;
		
		Debug.Log("Step Finished");
		
		if (noOfStepsCompleted >= (destinationPoints.Count - 1) * (noOfLoopsCompleted + 1))
		{
			LoopFinished();
		}
		
		nextPoint = destinationPoints[currentListPos];
		
		if (convertTimeToSpeed == true)
		{
			CalculateTime();
		}
	}
	
	void LoopFinished () {
		
		Debug.Log("Loop Finished");
		
		noOfLoopsCompleted++;
		
		if (noOfLoopsCompleted >= noOfLoops && noOfLoops > 0)
		{
			dead = true;
		}
		
		if (reverseAtEnd)
		{
			listDirection *= -1; // invert listDirection
			
			currentListPos = Mathf.Clamp(currentListPos, 0, destinationPoints.Count-1); // set currentListPos to either 0 or end of list
			currentPoint = destinationPoints[currentListPos];
			
			currentListPos += listDirection;
		}else{
			currentPoint = startPoint;
			currentListPos = 1;
		}
		
		
	}
	
	// called in awake, makes sure List is in playable state
	public virtual void DestinationPointsSetUp () {
		// ReverseProtection(destinationPoints, 0);
		
		// if (useLocalSpace)
		// {
			// for (int i = 0; i < destinationPoints.Count; i++)
			// {
				// destinationPoints[i] += transform.eulerAngles.z;
			// }
		// }
	}
	
	// for setting up starting values
	public virtual void SetUp () {
		   
		// startPoint = GetRelevantValue();
		   
		// currentPoint = GetRelevantValue();
		// nextPoint = destinationPoints[currentListPos];
		   
		// lastFramePoint = GetRelevantValue();
	}
	
	// what actually happens every tick
	public virtual void ExecuteUpdate (float lerpAmount) {
		
		/* TEMPLATE 
		if (GetRelevantValue() != lastFramePoint)
		{
			RecalculatePosition();
		}else{
			
			transform.position = Vector2.Lerp(currentPoint, nextPoint, lerpAmount); 
		}
		
		lastFramePoint = GetRelevantValue();
		 */
	}
	
	// called when an external script moves object (eg teleporter)
	public virtual void RecalculatePosition () {
		// T difference = (T)(lastFramePoint - GetRelevantValue());
		
		// currentPoint = currentPoint - difference;
		// nextPoint = nextPoint - difference;
	}
	
	public virtual void CalculateSpeed () {
		// speed = Vector2.Distance(currentPoint, nextPoint) / executeTime;
	}
	
	public virtual void CalculateTime () {
		// executeTime = Vector2.Distance(currentPoint, nextPoint) / speed;
	}
	
	public void GetRelevantValue () {
		// return ;
	}
	
	
}
