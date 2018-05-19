using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
 
public class CustomMenuButton : MonoBehaviour, ISelectHandler, IDeselectHandler, IPointerEnterHandler, IPointerExitHandler {
 
    private Text buttonText;
	 
	private Color selectedColor = new Color (1, 1, 1, 1);
	private Color deselectedColor = new Color (1, 1, 1, 0.8f);
	 
	private string defaultText;
	// private Color defaultColour;
	
	// private MainMenuController mainMenuController;
 
    void Awake () {
        buttonText = GetComponentInChildren<Text>();
		
		defaultText = buttonText.text;
		// defaultColour = buttonText.color;
		 
		buttonText.color = deselectedColor;
		
		// mainMenuController = GameObject.Find("MainMenu").GetComponent<MainMenuController>();
		GetComponent<Button>().onClick.AddListener(OnClick);
    }
	 
	void Select () {
		// buttonText.text = " " + defaultText;
        buttonText.text = defaultText.TrimStart(' ');
		
        buttonText.fontStyle = FontStyle.Bold;
        buttonText.color = selectedColor;
		
		// mainMenuController.selectedButtonGO = this.gameObject;
		
		// Debug.Log("Selected");
	}
	
	void Deselect () {
		buttonText.text = defaultText;
		buttonText.fontStyle = FontStyle.Normal;
        buttonText.color = deselectedColor;
	}
	
	void OnDisable () {
		Deselect();
	}
	
	public void ClearSelected () {
		// EventSystem.current.SetSelectedGameObject(null);
	}
	
	void OnClick () {
		// mainMenuController.previousSelectedGOs.Add(this.gameObject);
		// EventSystem.current.SetSelectedGameObject(null);
	}
 
    public void OnSelect (BaseEventData eventData) {
		Select();
    }
 
    public void OnDeselect (BaseEventData eventData) {
        Deselect();
    }
	
	public void OnPointerEnter (PointerEventData eventData) {
		Select();
	}
	
	public void OnPointerExit (PointerEventData eventData) {
        Deselect();
    }
}