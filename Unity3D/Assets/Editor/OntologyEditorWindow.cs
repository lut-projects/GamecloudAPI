using UnityEngine;
using UnityEditor;
using System.Collections;

public class OntologyEditorWindow : EditorWindow
{
	string myString = "Hello World";
	bool groupEnabled;
	bool myBool = true;
	float myFloat = 1.23f;

	/// <summary>
	/// Shows the window in Unity Editor.
	/// </summary>
	[MenuItem("Window/Ontology Editor")]
	public static void ShowWindow()
	{
		EditorWindow.GetWindow(typeof(OntologyEditorWindow));
	}

	/// <summary>
	/// Raises the GU event.
	/// </summary>
	void OnGUI()
	{
		// The window code
		GUILayout.Label("Base Settings", EditorStyles.boldLabel);
		myString = EditorGUILayout.TextField("Text Field", myString);

		// Layout group that can be toggled on and off
		groupEnabled = EditorGUILayout.BeginToggleGroup("Optional Settings", groupEnabled);
			myBool = EditorGUILayout.Toggle("Toggle", myBool);
			myFloat = EditorGUILayout.Slider("Slider", myFloat, -3, 3);
		EditorGUILayout.EndToggleGroup();
	}

}
