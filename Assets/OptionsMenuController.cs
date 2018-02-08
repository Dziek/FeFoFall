using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class OptionsMenuController : MonoBehaviour {
	
	public MenuScreen optionsMenu;
	public MenuScreen resetMenu;
	
	private GameObject selectedButtonGO;
	
	void OnEnable () {
		LoadOptionsMenu();
	}
	
	void OnDisable () {
		selectedButtonGO = null;
	}
	
	public void LoadOptionsMenu () {
		optionsMenu.menuScreenGO.SetActive(true);
		resetMenu.menuScreenGO.SetActive(false);
		
		// MainMenuController sets Options menu button to be selected, which is why we don't need to do it here
		
		if (selectedButtonGO != null)
		{
			EventSystem.current.SetSelectedGameObject(selectedButtonGO);
		}
	}
	
	public void LoadResetMenu () {
		optionsMenu.menuScreenGO.SetActive(false);
		resetMenu.menuScreenGO.SetActive(true);
		
		selectedButtonGO = EventSystem.current.currentSelectedGameObject;
		EventSystem.current.SetSelectedGameObject(resetMenu.firstSelectedGO);
	}
	
	public void ResetGame () {
		LoadLevel.Reset();
		LoadOptionsMenu();
	}
}
