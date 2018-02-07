using UnityEngine;
using System.Collections;

public class Spawner : MonoBehaviour {
	
	public GameObject obj;
	
	// public int amountOfObj = 15;
	
	private ObjectPool objects;
	
	void Awake () {
		objects = gameObject.AddComponent<ObjectPool>();
		objects.SetUp(obj);
	}
	
	public void SpawnObject () {
		
		GameObject o = objects.GetObject();
		o.transform.position = transform.position;
		o.SetActive(true);
	}
}
