using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
 
public class CustomMenuButton : MonoBehaviour, ISelectHandler, IDeselectHandler, IPointerEnterHandler, IPointerExitHandler {
 
    private Text buttonText;
	 
	private Color selectedColor = new Color (1, 1, 1, 1);
	private Color deselectedColor = new Color (1, 1, 1, 0.8f);
	 
	private string defaultText;
	// private Color defaultColour;
 
    void Awake () {
        buttonText = GetComponentInChildren<Text>();
		 
		defaultText = buttonText.text;
		// defaultColour = buttonText.color;
		 
		buttonText.color = deselectedColor;
    }
	 
	void Select () {
		// buttonText.text = " " + defaultText;
        buttonText.text = defaultText.TrimStart(' ');
		
        buttonText.fontStyle = FontStyle.Bold;
        buttonText.color = selectedColor;
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
		EventSystem.current.SetSelectedGameObject(null);
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