using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class OptionsMenuController : MonoBehaviour {
	
	public MenuScreen optionsMenu;
	public MenuScreen resetMenu;
	
	void OnEnable () {
		LoadOptionsMenu();
	}
	
	public void LoadOptionsMenu () {
		optionsMenu.menuScreenGO.SetActive(true);
		resetMenu.menuScreenGO.SetActive(false);
		
		EventSystem.current.SetSelectedGameObject(optionsMenu.firstSelectedGO);
	}
	
	public void LoadResetMenu () {
		optionsMenu.menuScreenGO.SetActive(false);
		resetMenu.menuScreenGO.SetActive(true);
		
		EventSystem.current.SetSelectedGameObject(resetMenu.firstSelectedGO);
	}
	
	public void ResetGame () {
		LoadLevel.Reset();
		LoadOptionsMenu();
	}
}
