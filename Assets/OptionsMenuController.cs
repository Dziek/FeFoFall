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
	
	// void Update () {
		// if (Input.GetButtonDown("Cancel"))
		// {
			// Debug.Log("");
		// }
	// }
	
	public void LoadOptionsMenu () {
		optionsMenu.menuScreenGO.SetActive(true);
		resetMenu.menuScreenGO.SetActive(false);
		
		// MainMenuController sets Options menu button to be selected, which is why we don't need to do it here
		
		if (selectedButtonGO != null)
		{
			EventSystem.current.SetSelectedGameObject(selectedButtonGO);
		}
	}
	
	public void LoadBoostMenu () {
		// optionsMenu.menuScreenGO.SetActive(false);
		// resetMenu.menuScreenGO.SetActive(true);
		
		// selectedButtonGO = EventSystem.current.currentSelectedGameObject;
		// EventSystem.current.SetSelectedGameObject(resetMenu.defaultSelectedGO);
	}
	
	public void LoadResetMenu () {
		optionsMenu.menuScreenGO.SetActive(false);
		resetMenu.menuScreenGO.SetActive(true);
		
		selectedButtonGO = EventSystem.current.currentSelectedGameObject;
		EventSystem.current.SetSelectedGameObject(resetMenu.defaultSelectedGO);
	}
	
	public void ResetGame () {
		// LoadLevel.Reset();
		// GameObject.Find("StatsManager").GetComponent<StatsManager>().ClearModeStats(Mode.Main);
		GameObject.Find("Levels").GetComponent<LevelManager>().ResetModeLevels();
		LoadOptionsMenu();
	}
}
