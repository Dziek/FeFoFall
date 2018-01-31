using UnityEngine;
using System.Collections;

public class GameItems : MonoBehaviour {
	
	Transform[] objectsInGroup;
	Vector3[] objectsStartingPos;
	
	void Awake () {
		// objectsInGroup = GetComponentsInChildren<Transform>();
		// objectsStartingPos = new Vector3[objectsInGroup.Length];
		
		// for (int i = 1; i < objectsInGroup.Length; i++)
		// {
			// objectsStartingPos[i] = objectsInGroup[i].position;
		// }
		
		int childCount = transform.childCount;
		
		objectsInGroup = new Transform[childCount];
		objectsStartingPos = new Vector3[childCount];
		
		for (int i = 0; i < childCount; i++)
		{
			objectsInGroup[i] = transform.GetChild(i);
			objectsStartingPos[i] = transform.GetChild(i).position;
		}
		
	}
	
	void Reset () {
	// void OnEnable () {
		// for (int i = 1; i < objectsInGroup.Length; i++)
		// {
			// objectsInGroup[i].position = objectsStartingPos[i];
			// Debug.Log("Moved");
		// }
		
		for (int i = 0; i < objectsInGroup.Length; i++)
		{
			objectsInGroup[i].position = objectsStartingPos[i];
			objectsInGroup[i].SendMessage("Reset", SendMessageOptions.DontRequireReceiver);
			
		}
	}
}
