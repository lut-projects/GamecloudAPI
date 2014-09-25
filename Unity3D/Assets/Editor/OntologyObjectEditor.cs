using UnityEngine;
using UnityEditor;
using System.Collections;
using System;
using System.Reflection;

public enum MethodTypes
{
	Ask,
	Gain,
	Lose
};

[CustomEditor(typeof(OntologyObject))]
public class OntologyObjectEditor : Editor {

	private string[] askMethods = null;
	private int _selectedAskMethod = 0;

	private string[] gainMethods = null;
	private int _selectedGainMethod = 0;

	private string[] loseMethods = null;
	private int _selectedLoseMethod = 0;

	// Flags to show the members of this instance and their declared public members
	const BindingFlags flags = BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly;
	
	// Placeholder for checking if the game object has changed
	private GameObject tempAskObject = null;
	private GameObject tempGainObject = null;
	private GameObject tempLoseObject = null;

	/// <summary>
	/// Gets the name of the selected method.
	/// </summary>
	/// <returns>The selected method name.</returns>
	/// <param name="methodType">Method type.</param>
	public string GetSelectedMethodName(MethodTypes methodType)
	{
		// Switch accordingly
		switch(methodType)
		{
		case MethodTypes.Ask:
			if ((this.askMethods == null) || (this._selectedAskMethod == null))
				return null;
			return this.askMethods[this._selectedAskMethod];
		case MethodTypes.Gain:
			if ((this.gainMethods == null)  || (this._selectedGainMethod == null))
				return null;
			return this.gainMethods[this._selectedGainMethod];
		case MethodTypes.Lose:
			if ((this.loseMethods == null  || (this._selectedLoseMethod == null)))
				return null;
			return this.loseMethods[this._selectedLoseMethod];
		default:
			throw new UnityException("GetSelectedMethodName() - Should not be able to get here!");
		}
	}

	/// <summary>
	/// Raises the inspector GU event.
	/// </summary>
	public override void OnInspectorGUI() 
	{
		// Get my target
		OntologyObject myTarget = (OntologyObject)target;

		// Create the views accordingly
		// First, the name
		EditorGUILayout.LabelField("Name", myTarget.Name);

		// Then, each hash and their appropriate callback helpers if the hash exists
		if (!myTarget.AskHash.Equals(""))
		{
			EditorGUILayout.Separator();
			GUILayout.Label("ASK", EditorStyles.boldLabel);
			DisplaySelectedFunction(myTarget, MethodTypes.Ask);
			EditorGUILayout.LabelField("Ask Hash", myTarget.AskHash);
			DefineCallbackView(myTarget, MethodTypes.Ask);
			DefineCallbackDropdown (myTarget, MethodTypes.Ask);
			DefineCallFunction(myTarget, MethodTypes.Ask);

		}

		// Then, each hash and their appropriate callback helpers if the hash exists
		if (!myTarget.GainHash.Equals(""))
		{
			EditorGUILayout.Separator();
			GUILayout.Label("GAIN", EditorStyles.boldLabel);
			DisplaySelectedFunction(myTarget, MethodTypes.Gain);
			EditorGUILayout.LabelField("Gain Hash", myTarget.GainHash);
			DefineCallbackView(myTarget, MethodTypes.Gain);
			DefineCallbackDropdown (myTarget, MethodTypes.Gain);
			DefineCallFunction(myTarget, MethodTypes.Gain);

		}


		// Then, each hash and their appropriate callback helpers if the hash exists
		if (!myTarget.LoseHash.Equals(""))
		{
			EditorGUILayout.Separator();
			GUILayout.Label("LOSE", EditorStyles.boldLabel);
			DisplaySelectedFunction(myTarget, MethodTypes.Lose);
			EditorGUILayout.LabelField("Lose Hash", myTarget.LoseHash);
			DefineCallbackView(myTarget, MethodTypes.Lose);
			DefineCallbackDropdown (myTarget, MethodTypes.Lose);
			DefineCallFunction(myTarget, MethodTypes.Lose);

		}

		if(GUILayout.Button("Refresh"))
		{
			// Always do ASK & GAIN
			PopulateMethodsInformation(myTarget, MethodTypes.Ask);
			PopulateMethodsInformation(myTarget, MethodTypes.Gain);

			if (!myTarget.LoseHash.Equals(""))
			{
				PopulateMethodsInformation(myTarget, MethodTypes.Lose);
			}


		}

	}

