﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class DataManager : MonoBehaviour {
	
	public static Dictionary <string, LevelStats> levelStats = new Dictionary <string, LevelStats>();
	public static Dictionary <Mode, ModeStats> modeStats = new Dictionary <Mode, ModeStats>();
	// public static Dictionary <string, LevelData> levelData = new Dictionary <string, LevelData>();
	// public static Dictionary <string, Dictionary<string, LevelStats>> levelStatsByMode = new Dictionary <string, Dictionary<string, LevelStats>>();
	
	public static void SaveData () {
		// separate Dictionary into two separate Lists
		// List<string> keys = new List<string>();
		// List<LevelStats> values = new List<LevelStats>();
		
		// foreach(var entry in levelStats)
        // {
            // keys.Add(entry.Key);
            // values.Add(entry.Value);
        // }  
		
		List<DictionaryData> data = new List<DictionaryData>();
		
		foreach(var entry in levelStats)
        {
            data.Add(new DictionaryData(entry.Key, entry.Value));
        }  
		
		// save both those Lists
		
		BinaryFormatter formatter = new BinaryFormatter();
		FileStream fs = new FileStream(Application.persistentDataPath + "/LevelStats.dat", FileMode.Create);
		formatter.Serialize(fs, data);
		fs.Close();
		
		SaveModeData();
	}
	
	public static void SaveModeData () {
		
		// Debug.Log("Saving Mode Data");
		
		List<DictionaryDataForMode> data = new List<DictionaryDataForMode>();
		
		foreach(var entry in modeStats)
        {
            data.Add(new DictionaryDataForMode(entry.Key, entry.Value));
        }  
		
		// save both those Lists
		
		BinaryFormatter formatter = new BinaryFormatter();
		FileStream fs = new FileStream(Application.persistentDataPath + "/ModeStats.dat", FileMode.Create);
		formatter.Serialize(fs, data);
		fs.Close();
	}
	
	public static void LoadData () {
		if(File.Exists(Application.persistentDataPath + "/LevelStats.dat")) 
		{
			BinaryFormatter bf = new BinaryFormatter();
			FileStream file = File.Open(Application.persistentDataPath + "/LevelStats.dat", FileMode.Open);
			List<DictionaryData> data = (List<DictionaryData>)bf.Deserialize(file);
			file.Close();
			
			foreach(var d in data)
			{
				levelStats.Add(d.key, d.value);
				// Debug.Log(d.key);
				// Debug.Log(Resources.Load("Levels/C0/" + d.key));
			}
			
			// Debug.Log("DONE");
		}
		
		if(File.Exists(Application.persistentDataPath + "/ModeStats.dat")) 
		{
			BinaryFormatter bf = new BinaryFormatter();
			FileStream file = File.Open(Application.persistentDataPath + "/ModeStats.dat", FileMode.Open);
			List<DictionaryDataForMode> data = (List<DictionaryDataForMode>)bf.Deserialize(file);
			file.Close();
			
			foreach(var d in data)
			{
				modeStats.Add(d.key, d.value);
				// Debug.Log(d.key);
				// Debug.Log(Resources.Load("Levels/C0/" + d.key));
			}
			
			// Debug.Log("DONE");
		}
	}
	
	// public static void LoadModeData () {
		// if(File.Exists(Application.persistentDataPath + "/ModeStats.dat")) 
		// {
			// BinaryFormatter bf = new BinaryFormatter();
			// FileStream file = File.Open(Application.persistentDataPath + "/ModeStats.dat", FileMode.Open);
			// List<DictionaryData> data = (List<DictionaryData>)bf.Deserialize(file);
			// file.Close();
			
			// foreach(var d in data)
			// {
				// modeStats.Add(d.key, d.value);
				// // Debug.Log(d.key);
				// // Debug.Log(Resources.Load("Levels/C0/" + d.key));
			// }
			
			// // Debug.Log("DONE");
		// }
	// }
	
	// public static void ClearModeStats (Mode mode) {
		// // go through every level, making them not completed and setting current attempts / whatever to 0
		
		// // for (int i = 0; i < levelStats[]
		
		// SaveData();
	// }
	
	public static void ClearData () {
		File.Delete(Application.persistentDataPath + "/LevelStats.dat");
		File.Delete(Application.persistentDataPath + "/ModeStats.dat");
	}
	
	[System.Serializable]
	class DictionaryData {
		public string key;
		public LevelStats value; 
		
		public DictionaryData (string k, LevelStats v)
		{
			key = k;
			value = v;
		}
	}  
	
	[System.Serializable]
	class DictionaryDataForMode {
		public Mode key;
		public ModeStats value; 
		
		public DictionaryDataForMode (Mode k, ModeStats v)
		{
			key = k;
			value = v;
		}
	}  
	
	// [System.Serializable]
	// class LevelData {
		// public Mode mode;
		// public LevelStats levelStats; 
		
		// public LevelData (Mode m, LevelStats lS) 
		// {
			// mode = m;
			// levelStats = lS;
		// }
	// }  
}
