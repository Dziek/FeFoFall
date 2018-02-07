using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ObjectPool : MonoBehaviour {

	public GameObject gO;
	
	public bool willGrow;
	
	public List<GameObject> pooledObjects;
	
	private GameObject poolParent;
	private GameObject poolsParent;
	
	public void SetUp (GameObject newGO, int size = 15, bool wG = true) {
		
		pooledObjects = new List<GameObject>();
		
		gO = newGO;
		willGrow = wG;
		
		poolParent = new GameObject(newGO.name + " Object Pool");
		
		string poolsParentName = "PoolsObject";
		poolsParent = GameObject.Find(poolsParentName);
		
		if (poolsParent == null)
		{
			poolsParent = new GameObject(poolsParentName);
			// poolsParent.transform.SetParent(GameObject.Find("LevelElements").transform);
		}
		
		poolParent.transform.SetParent(poolsParent.transform);
		
		for (int i = 0; i < size; i++)
		{
			GameObject temp = Instantiate(gO) as GameObject;
			temp.transform.SetParent(poolParent.transform);
			temp.SetActive(false);
			
			pooledObjects.Add(temp);
		}
	}
	
	public GameObject GetObject () {
	
		for (int i = 0; i < pooledObjects.Count; i++)
		{
			if (!pooledObjects[i].activeInHierarchy)
			{
				return pooledObjects[i];
			}
		}
		
		if (willGrow)
		{
			GameObject tempGO = Instantiate(gO) as GameObject;
			tempGO.transform.SetParent(poolParent.transform);
			tempGO.SetActive(false);
			
			pooledObjects.Add(tempGO);
			
			return tempGO;
		}
		
		return null;
	}
	
	public int GetNoOfActiveObjects () {
		
		int amount = 0;
		
		for (int i = 0; i < pooledObjects.Count; i++)
		{
			if (pooledObjects[i].activeInHierarchy)
			{
				amount++;
			}
		}
		
		return amount;
	}
}