	/// <summary>
	/// Displaies the selected function information on ASK/GAIN/LOSE
	/// </summary>
	/// <param name="myTarget">My target.</param>
	/// <param name="methodType">Method type.</param>
	void DisplaySelectedFunction(OntologyObject myTarget, MethodTypes methodType)
	{
		switch(methodType)
		{
		case MethodTypes.Ask:
			EditorGUILayout.LabelField("Selected ASK callback", myTarget.askCallback, EditorStyles.boldLabel);
			break;
		case MethodTypes.Gain:
			EditorGUILayout.LabelField("Selected GAIN callback", myTarget.gainCallback, EditorStyles.boldLabel);
			break;
		case MethodTypes.Lose:
			EditorGUILayout.LabelField("Selected LOSE callback", myTarget.loseCallback, EditorStyles.boldLabel);
			break;
		default:
			throw new UnityException("DisplaySelectedFunction() - Went in to default, which should not be possible!");
		}
	}

	/// <summary>
	/// Defines the call function that can be used by the creator to call the defined function from anywhere.
	/// </summary>
	/// <param name="myTarget">My target.</param>
	/// <param name="methodType">Method type.</param>
	void DefineCallFunction(OntologyObject myTarget, MethodTypes methodType)
	{
		// Define the function call text
		string functionCallText = "OntologyObject script = GameObject.Find(\"" + myTarget.name + "\").GetComponent<OntologyObject>();\n";

		// Then, switch and add the next part accordingly
		switch(methodType)
		{
		case MethodTypes.Ask:
			functionCallText += "script.CallAskFunction();";
			break;
		case MethodTypes.Gain:
			functionCallText += "script.CallGainFunction();";
			break;
		case MethodTypes.Lose:
			functionCallText += "script.CallLoseFunction();";
			break;
		default:
			throw new UnityException("DefineCallFunction() - MethodType was something totally unexpected!");
		}

		// Add the explanation label
		GUILayout.Label("Use the following code anywhere to call this function", EditorStyles.boldLabel);

		// Finally, set the text area accordingly
		EditorGUILayout.TextArea(functionCallText);

	}

	/// <summary>
	/// Defines the callback dropdown selector.
	/// </summary>
	/// <param name="myTarget">My target.</param>
	void DefineCallbackDropdown (OntologyObject myTarget, MethodTypes methodType)
	{
		string[] requestedMethods;
		GameObject requestedGameObject;
		int selectedRequestMethod;
		string popupString;

		// Switch here and check if the methods of the asked method type are still null
		switch(methodType)
		{
		case MethodTypes.Ask:
			requestedMethods = this.askMethods;
			requestedGameObject = myTarget.askGameObject;
			selectedRequestMethod = this._selectedAskMethod;
			popupString = "Ask Callback Method";
			break;
		case MethodTypes.Gain:
			requestedMethods = this.gainMethods;
			requestedGameObject = myTarget.gainGameObject;
			selectedRequestMethod = this._selectedGainMethod;
			popupString = "Gain Callback Method";
			break;
		case MethodTypes.Lose:
			requestedMethods = this.loseMethods;
			requestedGameObject = myTarget.loseGameObject;
			selectedRequestMethod = this._selectedLoseMethod;
			popupString = "Lose Callback Method";
			break;
		default:
			throw new UnityException("DefineCallbackDropdown() - Switching method type resulted in faulty type!");
		}

		// Check if the methods are still null
		if (requestedMethods == null)
		{
			//Debug.Log("Requested method null for type: " + methodType.ToString());

			// Check that the gameobject is not null
			if (requestedGameObject != null)
			{
				// Well, we need to do the extraction of public methods again
				//PopulateMethodsInformation(myTarget, methodType);
			}

		} // Otherwise, start selecting
		else 
		{
			// If the selection is greater than the amount of options
			if ((selectedRequestMethod > requestedMethods.Length) || (selectedRequestMethod == null))
			{
				selectedRequestMethod = 0;
			}

			if(methodType == MethodTypes.Lose)
			{
				Debug.Log ("We  have LOSE type trying to paint itself!");
			}

			// Next, create the selector for the function
			// FIX
			selectedRequestMethod = EditorGUILayout.Popup(popupString, selectedRequestMethod, requestedMethods);

			// And finally, set the chosen value back to appropriate type with the switch
			switch(methodType)
			{
			case MethodTypes.Ask:
				this._selectedAskMethod = selectedRequestMethod;
				myTarget.askCallback = GetSelectedMethodName(MethodTypes.Ask);
				break;
			case MethodTypes.Gain:
				this._selectedGainMethod = selectedRequestMethod;
				myTarget.gainCallback = GetSelectedMethodName(MethodTypes.Gain);
				break;
			case MethodTypes.Lose:
				this._selectedLoseMethod = selectedRequestMethod;
				myTarget.loseCallback = GetSelectedMethodName(MethodTypes.Lose);
				break;
			}

		}


	}

