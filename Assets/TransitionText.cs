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
	private StatsManager statsManager;
	private ModeManager modeManager;
	
	void Awake () {
		controller = GetComponent<TransitionController>();
		statsManager = GameObject.Find("StatsManager").GetComponent<StatsManager>();
		modeManager = GameObject.Find("ModeManager").GetComponent<ModeManager>();
	}
	
	public string ChooseText () {
		
		// Debug.Log("Choosing Text");
		
		if (controller.gameCompleted == true)
		{
			return completeText;
		}
		
		// GameObject levelGO = GameObject.FindGameObjectWithTag("Level");
		// if (levelGO != null)
		// {
			// LevelText levelText = levelGO.GetComponent<LevelText>();
			// if (levelText != null)
			// {
				// Debug.Log("Woop");
			// }
		// }
		
		switch (controller.transitionReason)
		{
			case TransitionReason.levelLoad:
				
				List<string> optionsL = new List<string>(loadTextOptions);
				
				if (controller.gameReset == true)
				{
					optionsL.Clear();
					
					optionsL.Add("Another Go?");
					optionsL.Add("Clearing Data");
					optionsL.Add("Doing A Reset");
					optionsL.Add("Back For More?");
					optionsL.Add("Do Better This Time");
					optionsL.Add("Cleaning Up");
					
					return optionsL[Random.Range(0, optionsL.Count)];
				}
			
				return optionsL[Random.Range(0, optionsL.Count)];
			// break;
			
			case TransitionReason.levelSuccess:	
				// Check for Unique Level Text
				// GameObject levelGO = GameObject.FindGameObjectWithTag("Level");
				// if (levelGO != null)
				// {
				LevelText levelText = GameObject.FindGameObjectWithTag("Level").GetComponent<LevelText>();
				if (levelText != null)
				{
					int r = Random.Range(0, 10); // might balance this out based on times attempted / number of options / start at 2 add 1 for each option
					// bool doThis = r < 8 ? true : false;
					bool doThis = false;
					
					List<string> uniqueLevelOptions = new List<string>();
					
					// LevelText levelText = levelGO.GetComponent<LevelText>();
					// if (levelText != null)
					// {
						if (levelText.bonusSuccessText.Length > 0)
						{
							uniqueLevelOptions = new List<string>(levelText.bonusSuccessText);
							// doThis = r < (7 + uniqueLevelOptions.Count) ? true : false;
							doThis = r < 8 ? true : false;
						}
					// }
					
					// if (doThis && uniqueLevelOptions.Count > 0) return uniqueLevelOptions[Random.Range(0, uniqueLevelOptions.Count)];
					if (doThis) return uniqueLevelOptions[Random.Range(0, uniqueLevelOptions.Count)];
				}
				
				// Start of normal text options
				List<string> options = new List<string>(successTextOptions);
				
				//basic ones - TODO: Move others here
				// options.Add("...");
				
				int currentLevelAttempts = statsManager.GetCurrentLevelCurrentAttempts();
				
				if (currentLevelAttempts == 1)
				{
					float r = Random.Range(0.0f, 1f) - statsManager.GetPercentageComplete(modeManager.GetMode());
					bool doThis = r < 0.02f ? true : false;
					
					if (doThis) options.Clear();
					
					options.Add("First Time Lucky!");
					options.Add("Beginner's Luck");
					options.Add("Hole In One!");
					options.Add("We Got Us A Natural");
					options.Add("Straight To It");
					options.Add("No Messing About");
					options.Add("Ooft");
					
					if (doThis) return options[Random.Range(0, options.Count)];
				}
				
				if (currentLevelAttempts >= 3)
				{
					options.Add(GetNumberSuffix(currentLevelAttempts) + " Time Lucky");
					options.Add("Whatever");
					options.Add("Practice Makes Perfect");
					options.Add("That's More Like It");
				}
				
				if (currentLevelAttempts >= 10)
				{
					options.Add("Finally");
					options.Add("About Time");
					options.Add("Yawn");
					
					options.Add("Proud Of You");
					options.Add("Knew You Could Do It");
				}
				
				switch (statsManager.GetPercentageComplete(modeManager.GetMode()).ToString()) {
					case "0.25":
						// options.Add("Quarter Done!");
						return "Quarter Done!";
					break;
					
					case "0.5":
						// options.Add("Halfway There!");
						return "Halfway There!";
					break;
					
					case "0.75":
						// options.Add("Three Quarters Done!");
						return "Three Quarters Done!";
					break;
					
					default:
						options.Add((statsManager.GetPercentageComplete(modeManager.GetMode()) * 100).ToString("f2") + "% Done!");
					break;
				}
				
				if (statsManager.GetPercentageComplete(modeManager.GetMode()) > 0.1f && statsManager.GetPercentageComplete(modeManager.GetMode()) < 0.2f)
				{
					options.Add("Only Gets Better From Here");
				}
				
				if (statsManager.GetPercentageComplete(modeManager.GetMode()) > 0.45f && statsManager.GetPercentageComplete(modeManager.GetMode()) < 0.5f)
				{
					options.Add("Nearly Halfway There!");
				}
				
				if (statsManager.GetPercentageComplete(modeManager.GetMode()) > 0.92f && statsManager.GetPercentageComplete(modeManager.GetMode()) < 0.98f)
				{
					options.Add("Not Long To Go Now");
					options.Add("Just A Few More");
					options.Add("Nearly There");
				}
				
				if (statsManager.GetPercentageComplete(modeManager.GetMode()) < 0.1f)
				{			
					options.Add("Just Getting Started");
					options.Add("Early Victory");
					
					options.Add("Pfft, That Was Easy");
				}
				
				if (statsManager.GetPercentageComplete(modeManager.GetMode()) > 0.5f)
				{
					options.Add("Way To Go Slugger");
					options.Add("CHAMPION");
					options.Add("A+ Performance There");
					options.Add("Textbook");
					options.Add("Back Of The Net");
					options.Add("GOOOOOOOAAAAAAL");
					options.Add("BRING IT ON");
					
					options.Add("Feeling It Yet?");
					options.Add("As If");
					options.Add("I Don't Believe It");
				}
				
				if (statsManager.GetPercentageComplete(modeManager.GetMode()) > 0.75f)
				{
					options.Add("Getting There");
					options.Add(":D");
				}
				
				// if (statsManager.GetPercentageComplete(modeManager.GetMode()) > 0.85f)
				// {
					// if (statsManager.GetCurrentAttempts(modeManager.GetMode()) < statsManager.GetBestAttempts(modeManager.GetMode()) 
						// && statsManager.GetCurrentAttempts(modeManager.GetMode()) - statsManager.GetBestAttempts(modeManager.GetMode()) >= statsManager.GetNumberOfLevelsRemaining(modeManager.GetMode()))
					// {
						// options.Add("Can Still Beat Your Attempts Record!");
					// }
					
					// if (statsManager.GetCurrentSeconds() < statsManager.GetBestSeconds())
					// {
						// options.Add("Can Still Beat Your Time Record!");
					// }
				// }
				
				switch (statsManager.GetNumberOfLevelsRemaining(modeManager.GetMode())) {
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
						options.Add("Just " + statsManager.GetNumberOfLevelsRemaining(modeManager.GetMode()).ToString() + " Levels To Go");
						options.Add("Only " + statsManager.GetNumberOfLevelsRemaining(modeManager.GetMode()).ToString() + " Levels Remaining");
						options.Add("Now Do That " + statsManager.GetNumberOfLevelsRemaining(modeManager.GetMode()).ToString() + " More Times");
					break;
				}
				
				if (statsManager.GetCurrentStreak() > 9)
				{
					float r = Random.Range(0.0f, 1f) - statsManager.GetPercentageComplete(modeManager.GetMode());
					bool doThis = r < 0.02f ? true : false;
					
					if (doThis) options.Clear();
					
					options.Add("You Got The Golden Touch");
					options.Add("Wowza!");
					options.Add("Gee Wizz");
					options.Add("Now This Is Impressive");
					
					if (doThis) return options[Random.Range(0, options.Count)];
				}
				
				if (statsManager.GetCurrentStreak() > 4)
				{
					float r = Random.Range(0.0f, 1f) - statsManager.GetPercentageComplete(modeManager.GetMode());
					bool doThis = r < 0.02f ? true : false;
					
					if (doThis) options.Clear();
					
					options.Add("Hot Streak!");
					options.Add("Keep 'em Coming");
					options.Add("Hanging Out In The Zone");
					options.Add("Oooh Baby");
					options.Add("You've Seen The Light");
					options.Add("Surely You Can't Keep This Up");
					options.Add("Show Off");
					
					options.Add(statsManager.GetCurrentStreak().ToString() + " Done And No Signs Of Slowing");
					options.Add(statsManager.GetCurrentStreak().ToString() + " In A Row!");
					
					// if (LoadLevel.GetBestGoodStreak() == LoadLevel.GetCurrentGoodStreak())
					// {
						// options.Add("Best Streak Yet!");
					// }
					
					if (doThis) return options[Random.Range(0, options.Count)];
				}
				
				if (statsManager.GetStreakBreak() == true)
				{
					// float r = Random.Range(0.0f, 1f) - statsManager.GetPercentageComplete(modeManager.GetMode());
					float r = Random.Range(0.0f, 1f);
					// bool doThis = r < 0.02f ? true : false;
					bool doThis = r < 0.8f ? true : false;
					
					if (doThis) options.Clear();
					
					options.Add("That's The Spirit");
					options.Add("About Time");
					options.Add("Finally");
					options.Add("Bad Streak Broken");
					options.Add("But Can You Keep It Going?");
					options.Add("Ruin My Fun");
					options.Add("That Was Some Losing Streak");
					options.Add("Glad You're Doing Better");
					options.Add("I Knew You Could Do One Of Them");
					
					if (doThis) return options[Random.Range(0, options.Count)];
				}
				
				// Debug.Log("Best Time: " + LoadLevel.GetCurrentLevelBestTime() + " Last Time: " + LoadLevel.GetLastSeconds()
				// + " Last Best Time: " + LoadLevel.GetLastBestTime());
				
				// if (LoadLevel.GetCurrentLevelBestTime() == LoadLevel.GetLastSeconds() && LoadLevel.GetLastBestTime() != 0)
				// {
					// options.Add("Best Time!");
					// options.Add("New Record!");
					// options.Add("Better Than Last Time!");
					// options.Add(Mathf.Abs(LoadLevel.GetLastTimeDifference()).ToString("f2") + " Seconds Faster!");
					
					// // return Mathf.Abs(LoadLevel.GetLastTimeDifference()).ToString("f2") + " Seconds Faster!";
				// }
				
				// if (LoadLevel.GetCurrentLevelBestTime() < LoadLevel.GetLastSeconds())
				// {
					// options.Add("Not Your Best Time");
					// options.Add(Mathf.Abs(LoadLevel.GetLastTimeDifference()).ToString("f2") + " Seconds Slower");
					
					// // return Mathf.Abs(LoadLevel.GetLastTimeDifference()).ToString("f2") + " Seconds Slower";
				// }
				
				// if (LoadLevel.GetCurrentLevelBestAttempts() == LoadLevel.GetCurrentLevelCurrentAttempts() && LoadLevel.GetBestAttemptsLast() != 0)
				// {
					// options.Add("Best Effort!");
					// options.Add("New Record!");
					// options.Add("Better Than Last Time!");
					// options.Add(Mathf.Abs(LoadLevel.GetLastAttemptsDifference()).ToString() + " Attempts Better!");
					
					// // return Mathf.Abs(LoadLevel.GetLastAttemptsDifference()).ToString() + " Attempts Better!";
				// }
				
				// if (LoadLevel.GetCurrentLevelBestAttempts() < LoadLevel.GetCurrentLevelCurrentAttempts())
				// {
					// options.Add("Not Your Best Effort");
					// options.Add("Took " + LoadLevel.GetLastAttemptsDifference() + " More Attempts");
				// }
					
				if (statsManager.GetLevelsCompleted(modeManager.GetMode()) > 1)
				{			
					options.Add(statsManager.GetLevelsCompleted(modeManager.GetMode()).ToString() + " Levels Completed");
				}
				
				options.Add(statsManager.GetLevelsCompleted(modeManager.GetMode()).ToString() + " Down, " + statsManager.GetNumberOfLevelsRemaining(modeManager.GetMode()).ToString() + " To Go");
				options.Add(statsManager.GetLevelsCompleted(modeManager.GetMode()).ToString() + " / " + statsManager.GetNumberOfLevels(modeManager.GetMode()).ToString());
				
				if (statsManager.GetLastSeconds() > 10)
				{
					options.Add("Took Your Time");
					options.Add("Slowpoke");
					options.Add("Well That Wasn't Very Fast");
				}
				
				options.Add("In Only " + statsManager.GetLastSeconds().ToString("f2") + " Seconds");
				
				options.Add("Your " + GetNumberSuffix(statsManager.GetLevelsCompleted(modeManager.GetMode())) + " Success");
				
				#if UNITY_WSA 
					options.Add("Press B To Return To Menu");
				#endif
				
				#if UNITY_STANDALONE
					options.Add("Press Escape To Return To Menu");
				#endif
				
				return options[Random.Range(0, options.Count)];
			break;
			
			case TransitionReason.levelFailure:
				// Check for Unique Level Text
				// GameObject levelGO = GameObject.FindGameObjectWithTag("Level");
				// if (levelGO != null)
				// {
				LevelText levelTextF = GameObject.FindGameObjectWithTag("Level").GetComponent<LevelText>();
				if (levelTextF != null)
				{
					int r = Random.Range(0, 10);
					// bool doThis = r < 8 ? true : false;
					bool doThis = false;
					
					List<string> uniqueLevelOptions = new List<string>();
					
					// LevelText levelText = levelGO.GetComponent<LevelText>();
					// if (levelText != null)
					// {
						if (levelTextF.bonusFailureText.Length > 0)
						{
							uniqueLevelOptions = new List<string>(levelTextF.bonusFailureText);
							doThis = r < (2 + uniqueLevelOptions.Count) ? true : false;
						}
					// }
					
					// if (doThis && uniqueLevelOptions.Count > 0) return uniqueLevelOptions[Random.Range(0, uniqueLevelOptions.Count)];
					if (doThis) return uniqueLevelOptions[Random.Range(0, uniqueLevelOptions.Count)];
					// return uniqueLevelOptions[Random.Range(0, uniqueLevelOptions.Count)];
				}
			
				List<string> optionsF = new List<string>(failureTextOptions);
				
				// basics
				optionsF.Add("...");
				
				if (statsManager.GetCurrentLevelCurrentAttempts() > 1)
				{
					optionsF.Add("Again?");
					optionsF.Add("History Repeats Itself");
				}
				
				if (statsManager.GetCurrentLevelCurrentAttempts() > 4)
				{
					optionsF.Add(statsManager.GetCurrentLevelCurrentAttempts().ToString() + " Tries Already?");
					optionsF.Add("Level Attempts: " + statsManager.GetCurrentLevelCurrentAttempts().ToString());
					
					optionsF.Add("Tricky isn't it?");
					optionsF.Add("Struggling?");
					optionsF.Add("You're Having Trouble With <i>This</i> One?");
					optionsF.Add("This Is An Easy One!");
				}
				
				if (statsManager.GetCurrentLevelCurrentAttempts() > 6)
				{
					optionsF.Add("Ha");
					optionsF.Add(":(");
					optionsF.Add("NOT AGAIN");
				}
				
				if (statsManager.GetCurrentLevelCurrentAttempts() > 10)
				{
					optionsF.Add("I Admire Your Determination");
					optionsF.Add("Keep Going!");
					optionsF.Add("I Believe In You");
					optionsF.Add("I No Longer Believe In You");
					optionsF.Add("Give Up");
					optionsF.Add("This One Level Eh?");
					optionsF.Add("Giving You Real Trouble Isn't It");
					optionsF.Add("I Did It First Try");
					optionsF.Add("You're Doing It Wrong");
				}
				
				if (statsManager.GetCurrentAttempts(modeManager.GetMode()) > 20)
				{
					optionsF.Add("That's Your " + GetNumberSuffix(statsManager.GetCurrentAttempts(modeManager.GetMode())) + " Attempt");
					optionsF.Add(statsManager.GetCurrentAttempts(modeManager.GetMode()).ToString() + " Attempts So Far");
					optionsF.Add("That Was Attempt " + statsManager.GetCurrentAttempts(modeManager.GetMode()).ToString());
				}
				
				if (statsManager.GetCurrentAttempts(modeManager.GetMode()) > 700)
				{
					optionsF.Add("This Isn't Going Great");
					optionsF.Add("Not Going For A Highscore Eh?");
					optionsF.Add("Probably Won't Be #1");
				}
				
				// if (LoadLevel.GetTimesStarted() >= 1) 
				// {
					// optionsF.Add("That's Your " + GetNumberSuffix(LoadLevel.GetTotalAttempts()) + " Total Attempt");
					// optionsF.Add("At " + LoadLevel.GetTotalSeconds().ToString("f2") + " Seconds Across All Games");
				// }
				
				// if (LoadLevel.GetTimesCompleted() >= 1) 
				// {
					// optionsF.Add(LoadLevel.GetTimesCompleted().ToString() );
				// }
				
				if (statsManager.GetPercentageComplete(modeManager.GetMode()) > 0.5f)
				{
					optionsF.Add("Thought You Could Handle It?");
					optionsF.Add("Bad");
					optionsF.Add("Made A Mistake There");
					optionsF.Add("Classic");
					optionsF.Add("Oh Boo Hoo");
					optionsF.Add("Miss The Easy Levels?");
					optionsF.Add("It Doesn't Get Better");
					optionsF.Add("Shameful");
					optionsF.Add("What Happened There?");
					optionsF.Add("I Wouldn't Have Done That");
					optionsF.Add("One More Try");
					optionsF.Add("C'mon");
					optionsF.Add("Ouch");
					optionsF.Add("Rookie Mistake");
					optionsF.Add("Biffed It");
				}
				
				if (controller.closeCall == true)
				{
					int r = Random.Range(0, 4);
					bool doThis = r == 0 ? true : false;
					
					if (doThis) optionsF.Clear();
					
					optionsF.Add("So Close!");
					optionsF.Add("You Nearly Had It!");
					optionsF.Add("AARRGGHH!");
					optionsF.Add("AAAAHHHHH!");
					optionsF.Add("WHY!");
					optionsF.Add("That's Disappointing!");
					
					if (doThis) return optionsF[Random.Range(0, optionsF.Count)];
				}
				
				// minus because bad streak is negative, might change
				if (statsManager.GetCurrentStreak() < -9)
				{
					float r = Random.Range(0.0f, 1f) - statsManager.GetPercentageComplete(modeManager.GetMode());
					bool doThis = r < 0.02f ? true : false;
					
					if (doThis) optionsF.Clear();
					
					optionsF.Add("Not Doing So Hot");
					optionsF.Add("Doing Real Bad Boss");
					optionsF.Add("Failure After Failure");
					optionsF.Add("Consistently Awful");
					optionsF.Add("Turn It Around");
					optionsF.Add("Pattern Of Defeat");
					optionsF.Add("Can You Break The Cycle?");
					optionsF.Add("Surely You Can Do One Of These?");
					
					optionsF.Add(Mathf.Abs(statsManager.GetCurrentStreak()).ToString() + " Failures And Counting");
					optionsF.Add(Mathf.Abs(statsManager.GetCurrentStreak()).ToString() + " Failures In A Row!");
					
					// if (statsManager.GetBestBadStreak() == statsManager.GetCurrentBadStreak())
					// {
						// optionsF.Add("Worst Streak Yet!");
						// optionsF.Add("Hopeless!");
					// }
					
					if (doThis) return optionsF[Random.Range(0, optionsF.Count)];
				}
				
				if (statsManager.GetStreakBreak() == true)
				{
					optionsF.Add("Couldn't Last Forever");
					optionsF.Add("You Had A Good Run");
					optionsF.Add("Win Some Lose Some");
					optionsF.Add("You Deserved That");
					optionsF.Add("All Good Things End");
					optionsF.Add("Rightly So");
					optionsF.Add("About Time");
				}
				
				// if (LoadLevel.GetCurrentLevelBestAttempts() < LoadLevel.GetCurrentLevelCurrentAttempts())
				// {
					// optionsF.Add("No Record For You");
					// optionsF.Add("Taken " + Mathf.Abs(LoadLevel.GetLastAttemptsDifference()).ToString() + " More Attempts Already");
				// }
				
				optionsF.Add(statsManager.GetLastSeconds().ToString("f2") + " Seconds Wasted");
				
				optionsF.Add(statsManager.GetCurrentSeconds(modeManager.GetMode()).ToString("f2") + " Seconds Of Practice...");
				optionsF.Add("Current Running Time: " + statsManager.GetCurrentSeconds(modeManager.GetMode()).ToString("f2") + " Seconds");
				optionsF.Add("At " + statsManager.GetCurrentSeconds(modeManager.GetMode()).ToString("f2") + " Seconds And Counting");
				
				if (statsManager.GetCurrentSeconds(modeManager.GetMode()) > 3600)
				{
					optionsF.Add("It's Been Over An Hour");
				}
				
				if (statsManager.GetCurrentLevelCurrentSeconds() > 5)
				{
					optionsF.Add("That's " + statsManager.GetCurrentLevelCurrentSeconds().ToString("f2") + " Seconds On This Level Alone");
				}
				
				// optionsF.Add("Double Press Direction To Boost");
				// optionsF.Add("Remember To Boost?");
				// optionsF.Add("Did You Boost?");
				
				optionsF.Add("Your " + GetNumberSuffix(statsManager.GetCurrentAttempts(modeManager.GetMode()) - statsManager.GetLevelsCompleted(modeManager.GetMode())) + " Failure");
				
				return optionsF[Random.Range(0, optionsF.Count)];
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
	
	// string[] GetUniqueLevelOptions (string s) {
		// GameObject levelGO = GameObject.FindGameObjectWithTag("Level");
		// if (levelGO != null)
		// {
			// LevelText levelText = levelGO.GetComponent<LevelText>();
			// if (levelText != null)
			// {
				// if (s == "Success")
				// {
					// return levelText.bonusSuccessText;
				// }
				
				// if (s == "Failure")
				// {
					// return levelText.bonusFailureText;
				// }
			// }
		// }
		
		// // string[] blankArray = new string[]{"MISTAKE MADE"};
		// // return blankArray;
		
		// return null;
	// }
	
	public string GetText () {
		return displayText.text;
	}
}
