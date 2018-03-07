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
					float r = Random.Range(0.0f, 1f) - LoadLevel.GetPercentageComplete();
					bool doThis = r < 0.02f ? true : false;
					
					if (doThis) options.Clear();
					
					options.Add("First Time Lucky!");
					options.Add("Beginner's Luck!");
					options.Add("Hole In One!");
					
					if (doThis) return options[Random.Range(0, options.Count)];
				}
				
				if (currentLevelAttempts >= 3)
				{
					options.Add(GetNumberSuffix(currentLevelAttempts) + " Time Lucky");
					options.Add("Whatever");
					options.Add("Practice Makes Perfect");
				}
				
				if (currentLevelAttempts >= 10)
				{
					options.Add("Finally");
					options.Add("Yawn");
					
					options.Add("Proud Of You");
					options.Add("Knew You Could Do It");
				}
				
				switch (LoadLevel.GetPercentageComplete().ToString()) {
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
						options.Add((LoadLevel.GetPercentageComplete() * 100).ToString("f2") + "% Done!");
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
				
				if (LoadLevel.GetPercentageComplete() < 0.1f)
				{			
					options.Add("Just Getting Started");
					options.Add("Hope You Got Snacks");
					options.Add("Early Victory");
					
					options.Add("Pfft, That Was Easy");
				}
				
				if (LoadLevel.GetPercentageComplete() > 0.5f)
				{
					options.Add("Way To Go Slugger");
					options.Add("CHAMPION");
					options.Add("A* Performance There");
					options.Add("Textbook");
					options.Add("Back Of The Net");
					options.Add("GOOOOOOOAAAAAAL");
					options.Add("BRING IT ON");
					
					options.Add("Feeling It Yet?");
					options.Add("As If");
					options.Add("I Don't Believe It");
				}
				
				if (LoadLevel.GetPercentageComplete() > 0.75f)
				{
					options.Add("Getting There");
					options.Add(":D");
				}
				
				if (LoadLevel.GetPercentageComplete() > 0.85f)
				{
					if (LoadLevel.GetCurrentAttempts() < LoadLevel.GetBestAttempts() 
						&& LoadLevel.GetCurrentAttempts() - LoadLevel.GetBestAttempts() >= LoadLevel.GetNumberOfLevelsRemaining())
					{
						options.Add("Can Still Beat Your Time Record!");
					}
					
					if (LoadLevel.GetCurrentSeconds() < LoadLevel.GetBestSeconds())
					{
						options.Add("Can Still Beat Your Attempts Record!");
					}
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
				
				if (LoadLevel.GetCurrentGoodStreak() > 9)
				{
					float r = Random.Range(0.0f, 1f) - LoadLevel.GetPercentageComplete();
					bool doThis = r < 0.02f ? true : false;
					
					if (doThis) options.Clear();
					
					options.Add("You Got The Golden Touch");
					options.Add("Wowza!");
					options.Add("Gee Wizz");
					options.Add("Now This Is Impressive");
					
					if (doThis) return options[Random.Range(0, options.Count)];
				}
				
				if (LoadLevel.GetCurrentGoodStreak() > 4)
				{
					float r = Random.Range(0.0f, 1f) - LoadLevel.GetPercentageComplete();
					bool doThis = r < 0.02f ? true : false;
					
					if (doThis) options.Clear();
					
					options.Add("Hot Streak!");
					options.Add("Keep 'em Coming");
					options.Add("Hanging Out In The Zone");
					options.Add("Oooh Baby");
					options.Add("You've Seen The Light");
					options.Add("Surely You Can't Keep This Up");
					options.Add("Show Off");
					
					options.Add(LoadLevel.GetCurrentGoodStreak().ToString() + " Done And No Signs Of Slowing");
					options.Add(LoadLevel.GetCurrentGoodStreak().ToString() + " In A Row!");
					
					if (LoadLevel.GetBestGoodStreak() == LoadLevel.GetCurrentGoodStreak())
					{
						options.Add("Best Streak Yet!");
					}
					
					if (doThis) return options[Random.Range(0, options.Count)];
				}
				
				if (LoadLevel.GetBadStreakBreak() == true)
				{
					float r = Random.Range(0.0f, 1f) - LoadLevel.GetPercentageComplete();
					bool doThis = r < 0.02f ? true : false;
					
					if (doThis) options.Clear();
					
					options.Add("That's The Spirit");
					options.Add("About Time");
					options.Add("Bad Streak Broken");
					options.Add("But Can You Keep It Going?");
					options.Add("Ruin My Fun");
					options.Add("That Was Some Losing Streak");
					options.Add("Glad You're Doing Better");
					options.Add("I Knew You Could Do One Of Them");
					
					if (doThis) return options[Random.Range(0, options.Count)];
				}
				
				Debug.Log("Best Time: " + LoadLevel.GetCurrentLevelBestTime() + " Last Time: " + LoadLevel.GetLastSeconds()
				+ " Last Best Time: " + LoadLevel.GetLastBestTime());
				
				if (LoadLevel.GetCurrentLevelBestTime() == LoadLevel.GetLastSeconds() && LoadLevel.GetLastBestTime() != 0)
				{
					options.Add("Best Time!");
					options.Add("New Record!");
					options.Add("Better Than Last Time!");
					options.Add(Mathf.Abs(LoadLevel.GetLastTimeDifference()).ToString("f2") + " Seconds Faster!");
					
					// return Mathf.Abs(LoadLevel.GetLastTimeDifference()).ToString("f2") + " Seconds Faster!";
				}
				
				if (LoadLevel.GetCurrentLevelBestTime() < LoadLevel.GetLastSeconds())
				{
					options.Add("Not Your Best Time");
					options.Add(Mathf.Abs(LoadLevel.GetLastTimeDifference()).ToString("f2") + " Seconds Slower");
					
					// return Mathf.Abs(LoadLevel.GetLastTimeDifference()).ToString("f2") + " Seconds Slower";
				}
				
				if (LoadLevel.GetCurrentLevelBestAttempts() == LoadLevel.GetCurrentLevelCurrentAttempts() && LoadLevel.GetBestAttemptsLast() != 0)
				{
					options.Add("Best Effort!");
					options.Add("New Record!");
					options.Add("Better Than Last Time!");
					options.Add(Mathf.Abs(LoadLevel.GetLastAttemptsDifference()).ToString() + " Attempts Better!");
					
					// return Mathf.Abs(LoadLevel.GetLastAttemptsDifference()).ToString() + " Attempts Better!";
				}
				
				if (LoadLevel.GetCurrentLevelBestAttempts() < LoadLevel.GetCurrentLevelCurrentAttempts())
				{
					options.Add("Not Your Best Effort");
					options.Add("Took " + LoadLevel.GetLastAttemptsDifference() + " More Attempts");
				}
					
				if (LoadLevel.GetLevelsCompleted() > 1)
				{			
					options.Add(LoadLevel.GetLevelsCompleted().ToString() + " Levels Completed");
				}
				
				options.Add(LoadLevel.GetLevelsCompleted().ToString() + " Down, " + LoadLevel.GetNumberOfLevelsRemaining().ToString() + " To Go");
				options.Add(LoadLevel.GetLevelsCompleted().ToString() + " / " + LoadLevel.GetNumberOfLevels().ToString());
				
				if (LoadLevel.GetLastSeconds() > 10)
				{
					options.Add("Took Your Time");
					options.Add("Slowpoke");
					options.Add("Well That Wasn't Very Fast");
				}
				
				options.Add("In Only " + LoadLevel.GetLastSeconds().ToString("f2") + " Seconds");
				
				options.Add("Your " + GetNumberSuffix(LoadLevel.GetLevelsCompleted()) + " Success");
				
				options.Add("Press B To Return To Menu");
				
				return options[Random.Range(0, options.Count)];
			break;
			
			case TransitionState.levelFailure:
				List<string> optionsF = new List<string>(failureTextOptions);
				
				if (LoadLevel.GetCurrentLevelCurrentAttempts() > 1)
				{
					optionsF.Add("Again?");
					optionsF.Add("History Repeats Itself");
				}
				
				if (LoadLevel.GetCurrentLevelCurrentAttempts() > 4)
				{
					optionsF.Add(LoadLevel.GetCurrentLevelCurrentAttempts().ToString() + " Tries Already?");
					optionsF.Add("Level Attempts: " + LoadLevel.GetCurrentLevelCurrentAttempts().ToString());
					
					optionsF.Add("Tricky isn't it?");
					optionsF.Add("Struggling?");
					optionsF.Add("You're Having Trouble With This One?");
					optionsF.Add("This Is An Easy One!");
				}
				
				if (LoadLevel.GetCurrentLevelCurrentAttempts() > 6)
				{
					optionsF.Add("Ha");
					optionsF.Add(":(");
					optionsF.Add("NOT AGAIN");
				}
				
				if (LoadLevel.GetCurrentLevelCurrentAttempts() > 10)
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
				
				if (LoadLevel.GetCurrentAttempts() > 20)
				{
					optionsF.Add("That's Your " + GetNumberSuffix(LoadLevel.GetCurrentAttempts()) + " Attempt");
					optionsF.Add(LoadLevel.GetCurrentAttempts().ToString() + " Attempts So Far");
					optionsF.Add("That Was Attempt " + LoadLevel.GetCurrentAttempts().ToString());
				}
				
				if (LoadLevel.GetCurrentAttempts() > 700)
				{
					optionsF.Add("This Isn't Going Great");
					optionsF.Add("Not Going For A Highscore Eh?");
					optionsF.Add("Probably Won't Be #1");
				}
				
				if (LoadLevel.GetTimesStarted() >= 1) 
				{
					optionsF.Add("That's Your " + GetNumberSuffix(LoadLevel.GetTotalAttempts()) + " Total Attempt");
					optionsF.Add("At " + LoadLevel.GetTotalSeconds().ToString("f2") + " Seconds Across All Games");
				}
				
				// if (LoadLevel.GetTimesCompleted() >= 1) 
				// {
					// optionsF.Add(LoadLevel.GetTimesCompleted().ToString() );
				// }
				
				if (LoadLevel.GetPercentageComplete() > 0.5f)
				{
					optionsF.Add("Thought You Could Handle It?");
					optionsF.Add("Bad");
					optionsF.Add("Made A Mistake There");
					optionsF.Add("Classic");
					optionsF.Add("Oh Boo Hoo");
					optionsF.Add("Miss The Easy Levels?");
					optionsF.Add("It Doesn't Get Better");
					optionsF.Add("Shameful");
					optionsF.Add("Coward");
					
					optionsF.Add("One More Try");
					optionsF.Add("C'mon");
				}
				
				// if (Vector2.Distance(playerGO.transform.position, endPointGO.transform.position) < 1)
				// if (Vector2.Distance(playerLastPos, endPointLastPos) < 1)
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
				
				if (LoadLevel.GetCurrentBadStreak() > 9)
				{
					float r = Random.Range(0.0f, 1f) - LoadLevel.GetPercentageComplete();
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
					
					optionsF.Add(LoadLevel.GetCurrentBadStreak().ToString() + " Failures And Counting");
					optionsF.Add(LoadLevel.GetCurrentBadStreak().ToString() + " Failures In A Row!");
					
					if (LoadLevel.GetBestBadStreak() == LoadLevel.GetCurrentBadStreak())
					{
						optionsF.Add("Worst Streak Yet!");
						optionsF.Add("Hopeless!");
					}
					
					if (doThis) return optionsF[Random.Range(0, optionsF.Count)];
				}
				
				if (LoadLevel.GetGoodStreakBreak() == true)
				{
					optionsF.Add("Couldn't Last Forever");
					optionsF.Add("You Had A Good Run");
					optionsF.Add("Win Some Lose Some");
					optionsF.Add("You Deserved That");
					optionsF.Add("All Good Things End");
					optionsF.Add("Rightly So");
					optionsF.Add("About Time");
					optionsF.Add("You Lose Your Streak On That One?");
				}
				
				if (LoadLevel.GetCurrentLevelBestAttempts() < LoadLevel.GetCurrentLevelCurrentAttempts())
				{
					optionsF.Add("No Record For You");
					optionsF.Add("Taken " + LoadLevel.GetLastAttemptsDifference().ToString("f2") + " More Attempts Already");
				}
				
				optionsF.Add(LoadLevel.GetLastSeconds().ToString("f2") + " Seconds Wasted");
				
				optionsF.Add(LoadLevel.GetCurrentSeconds().ToString("f2") + " Seconds Of Practice...");
				optionsF.Add("Current Running Time: " + LoadLevel.GetCurrentSeconds().ToString("f2") + " Seconds");
				optionsF.Add("At " + LoadLevel.GetCurrentSeconds().ToString("f2") + " Seconds And Counting");
				
				if (LoadLevel.GetCurrentSeconds() > 3600)
				{
					optionsF.Add("It's Been Over An Hour");
				}
				
				optionsF.Add("That's " + LoadLevel.GetCurrentLevelCurrentSeconds().ToString("f2") + " Seconds On This Level Alone");
				
				// optionsF.Add("Double Press Direction To Boost");
				optionsF.Add("Remember To Boost?");
				optionsF.Add("Did You Boost?");
				
				optionsF.Add("Your " + GetNumberSuffix(LoadLevel.GetCurrentAttempts() - LoadLevel.GetLevelsCompleted()) + " Failure");
				
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
	
	public string GetText () {
		return displayText.text;
	}
}
