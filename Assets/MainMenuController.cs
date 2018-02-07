using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MainMenuController : MonoBehaviour {

	public MenuScreen mainSubMenu;
	public MenuScreen optionsSubMenu;
	public MenuScreen statsSubMenu;
	public MenuScreen extrasSubMenu;
	
	void OnEnable () {
		LoadMainSub();
	}
	
	public void LoadMainSub () {
		mainSubMenu.menuScreenGO.SetActive(true);
		optionsSubMenu.menuScreenGO.SetActive(false);
		statsSubMenu.menuScreenGO.SetActive(false);
		extrasSubMenu.menuScreenGO.SetActive(false);
		
		EventSystem.current.SetSelectedGameObject(mainSubMenu.firstSelectedGO);
	}
	
	public void OpenOptionsSub () {
		mainSubMenu.menuScreenGO.SetActive(false);
		optionsSubMenu.menuScreenGO.SetActive(true);
		statsSubMenu.menuScreenGO.SetActive(false);
		extrasSubMenu.menuScreenGO.SetActive(false);
		
		EventSystem.current.SetSelectedGameObject(optionsSubMenu.firstSelectedGO);
	}
	
	public void OpenStatsSub () {
		mainSubMenu.menuScreenGO.SetActive(false);
		optionsSubMenu.menuScreenGO.SetActive(false);
		statsSubMenu.menuScreenGO.SetActive(true);
		extrasSubMenu.menuScreenGO.SetActive(false);
		
		EventSystem.current.SetSelectedGameObject(statsSubMenu.firstSelectedGO);
	}
	
	public void OpenExtrasSub () {
		mainSubMenu.menuScreenGO.SetActive(false);
		optionsSubMenu.menuScreenGO.SetActive(false);
		statsSubMenu.menuScreenGO.SetActive(false);
		extrasSubMenu.menuScreenGO.SetActive(true);
		
		EventSystem.current.SetSelectedGameObject(extrasSubMenu.firstSelectedGO);
	}
}

[System.Serializable]
	public struct MenuScreen {
		public GameObject menuScreenGO;
		public GameObject firstSelectedGO;
	} 
