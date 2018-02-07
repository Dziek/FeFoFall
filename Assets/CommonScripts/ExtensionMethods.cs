using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ExtensionMethods {

	public static void ChanceAdd (this List<string> list, string s, float chance)
	{
		if (Random.Range(0, 100) <= chance)
		{
			list.Add(s);
		}
	}
	
	public static void AddArray (this List<string> list, string[] s)
	{
		for (int i = 0; i < s.Length; i++)
		{
			list.Add(s[i]);
		}
	}
	
	public static string[] RandomiseAlong (this List<string[]> list)
	{
		string[] s = new string[list.Count];
	
		for (int i = 0; i < s.Length; i++)
		{
			s[i] = list[i].GetRandom();
		}
		
		return s;
	}
	
	public static string RandomiseAlongToString (this List<string[]> list)
	{
		string s = "";
	
		for (int i = 0; i < list.Count; i++)
		{
			s += list[i].GetRandom() + " ";
		}
		
		return s;
	}
	
	public static string GetRandom (this string[] s)
	{
		return s[Random.Range(0, s.Length)];
	}
	
	public static string GetRandom (this List<string> s)
	{
		return s[Random.Range(0, s.Count)];
	}
	
	public static string[] AddToArray (this string[] s, string[] arrayToAdd)
	{
		// string[] duringAdditions =  new string[]{"Further!", "I wasn't kidding", "Go on, get", "Run from me"};
		string[] arrayCopy = s;
		
		s = new string[arrayCopy.Length + arrayToAdd.Length];
		arrayCopy.CopyTo(s, 0);
		arrayToAdd.CopyTo(s, arrayCopy.Length);
		
		return s;
	}
}
