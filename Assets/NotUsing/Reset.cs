using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class Reset : MonoBehaviour {
	
	public GameObject resetMenu;
	public GameObject playButton;
	public GameObject resetButton;
	public GameObject noResetButton;
	
	void Awake () {
		resetMenu.SetActive(false);
	}
	
	public void LoadResetMenu () {
		resetMenu.SetActive(true);
		EventSystem.current.SetSelectedGameObject(noResetButton);
	}
	
	public void CloseResetMenu () {
		resetMenu.SetActive(false);
		EventSystem.current.SetSelectedGameObject(resetButton);
	}
	
	public void ResetCurrentRun () {
		LoadLevel.Reset();
		resetMenu.SetActive(false);
		EventSystem.current.SetSelectedGameObject(playButton);
	}
}
