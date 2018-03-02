using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingController : MonoBehaviour {
	
	public Text loadingText;
	
	// Use this for initialization
	void Start () {
		
		StartCoroutine(LoadMain());
		
		// InvokeRepeating("FlashText", 1, 1);
	}
	
	void FlashText () {
		loadingText.enabled = !loadingText.enabled;
	}
	
	IEnumerator LoadMain ()
    {
        // The Application loads the Scene in the background at the same time as the current Scene.
        //This is particularly good for creating loading screens. You could also load the Scene by build //number.
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("MainScene");
		
		Color transparent = new Color32(255, 255, 255, 0);
		
		// int t = 0;
		
        //Wait until the last operation fully loads to return anything
        while (!asyncLoad.isDone)
        // while (t < 500)
        {
			float lerpValue = Mathf.PingPong(Time.time, 1f);
            loadingText.color = Color.Lerp(transparent, Color.white, lerpValue);
			
			// loadingText.text = "LOADING: " + asyncLoad.progress.ToString();
			
			// t++;
            yield return null;
        }
    }
}
