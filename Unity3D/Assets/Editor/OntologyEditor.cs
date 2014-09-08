using UnityEngine;
using UnityEditor;
using System.Collections;

/// <summary>
/// Ontology editor for handling Ontology-type Unity3D classes in the Editor GUI window.
/// </summary>
[CustomEditor(typeof(Ontology))]
public class OntologyEditor : Editor {

	// Private values for holding the user selections from the 
	// ontology type and subtype dropdown selectors
	private int selected = 0;
	private int subSelected = 0;

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

		// Create a helper box to explain what this is all about
		EditorGUILayout.HelpBox("This is used to define the metadata (ontology) for the item/event/achievement to be used in the gamecloud", MessageType.Info);

		// Checks the Ontology type selection from the GUI
		ChooseSelection(myTarget);

		// Creates the ontology when the user presses the "Create Ontology Button"
		if(GUILayout.Button("Create Ontology"))
		{
			myTarget.CreateOntology();
		}

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
	private void ChooseEventSubType(Ontology myTarget)
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
	private void ChooseAchievementSubType(Ontology myTarget)
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
	private void ChooseItemSubType(Ontology myTarget)
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