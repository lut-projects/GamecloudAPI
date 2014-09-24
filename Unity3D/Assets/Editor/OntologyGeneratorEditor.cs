using UnityEngine;
using UnityEditor;
using System.Collections;
using Gamecloud;

/// <summary>
/// Ontology editor for handling Ontology-type Unity3D classes in the Editor GUI window.
/// </summary>
[CustomEditor(typeof(OntologyGenerator))]
public class OntologyGeneratorEditor : Editor {

	// Private values for holding the user selections from the 
	// ontology type and subtype dropdown selectors
	private int selected = 0;
	private int subSelected = 0;
	private int actionSelected = 0;
	private Gamecloud.Gamecloud gamecloud = Gamecloud.Gamecloud.Instance;//  new Gamecloud.Gamecloud("http://", "as");


	/// <summary>
	/// Raises the inspector GU event.
	/// </summary>
	public override void OnInspectorGUI() 
	{
		// Get my target
		OntologyGenerator myTarget = (OntologyGenerator)target;
		// Draws the default inspector
		// TODO: Fix this to be something else (we don't want to show everything probably)
		DrawDefaultInspector ();

		if (myTarget.AchievementsDictionary.Count == 0) 
		{
			myTarget.AchievementsDictionary.Add("One", "FirstLongHash");
			myTarget.AchievementsDictionary.Add("Two", "SecondHash");
		}



		// Create a helper box to explain what this is all about
		EditorGUILayout.HelpBox("This is used to define the metadata (ontology) for the item/event/achievement to be used in the gamecloud", MessageType.Info);

		// Checks the Ontology type selection from the GUI
		ChooseSelection(myTarget);

		ChooseAction(myTarget);

		// Creates the ontology when the user presses the "Create Ontology Button"
		if(GUILayout.Button("Create Ontology"))
		{
			myTarget.CreateOntology();
		}

        // Creates a button for checking gamecloud connection
        if(GUILayout.Button("Test Connection"))
        {
            myTarget.TestConnection();
        }

	}

	/// <summary>
	/// Chooses the subtype for the item
	/// </summary>
	/// <param name="myTarget">The target script for this editor extension</param>
	private void ChooseAction(OntologyGenerator myTarget)
	{
		string[] actions = new string[]
		{
			"One",
			"Two"
		};
		
		this.actionSelected = EditorGUILayout.Popup("ActionType", this.actionSelected, actions);
		
		// Check if the already existing selected value is higher than the existing options
		// e.g. you have selected from different type and list that has more options (achievement)
		// if so, make the selected to be 0, in order to prevent errors
		if(this.actionSelected > actions.Length) {
			this.actionSelected = 0;
		}
		myTarget.ActionType = myTarget.AchievementsDictionary[actions[this.actionSelected]];
	}


	/// <summary>
	/// Chooses between what type the user has selected from the ontology type dropdown box
	/// </summary>
	/// <param name="myTarget">The target script for this editor extension.</param>
	private void ChooseSelection(OntologyGenerator myTarget)
	{
		// Switch according to the type of the ontology the user has selected
		switch(myTarget.Type)
		{
		case Types.Event:
			ChooseEventSubType(myTarget);
			break;
		case Types.Achievement:
			ChooseAchievementSubType(myTarget);
			break;
		case Types.Item:
			ChooseItemSubType(myTarget);
			break;
		default:
			throw new UnityException("Faulty Ontology Type Selected!");
		}

	}

	/// <summary>
	/// Chooses the sub type of the event
	/// </summary>
	/// <param name="myTarget">The target script for this editor extension</param>
	private void ChooseEventSubType(OntologyGenerator myTarget)
	{
		string[] options = new string[]
		{
			"InAppPurchaseEvent",
			"PlayerEvent",
			"GameEvent",
			"MenuEvent"
		};
		
		this.selected = EditorGUILayout.Popup("Subtype", this.selected, options);

		// Check if the already existing selected value is higher than the existing options
		// e.g. you have selected from different type and list that has more options (achievement)
		// if so, make the selected to be 0, in order to prevent errors
		if(this.selected > options.Length) {
			selected = 0;
		}
		myTarget.SubType = options[this.selected];

		// If we have game event
		if (this.selected == 2)
		{
			string[] subOptions = new string[]
			{
				"GainEvent",
				"LoseEvent"
			};

			this.subSelected = EditorGUILayout.Popup("Lower Subtype", this.subSelected, subOptions);
			myTarget.SubType = subOptions[this.subSelected];
		}
	}

	/// <summary>
	/// Chooses the sub type of the achievement
	/// </summary>
	/// <param name="myTarget">The target script for this editor extension</param>
	private void ChooseAchievementSubType(OntologyGenerator myTarget)
	{
		string[] options = new string[]
		{
			"Fandom",
			"Veteran",
			"Minigame",
			"Loyalty",
			"Paragon",
			"Luck",
			"Veteran",
			"HardMode",
			"Tutorial",
			"Collection",
			"Virtuosity",
			"Completion",
			"Curiosity",
			"SpecialPlayStyle"
		};
		
		this.selected = EditorGUILayout.Popup("Subtype", this.selected, options);
		myTarget.SubType = options[this.selected];
	}

	/// <summary>
	/// Chooses the subtype for the item
	/// </summary>
	/// <param name="myTarget">The target script for this editor extension</param>
	private void ChooseItemSubType(OntologyGenerator myTarget)
	{
		string[] options = new string[]
		{
			"None"
		};
		
		this.selected = EditorGUILayout.Popup("Subtype", this.selected, options);

		// Check if the already existing selected value is higher than the existing options
		// e.g. you have selected from different type and list that has more options (achievement)
		// if so, make the selected to be 0, in order to prevent errors
		if(this.selected > options.Length) {
			selected = 0;
		}
		myTarget.SubType = options[this.selected];
	}
	
}