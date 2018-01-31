using UnityEngine;
using System.Collections;

public class Score : MonoBehaviour {

	// static int score = 0;
	// static int highScore;
	
	// static int gamesPlayed;
	// static int totalScore;
	
	// static GameplayText gameTextScript;
	
	// void Awake () {
		// // gameTextScript = transform.GetComponent<GameplayText>();
		
		// #if UNITY_STANDALONE || UNITY_EDITOR
			// highScore = PlayerPrefs.GetInt("HighScore", 0);
			
			// gamesPlayed = PlayerPrefs.GetInt("GamesPlayed", 0);
			// totalScore = PlayerPrefs.GetInt("TotalScore", 0);
		// #endif
		
		// #if UNITY_WEBGL && !UNITY_EDITOR
			// // Application.ExternalCall("GetStats");

		// #endif
		
		// gameTextScript.UpdateScoreText();
	// }
	
	// public static void ScoreUp (int points) {
		// score += points;
		
		// if (score > highScore)
		// {
			// highScore = score;
		// }
		
		// gameTextScript.UpdateScoreText();
	// }
	
	// public static int GetScore () {
		
		// return score;
	// }
	
	// public static int GetHighScore () {
		
		// return highScore;
	// }
	
	// public static int GetGamesPlayed () {
		
		// return gamesPlayed;
	// }
	
	// public static int GetTotalScore () {
		
		// return totalScore;
	// }
	
	// public static void SaveScore () {
		
		// totalScore += score;
		// gamesPlayed++;
		
		// #if UNITY_STANDALONE || UNITY_EDITOR
			// PlayerPrefs.SetInt("TotalScore", totalScore);
			// PlayerPrefs.SetInt("GamesPlayed", gamesPlayed);
			
			// if (score > PlayerPrefs.GetInt("HighScore",0))
			// // if (score >= highScore && score != 0)
			// {
				// PlayerPrefs.SetInt("HighScore", score);
			// }
		// #endif
		
		// #if UNITY_WEBGL && !UNITY_EDITOR
			// // Application.ExternalCall("UpdateOtherStats", totalScore, gamesPlayed);
			
			// // if (score >= highScore && score != 0)
			// // {
				// // Application.ExternalCall("UpdateHighScore", score);
			// // }
		// #endif
	// }
	
	// public static void ResetScore () {
		// score = 0;
		// gameTextScript.UpdateScoreText();
	// }
	
	// #if UNITY_WEBGL
	
		// // void UpdateHighScoreFromLocalStorage (string hS) {
			// // highScore = int.Parse(hS);
		// // }
		
		// // void UpdateTotalScoreFromLocalStorage (string tS) {
			// // totalScore = int.Parse(tS);
		// // }
		
		// // void UpdateGamesPlayedFromLocalStorage (string gP) {
			// // gamesPlayed = int.Parse(gP);
		// // }
	
	// #endif
}	
