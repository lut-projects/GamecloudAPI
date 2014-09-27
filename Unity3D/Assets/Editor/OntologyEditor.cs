using UnityEngine;
using UnityEditor;
using System.Collections;


[CustomEditor(typeof(Ontology))]
public class OntologyEditor : Editor 
{

	public int actionSelected;
    public int selectedHash;

	/// <summary>
	/// Raises the inspector GU event.
	/// </summary>
	public override void OnInspectorGUI() 
	{
		// Get my target
		Ontology myTarget = (Ontology)target;
		// Draws the default inspector
		// TODO: Fix this to be something else (we don't want to show everything probably)
		DrawDefaultInspector ();

		// Choose the action type from the list
		ChooseSelection(myTarget);

        // Select the wanted hash from the refreshed listing
        SelectHash(myTarget);

        // Creates the ontology when the user presses the "Create Ontology Button"
        if (GUILayout.Button("Get Listing"))
        {
            myTarget.RefreshListing();
        }
		

	}

    /// <summary>
    /// Selects the hash from the options menu
    /// </summary>
    /// <param name="myTarget">The Ontology</param>
    private void SelectHash(Ontology myTarget)
    {
        // Check if there are more than 0 items in the Listing.Count
        if (myTarget.Listing.Count <= 0)
        {
            return;
        }
        // Otherwise, keep on going
        // Create the string list as long as the count
        string[] listing = new string[myTarget.Listing.Count];
        // Then loop through it, populating as you go
        int i = 0;
        foreach(string item in myTarget.Listing)
        {
            // Add the item to listing
            listing[i] = item;
            i++;
        }
        // Then, get the selected hash
        this.selectedHash = EditorGUILayout.Popup("ActionName", this.selectedHash, listing);

        // Then, set the Action name and retrieve the hash
        myTarget.SetActionName(myTarget.Listing[this.selectedHash]);
    }

	/// <summary>
	/// Chooses between what type the user has selected from the ontology type dropdown box
	/// </summary>
	/// <param name="myTarget">The target script for this editor extension.</param>
	private void ChooseSelection(Ontology myTarget)
	{
		// Switch according to the type of the ontology the user has selected
		switch(myTarget.Type)
		{
		case Types.Event:
			SelectEventAction(myTarget);
			break;
		case Types.Item:
			SelectItemAction(myTarget);
			break;
		case Types.Achievement:
			SelectAchievementAction(myTarget);
			break;
		default:
			throw new UnityException("Faulty Ontology Type Selected!");
		}
		
	}

	public void SelectEventAction(Ontology myTarget)
	{

		string[] actions = new string[]
		{
			"Ask",
			"Trigger",
		};
		
		this.actionSelected = EditorGUILayout.Popup("ActionType", this.actionSelected, actions);
		
		// Check if the already existing selected value is higher than the existing options
		// e.g. you have selected from different type and list that has more options (achievement)
		// if so, make the selected to be 0, in order to prevent errors
		if(this.actionSelected > actions.Length) {
			this.actionSelected = 0;
		}
		myTarget.ActionType = actions[this.actionSelected];
	}

	public void SelectItemAction(Ontology myTarget)
	{
		
		string[] actions = new string[]
		{
			"Ask",
			"Gain",
			"Lose"
		};
		
		this.actionSelected = EditorGUILayout.Popup("ActionType", this.actionSelected, actions);

		myTarget.ActionType = actions[this.actionSelected];
		
	}


	public void SelectAchievementAction(Ontology myTarget)
	{
		
		string[] actions = new string[]
		{
			"Ask",
			"Gain"
		};
		
		this.actionSelected = EditorGUILayout.Popup("ActionType", this.actionSelected, actions);
		
		// Check if the already existing selected value is higher than the existing options
		// e.g. you have selected from different type and list that has more options (achievement)
		// if so, make the selected to be 0, in order to prevent errors
		if(this.actionSelected > actions.Length) {
			this.actionSelected = 0;
		}
		myTarget.ActionType = actions[this.actionSelected];
	}
	


}
