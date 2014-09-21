using UnityEngine;
using UnityEditor;
using System.Collections;

public class OntologyEditorWindow : EditorWindow
{
	// Gamecloud connection settings
	public string GamecloudAddress = "http://as";
	public string GamecloudUser = "";
	public string GamecloudPass = "";
	// End of Connection settings


	private float MAX_WIDTH = 300;

	private string GameName = "";

	// Settings
	private int selected = 0;
	private int subSelected = 0;
	private int actionSelected = 0;
	private Gamecloud.Gamecloud gamecloud = new Gamecloud.Gamecloud("http://", "as");
	private OntologyEdito ontologyGenerator = new OntologyEdito();



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
		// First, create the connection part
		CreateConnectionView();
		// Second, create the select game / new game part
		CreateGameView();
		// Third, create the Create new ontology part
		CreateOntologyView();

		/*
		if (ontologyGenerator.AchievementsDictionary.Count == 0) 
		{
			ontologyGenerator.AchievementsDictionary.Add("One", "FirstLongHash");
			ontologyGenerator.AchievementsDictionary.Add("Two", "SecondHash");
		}*/

	}

	/// <summary>
	/// Creates the game view.
	/// </summary>
	private void CreateGameView()
	{
		GUILayout.Label("Select/Create Current Game", EditorStyles.boldLabel);
		// Set the name of the game
		EditorGUILayout.HelpBox("The name of the game you are working with (no spaces please!)", MessageType.Info);
		this.GameName = EditorGUILayout.TextField("Game Name", this.GameName, GUILayout.MaxWidth(MAX_WIDTH));

	}

	/// <summary>
	/// Creates the connection view.
	/// </summary>
	private void CreateConnectionView()
	{
		// First, the connection testing
		GUILayout.Label("Gamecloud Connection Settings", EditorStyles.boldLabel);
		GamecloudAddress = EditorGUILayout.TextField("Gamecloud Address", GamecloudAddress, GUILayout.MaxWidth(MAX_WIDTH));

		// Creates a button for checking gamecloud connection
		if(GUILayout.Button("Test Connection", GUILayout.MaxWidth(MAX_WIDTH)))
		{
			this.TestConnection();
		}

		GamecloudUser = EditorGUILayout.TextField("Username", GamecloudUser, GUILayout.MaxWidth(MAX_WIDTH));
		GamecloudPass = EditorGUILayout.TextField("Password", GamecloudPass, GUILayout.MaxWidth(MAX_WIDTH));
		
		if(GUILayout.Button("Login", GUILayout.MaxWidth(MAX_WIDTH)))
		{
			// Log in, test the connection
		}
	}

	/// <summary>
	/// Tests the connection to Gamecloud.
	/// </summary>
	private void TestConnection()
	{
		// Add a test string to the given address
		string testString = GamecloudAddress + "/?callType=xyzzy";

		// Then, perform the query
		gamecloud.GetFromServer(testString, ConnectionTestCallback, true);
	}

	/// <summary>
	/// A callback function for TestConnection()
	/// </summary>
	/// <param name="message">The message received from the server</param>
	public void ConnectionTestCallback(string message)
	{
		Debug.Log(message);
	}

	/// <summary>
	/// Creates the ontology view.
	/// </summary>
	private void CreateOntologyView()
	{
		// The Label part for the creation
		GUILayout.Label("Create New Ontology", EditorStyles.boldLabel, GUILayout.MaxWidth(MAX_WIDTH));
		
		// Set the name
		EditorGUILayout.HelpBox("The name you want to use for the entry (no spaces please!)", MessageType.Info);
		this.ontologyGenerator.Name = EditorGUILayout.TextField("Name", this.ontologyGenerator.Name, GUILayout.MaxWidth(MAX_WIDTH));
		
		// Set the description
		EditorGUILayout.HelpBox("Write a description, that tells something about the entry. This will help you later to understand what this is meant for", MessageType.Info);
		this.ontologyGenerator.Description = EditorGUILayout.TextField("Description", this.ontologyGenerator.Description, GUILayout.MaxWidth(MAX_WIDTH));
		
		// Select the Type
		EditorGUILayout.HelpBox("Select the type and possible subtype for the entry", MessageType.Info);
		this.ontologyGenerator.Type = (Types)EditorGUILayout.EnumPopup("Type", this.ontologyGenerator.Type, GUILayout.MaxWidth(MAX_WIDTH));
		
		// Select the subtype
		this.ChooseSelection();
		
		// Creates the ontology when the user presses the "Create Ontology Button"
		if(GUILayout.Button("Create Ontology", GUILayout.MaxWidth(MAX_WIDTH)))
		{
			ontologyGenerator.CreateOntology();
		}
	}

	/// <summary>
	/// Chooses between what type the user has selected from the ontology type dropdown box
	/// </summary>
	/// <param name="myTarget">The target script for this editor extension.</param>
	private void ChooseSelection()
	{
		// Switch according to the type of the ontology the user has selected
		switch(this.ontologyGenerator.Type)
		{
		case Types.Event:
			ChooseEventSubType();
			break;
		case Types.Achievement:
			ChooseAchievementSubType();
			break;
		case Types.Item:
			ChooseItemSubType();
			break;
		default:
			throw new UnityException("Faulty Ontology Type Selected!");
		}
		
	}
	
	/// <summary>
	/// Chooses the sub type of the event
	/// </summary>
	/// <param name="myTarget">The target script for this editor extension</param>
	private void ChooseEventSubType()
	{
		string[] options = new string[]
		{
			"InAppPurchaseEvent",
			"PlayerEvent",
			"GameEvent",
			"MenuEvent"
		};
		
		this.selected = EditorGUILayout.Popup("Subtype", this.selected, options, GUILayout.MaxWidth(MAX_WIDTH));
		
		// Check if the already existing selected value is higher than the existing options
		// e.g. you have selected from different type and list that has more options (achievement)
		// if so, make the selected to be 0, in order to prevent errors
		if(this.selected > options.Length) {
			selected = 0;
		}
		this.ontologyGenerator.SubType = options[this.selected];
		
		// If we have game event
		if (this.selected == 2)
		{
			string[] subOptions = new string[]
			{
				"GainEvent",
				"LoseEvent"
			};
			
			this.subSelected = EditorGUILayout.Popup("Lower Subtype", this.subSelected, subOptions, GUILayout.MaxWidth(MAX_WIDTH));
			this.ontologyGenerator.SubType = subOptions[this.subSelected];
		}
	}
	
	/// <summary>
	/// Chooses the sub type of the achievement
	/// </summary>
	/// <param name="myTarget">The target script for this editor extension</param>
	private void ChooseAchievementSubType()
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
		
		this.selected = EditorGUILayout.Popup("Subtype", this.selected, options, GUILayout.MaxWidth(MAX_WIDTH));
		this.ontologyGenerator.SubType = options[this.selected];
	}
	
	/// <summary>
	/// Chooses the subtype for the item
	/// </summary>
	/// <param name="myTarget">The target script for this editor extension</param>
	private void ChooseItemSubType()
	{
		string[] options = new string[]
		{
			"None"
		};
		
		this.selected = EditorGUILayout.Popup("Subtype", this.selected, options, GUILayout.MaxWidth(MAX_WIDTH));
		
		// Check if the already existing selected value is higher than the existing options
		// e.g. you have selected from different type and list that has more options (achievement)
		// if so, make the selected to be 0, in order to prevent errors
		if(this.selected > options.Length) {
			selected = 0;
		}
		this.ontologyGenerator.SubType = options[this.selected];
	}

}
