using UnityEngine;
using UnityEditor;
using System.Collections;
using System;
using System.Reflection;

[CustomEditor(typeof(OntologyObject))]
public class OntologyObjectEditor : Editor {

	// Holders for the method informations
	public string[] methods;
	private int _selectedMethod = 0;

	// Flags to show the members of this instance and their declared public members
	const BindingFlags flags = BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly;

	// Placeholder for checking if the gameObject has changed
	private GameObject tempObject = null;

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

		DefineAskCallbackView (myTarget);

		DefineCallbackDropdown (myTarget);

		
	}

	/// <summary>
	/// Defines the callback dropdown selector.
	/// </summary>
	/// <param name="myTarget">My target.</param>
	void DefineCallbackDropdown (OntologyObject myTarget)
	{
		// If the methods are still null
		if (this.methods == null) {
			Debug.Log("Methods are null");
			// Well, we need to do the Extraction of public methods again
			PopulateMethodsInformation (myTarget);
		}
		// Otherwise, start selecting
		else {
			if (this._selectedMethod > this.methods.Length) {
				this._selectedMethod = 0;
			}
			// Next, create the selector for the function
			this._selectedMethod = EditorGUILayout.Popup ("Callback method", this._selectedMethod, this.methods);
		}
	}

	/// <summary>
	/// Defines the ASK Callback view
	/// </summary>
	/// <param name="myTarget">My target.</param>
	void DefineAskCallbackView (OntologyObject myTarget)
	{
		tempObject = (GameObject)EditorGUILayout.ObjectField ("Game Object", myTarget.gameObject, typeof(GameObject), true);

		ExtractPublicMethods (myTarget);
	}

	/// <summary>
	/// Extracts the public methods from the attached Game Object.
	/// </summary>
	/// <param name="myTarget">My target.</param>
	void ExtractPublicMethods (OntologyObject myTarget)
	{
		// If the gameobject is not null and different than the existing tempObject
		if ((tempObject != null) && (tempObject != myTarget.gameObject)) 
		{
			PopulateMethodsInformation (myTarget);
		}
	}

	/// <summary>
	/// Populates the methods information.
	/// </summary>
	/// <param name="myTarget">My target.</param>
	void PopulateMethodsInformation (OntologyObject myTarget)
	{
		// Just check once more, that the gameObject is not null
		if (myTarget.gameObject != null)
		{
			// Switch the gameobject to the temp one
			myTarget.gameObject = tempObject;
			// Then, display all the functions the object has
			foreach (Component component in myTarget.gameObject.GetComponents<MonoBehaviour> ()) 
			{
				// Get the count of methods
				this.methods = new string[component.GetType ().GetMethods (flags).Length];
				int i = 0;
				// Get all the methods
				foreach (MethodInfo method in component.GetType ().GetMethods (flags)) 
				{
					Debug.Log (method.Name);
					// Put the value to the list
					this.methods [i] = method.Name;
					// And increment the i
					i++;
				}
			}
		}
	}
}
