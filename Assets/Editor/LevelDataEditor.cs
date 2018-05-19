using UnityEngine;  
using UnityEditor;  
using UnityEditorInternal;

//http://va.lent.in/unity-make-your-lists-functional-with-reorderablelist/

[CustomEditor(typeof(LevelGroup))]
// [CustomEditor(typeof(DummyScript))]

public class LevelDataEditor : Editor {  
    private ReorderableList list;

    private void OnEnable() {
		list = new ReorderableList(serializedObject, 
                serializedObject.FindProperty("levels"), 
                true, true, true, true);
				
		list.drawElementCallback =  
			(Rect rect, int index, bool isActive, bool isFocused) => {
			var element = list.serializedProperty.GetArrayElementAtIndex(index);
			rect.y += 2;
			EditorGUI.LabelField(
				new Rect(rect.x, rect.y, rect.width * 0.3f, EditorGUIUtility.singleLineHeight),
				"Level " + index.ToString("000"));
			EditorGUI.PropertyField(
				new Rect(rect.x + rect.width * 0.32f, rect.y, rect.width * 0.68f, EditorGUIUtility.singleLineHeight),
				element, GUIContent.none);
			
				// if using custom classes / structs / whatever, can use below code with the name of the variable and it'll work it out!
				// element.FindPropertyRelative("Type")
				
			EditorGUI.DrawPreviewTexture(
				new Rect(rect.x + rect.width * 0.32f, rect.y, EditorGUIUtility.singleLineHeight, EditorGUIUtility.singleLineHeight), 
				AssetPreview.GetAssetPreview(element.objectReferenceValue));
		};
		
		list.drawHeaderCallback = (Rect rect) => {  
			EditorGUI.LabelField(rect, "Levels");
		};
		
		// list.onSelectCallback = (ReorderableList l) => {  
			// var prefab = l.serializedProperty.GetArrayElementAtIndex(l.index).objectReferenceValue as GameObject;
			// if (prefab)
				// EditorGUIUtility.PingObject(prefab.gameObject);
		// };
    }

    public override void OnInspectorGUI() {
        serializedObject.Update();
		list.DoLayoutList();
        serializedObject.ApplyModifiedProperties();
		
		LevelGroup myScript = (LevelGroup)target;
        myScript.folderName = EditorGUILayout.TextField("Folder Name", myScript.folderName);
		if(GUILayout.Button("Sync List"))
        {
            myScript.SyncList();
        }
    }
}