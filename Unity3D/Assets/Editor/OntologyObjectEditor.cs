using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(OntologyObject))]
public class OntologyObjectEditor : Editor {

	/// <summary>
	/// Raises the inspector GU event.
	/// </summary>
	public override void OnInspectorGUI() 
	{
		// Get my target
		OntologyObject myTarget = (OntologyObject)target;
		// Draws the default inspector
		// TODO: Fix this to be something else (we don't want to show everything probably)
		//DrawDefaultInspector ();

		// Draw the labels
		EditorGUILayout.LabelField("Name", myTarget.Name);
		EditorGUILayout.LabelField("Ask Hash", myTarget.AskHash);
		EditorGUILayout.LabelField("Gain Hash", myTarget.GainHash);
		EditorGUILayout.LabelField("Lose Hash", myTarget.LoseHash);

		
		
	}
}
