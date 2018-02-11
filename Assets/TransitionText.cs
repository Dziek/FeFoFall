using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TransitionText : MonoBehaviour {

	public Text displayText;
	
	public string[] successTextOptions;
	public string[] failureTextOptions;
	public string[] loadTextOptions;
	public string completeText;
	
	private string lastText;
	// private string newText;
	
	private TransitionController controller;
	
	void Awake () {
		controller = GetComponent<TransitionController>();
	}
	
	public string ChooseText () {
		
		if (controller.gameCompleted == true)
		{
			return completeText;
		}
		
		switch (controller.transitionState)
		{
			case TransitionState.levelLoad:
				return loadTextOptions[Random.Range(0, loadTextOptions.Length)];
			break;
			
			case TransitionState.levelSuccess:
				List<string> options = new List<string>(successTextOptions);
				
				int currentLevelAttempts = LoadLevel.GetCurrentLevelCurrentAttempts();
				
				if (currentLevelAttempts == 1)
				{
					options.Add("First Time Lucky!");
					options.Add("Hole In One!");
				}
				
				if (currentLevelAttempts >= 3)
				{
					options.Add(GetNumberSuffix(currentLevelAttempts) + " Times Lucky");
				}
				
				if (currentLevelAttempts >= 10)
				{
					options.Add("Finally");
				}
				
				switch (LoadLevel.GetPercentageComplete().ToString()) {
					case "0.25":
						options.Add("Quarter Done!");
					break;
					
					case "0.5":
						options.Add("Halfway There!");
					break;
					
					case "0.75":
						options.Add("Three Quarters Done!");
					break;
					
					default:
						options.Add((LoadLevel.GetPercentageComplete() * 100).ToString() + "% Done!");
					break;
				}
				
				if (LoadLevel.GetPercentageComplete() > 0.45f && LoadLevel.GetPercentageComplete() < 0.5f)
				{
					options.Add("Nearly Halfway There!");
				}
				
				if (LoadLevel.GetPercentageComplete() > 0.92f && LoadLevel.GetPercentageComplete() < 0.98f)
				{
					options.Add("Not Long To Go Now");
					options.Add("Just A Few More");
					options.Add("Nearly There");
				}
				
				switch (LoadLevel.GetNumberOfLevelsRemaining()) {
					case 5:
						options.Add("Final Five");
					break;
					
					case 4:
						options.Add("Four More To Go!");
					break;
					
					case 3:
						options.Add("Just Three To Go!");
					break;
					
					case 2:
						options.Add("Only Two Left!");
					break;
					
					case 1:
						options.Add("Last Level!");
					break;
					
					default:
						options.Add("Just " + LoadLevel.GetNumberOfLevelsRemaining().ToString() + " Levels To Go");
						options.Add("Only " + LoadLevel.GetNumberOfLevelsRemaining().ToString() + " Levels Remaining");
						options.Add("Now Do That " + LoadLevel.GetNumberOfLevelsRemaining().ToString() + " More Times");
					break;
				}
				
				options.Add(LoadLevel.GetLevelsCompleted().ToString() + " Down, " + LoadLevel.GetNumberOfLevelsRemaining().ToString() + " To Go");
				options.Add(LoadLevel.GetLevelsCompleted().ToString() + " / " + LoadLevel.GetNumberOfLevels().ToString());
				options.Add(LoadLevel.GetLevelsCompleted().ToString() + " Levels Completed");
				options.Add("That's Level " + LoadLevel.GetLevelsCompleted().ToString() + " Bested");
				options.Add("Your " + GetNumberSuffix(LoadLevel.GetLevelsCompleted()) + " Success");
				
				options.Add("Press B To Return To Menu");
				
				return options[Random.Range(0, options.Count)];
			break;
			
			case TransitionState.levelFailure:
				List<string> optionsF = new List<string>(failureTextOptions);
				
				if (LoadLevel.GetCurrentLevelCurrentAttempts() > 1)
				{
					optionsF.Add("Again?");
				}
				
				if (LoadLevel.GetCurrentLevelCurrentAttempts() > 5)
				{
					optionsF.Add(LoadLevel.GetCurrentLevelCurrentAttempts().ToString() + " Tries Already?");
					optionsF.Add("Level Attempts: " + LoadLevel.GetCurrentLevelCurrentAttempts().ToString());
				}
				
				if (LoadLevel.GetCurrentAttempts() > 20)
				{
					optionsF.Add(GetNumberSuffix(LoadLevel.GetCurrentAttempts()) + " Current Attempt");
					optionsF.Add(LoadLevel.GetCurrentAttempts().ToString() + " Current Attempts");
				}
				
				if (LoadLevel.GetTimesCompleted() >= 1) 
				{
					optionsF.Add(GetNumberSuffix(LoadLevel.GetTotalAttempts()) + " Total Attempt");
					optionsF.Add(LoadLevel.GetTotalAttempts().ToString() + " Total Attempts");
				}
				
				// if (Vector2.Distance(playerGO.transform.position, endPointGO.transform.position) < 1)
				// if (Vector2.Distance(playerLastPos, endPointLastPos) < 1)
				if (controller.closeCall == true)
				{
					optionsF.Add("So Close!");
					optionsF.Add("You Nearly Had It!");
					optionsF.Add("AARRGGHH!");
					optionsF.Add("WHY!");
					optionsF.Add("Bloody Idiot!");
					optionsF.Add("That's Disappointing!");
				}
				
				optionsF.Add("Double Press Direction To Boost");
				
				return optionsF[Random.Range(0, optionsF.Count)];
				// string[] test = new string[]{"A", "B"};
				// return test[Random.Range(0, test.Length)];
			break;
		}
		
		return "OH NO SOMETHING HAS GONE WRONG?";
	}
	
	public void UpdateText () {
		
		string newText = ChooseText();
		
		while (lastText == newText)
		{
			newText = ChooseText();
		}
		
		displayText.text = newText;
		lastText = newText;
	}
	
	string GetNumberSuffix (int number) {
		
		string suffix;
		
		int ones = number % 10;
		int tens = (int)Mathf.Floor(number / 10) % 10;

		if (tens == 1)
		{
			suffix = "th";
		}
		else
		{
			switch (ones)
			{
				case 1:
					suffix = "st";
					break;

				case 2:
					suffix = "nd";
					break;

				case 3:
					suffix = "rd";
					break;

				default:
					suffix = "th";
					break;
			}
		}
		
		return number.ToString() + suffix;
	}
}
