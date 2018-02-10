 using UnityEngine;
 using UnityEngine.EventSystems;
 using UnityEngine.UI;
 
 public class CustomMenuButton : MonoBehaviour, ISelectHandler, IDeselectHandler  {
 
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
 
     public void OnSelect (BaseEventData eventData)
     {
        // buttonText.text = " " + defaultText;
        buttonText.text = defaultText.TrimStart(' ');
		
        // buttonText.fontStyle = FontStyle.Bold;
        buttonText.color = selectedColor;
     }
 
     public void OnDeselect (BaseEventData eventData)
     {
        buttonText.text = defaultText;
		// buttonText.fontStyle = FontStyle.Normal;
        buttonText.color = deselectedColor;
     }
 }