	public void Update()
	{

	}

	/// <summary>
	/// Defines the ASK Callback view
	/// </summary>
	/// <param name="myTarget">My target.</param>
	void DefineCallbackView (OntologyObject myTarget, MethodTypes methodType)
	{
		//tempObject = (GameObject)EditorGUILayout.ObjectField ("Game Object", myTarget.gameObject, typeof(GameObject), true);

		// Switch between the types
		switch(methodType)
		{
		case MethodTypes.Ask:
			this.tempAskObject = (GameObject)EditorGUILayout.ObjectField ("Ask Object", myTarget.askGameObject, typeof(GameObject), true);
			break;
		case MethodTypes.Gain:
			this.tempGainObject = (GameObject)EditorGUILayout.ObjectField ("Gain Object", myTarget.gainGameObject, typeof(GameObject), true);
			break;
		case MethodTypes.Lose:
			this.tempLoseObject = (GameObject)EditorGUILayout.ObjectField ("Lose Object", myTarget.loseGameObject, typeof(GameObject), true);
			break;
		default:
			throw new UnityException("DefineCallbackView() - Switching of MethodTypes had no valid type!");
		}

		ExtractPublicMethods (myTarget, methodType);
	}

	/// <summary>
	/// Extracts the public methods from the attached Game Object.
	/// </summary>
	/// <param name="myTarget">My target.</param>
	void ExtractPublicMethods (OntologyObject myTarget, MethodTypes methodType)
	{

		// Switch between the types
		// And check that their representing objects are not null and differ from existing tempObject
		switch(methodType)
		{
		case MethodTypes.Ask:
			if ((this.tempAskObject != null) && (this.tempAskObject != myTarget.askGameObject)) 
			{
				PopulateMethodsInformation (myTarget, methodType);
			}
			break;
		case MethodTypes.Gain:
			if ((this.tempGainObject != null) && (this.tempGainObject != myTarget.gainGameObject)) 
			{
				PopulateMethodsInformation (myTarget, methodType);
			}
			break;
		case MethodTypes.Lose:
			if ((this.tempLoseObject != null) && (this.tempLoseObject != myTarget.loseGameObject)) 
			{
				PopulateMethodsInformation (myTarget, methodType);
			}
			break;
		default:
			throw new UnityException("ExtractPublicMethods() - Switching of MethodTypes had no valid type!");
		}
	}

	/// <summary>
	/// Populates the methods information.
	/// </summary>
	/// <param name="myTarget">My target.</param>
	void PopulateMethodsInformation (OntologyObject myTarget, MethodTypes methodType)
	{

		GameObject selectedGameobject;


		string[] foundMethods = new string[0];


		// Switch here and act accordingly
		// Switch the gameobject to the temp one
		switch(methodType)
		{
		case MethodTypes.Ask:
			if (this.tempAskObject == null)
			{
				return;
			}
			myTarget.askGameObject = this.tempAskObject;
			selectedGameobject = myTarget.askGameObject;
			break;
		case MethodTypes.Gain:
			if (this.tempGainObject == null)
			{
				return;
			}
			myTarget.gainGameObject = this.tempGainObject;
			selectedGameobject = myTarget.gainGameObject;
			break;
		case MethodTypes.Lose:
			if(this.tempLoseObject == null)
			{
				return;
			}
			myTarget.loseGameObject = this.tempLoseObject;
			selectedGameobject = myTarget.loseGameObject;
			break;
		default:
			throw new UnityException("PopulateMethodsInformation() - Switching of MethodTypes had no valid type!");
		}

		foreach(Component component in selectedGameobject.GetComponents<MonoBehaviour>())
		{
			// Get the count of methods
			foundMethods = new string[component.GetType ().GetMethods (flags).Length];
			int i = 0;
			foreach (MethodInfo method in component.GetType ().GetMethods (flags)) 
			{
				//Debug.Log (method.Name);
				// Put the value to the list
				foundMethods[i] = method.Name;
				// And increment the i
				i++;
			}
		}

		// And then finally, set this information to the appropriate category
		switch(methodType)
		{
		case MethodTypes.Ask:
			this.askMethods = foundMethods;
			//myTarget.askCallback = GetSelectedMethodName(MethodTypes.Ask);
			break;
		case MethodTypes.Gain:
			this.gainMethods = foundMethods;
			//myTarget.gainCallback = GetSelectedMethodName(MethodTypes.Gain);
			break;
		case MethodTypes.Lose:
			this.loseMethods = foundMethods;
			//myTarget.loseCallback = GetSelectedMethodName(MethodTypes.Lose);
			break;
		}

	}
}
