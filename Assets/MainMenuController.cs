using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MainMenuController : MonoBehaviour {

	public MenuScreen mainSubMenu;
	public MenuScreen optionsSubMenu;
	public MenuScreen statsSubMenu;
	public MenuScreen extrasSubMenu;
	
	private GameObject selectedButtonGO;
	
	void OnEnable () {
		LoadMainSub();
	}
	
	void OnDisable () {
		selectedButtonGO = null;
	}
	
	public void LoadMainSub () {
		mainSubMenu.menuScreenGO.SetActive(true);
		optionsSubMenu.menuScreenGO.SetActive(false);
		statsSubMenu.menuScreenGO.SetActive(false);
		extrasSubMenu.menuScreenGO.SetActive(false);
		
		// MenuManager always makes the Play Button be selected on MainMenu load anyway
		// What this does, is make it so when you leave Options, you're still on Options
		
		if (selectedButtonGO != null)
		{
			EventSystem.current.SetSelectedGameObject(selectedButtonGO);
		}
	}
	
	public void OpenOptionsSub () {
		mainSubMenu.menuScreenGO.SetActive(false);
		optionsSubMenu.menuScreenGO.SetActive(true);
		statsSubMenu.menuScreenGO.SetActive(false);
		extrasSubMenu.menuScreenGO.SetActive(false);
		
		selectedButtonGO = EventSystem.current.currentSelectedGameObject;
		EventSystem.current.SetSelectedGameObject(optionsSubMenu.firstSelectedGO);
	}
	
	public void OpenStatsSub () {
		mainSubMenu.menuScreenGO.SetActive(false);
		optionsSubMenu.menuScreenGO.SetActive(false);
		statsSubMenu.menuScreenGO.SetActive(true);
		extrasSubMenu.menuScreenGO.SetActive(false);
		
		selectedButtonGO = EventSystem.current.currentSelectedGameObject;
		EventSystem.current.SetSelectedGameObject(statsSubMenu.firstSelectedGO);
	}
	
	public void OpenExtrasSub () {
		mainSubMenu.menuScreenGO.SetActive(false);
		optionsSubMenu.menuScreenGO.SetActive(false);
		statsSubMenu.menuScreenGO.SetActive(false);
		extrasSubMenu.menuScreenGO.SetActive(true);
		
		selectedButtonGO = EventSystem.current.currentSelectedGameObject;
		EventSystem.current.SetSelectedGameObject(extrasSubMenu.firstSelectedGO);
	}
}

[System.Serializable]
	public struct MenuScreen {
		public GameObject menuScreenGO;
		public GameObject firstSelectedGO;
	} 
