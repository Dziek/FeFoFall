using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelText : MonoBehaviour {

	public string[] bonusSuccessText;
	public string[] bonusFailureText;
	[Tooltip("Only appears after 5 (or so) level failures")]
	public string[] bonusHintText; 
}
