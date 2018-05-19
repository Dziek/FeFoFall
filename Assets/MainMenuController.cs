using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MainMenuController : MonoBehaviour {

	public MenuScreen mainSubMenu;
	public MenuScreen optionsSubMenu;
	public MenuScreen statsSubMenu;
	public MenuScreen extrasSubMenu;
	
	// [HideInInspector]
	public GameObject selectedButtonGO;
	
	public GameObject onDisableButtonGO;
	[HideInInspector]
	public List<GameObject> previousSelectedGOs = new List<GameObject>();
	
	public GameObject currentButtonGO;
	
	void Update () {
		currentButtonGO = EventSystem.current.currentSelectedGameObject;
	}
	
	void OnEnable () {
		Invoke("D", 0.0f);
		
	}
	
	void D () {
		Debug.Log(EventSystem.current.alreadySelecting);
		
		if (onDisableButtonGO != null)
		{
			EventSystem.current.SetSelectedGameObject(onDisableButtonGO);
			// Debug.Log("Setting to: " + EventSystem.current.currentSelectedGameObject);
			Debug.Log("Setting to: " + onDisableButtonGO);
		}
	}
	
	void Start () {
		// LoadMainSub();
		Invoke("LoadMainSub", 0.0f);
	}
	
	void OnDisable () {
		// selectedButtonGO = null;
		// selectedButtonGO = EventSystem.current.currentSelectedGameObject;
		
		onDisableButtonGO = EventSystem.current.currentSelectedGameObject;
		// Debug.Log(EventSystem.current.currentSelectedGameObject);
		Debug.Log(onDisableButtonGO);
		
		EventSystem.current.SetSelectedGameObject(null);
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
		}else{
			EventSystem.current.SetSelectedGameObject(mainSubMenu.defaultSelectedGO);
		}
	}
	
	public void OpenOptionsSub () {
		mainSubMenu.menuScreenGO.SetActive(false);
		optionsSubMenu.menuScreenGO.SetActive(true);
		statsSubMenu.menuScreenGO.SetActive(false);
		extrasSubMenu.menuScreenGO.SetActive(false);
		
		selectedButtonGO = EventSystem.current.currentSelectedGameObject;
		EventSystem.current.SetSelectedGameObject(optionsSubMenu.defaultSelectedGO);
	}
	
	public void OpenStatsSub () {
		mainSubMenu.menuScreenGO.SetActive(false);
		optionsSubMenu.menuScreenGO.SetActive(false);
		statsSubMenu.menuScreenGO.SetActive(true);
		extrasSubMenu.menuScreenGO.SetActive(false);
		
		selectedButtonGO = EventSystem.current.currentSelectedGameObject;
		EventSystem.current.SetSelectedGameObject(statsSubMenu.defaultSelectedGO);
	}
	
	public void OpenExtrasSub () {
		mainSubMenu.menuScreenGO.SetActive(false);
		optionsSubMenu.menuScreenGO.SetActive(false);
		statsSubMenu.menuScreenGO.SetActive(false);
		extrasSubMenu.menuScreenGO.SetActive(true);
		
		selectedButtonGO = EventSystem.current.currentSelectedGameObject;
		EventSystem.current.SetSelectedGameObject(extrasSubMenu.defaultSelectedGO);
	}
}

[System.Serializable]
	public struct MenuScreen {
		public GameObject menuScreenGO;
		public GameObject defaultSelectedGO;
	} 
