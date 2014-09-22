using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using System;
using System.Text;

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
	private Gamecloud.Gamecloud gamecloud = new Gamecloud.Gamecloud("http://example.com", "auth");
	private OntologyEdito ontologyGenerator = new OntologyEdito();

	// For holding the information of the games of the creator
	private string[] gamesList;
	private int _selectedGame = 0;

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
		// Create a button for fetching games
		if(GUILayout.Button ("Fetch Games", GUILayout.MaxWidth(MAX_WIDTH)))
		{
			this.GetGames();
		}
		// Set the name of the game
		EditorGUILayout.HelpBox("The name of the game you are working with (no spaces please!)", MessageType.Info);

		// Now, select the game
		this._selectedGame = EditorGUILayout.Popup("Games", this._selectedGame, this.gamesList, GUILayout.MaxWidth(MAX_WIDTH));
	}

	/// <summary>
	/// Gets the games from the server.
	/// </summary>
	private void GetGames()
	{
		// Get the games of the user from the gamecloud
		gamecloud.GetGames("ex:" + this.GamecloudUser, "authkey", GetGamesCallback, true);
	}

	/// <summary>
	/// Callback used when requesting for game listing from the backend	
	/// </summary>
	/// <param name="error">Error.</param>
	/// <param name="result">Result.</param>
	private void GetGamesCallback(string error, Hashtable result)
	{
		// Encode to show the entires
		Debug.Log(JSON.JsonEncode(result));
		// Create an array list in order to parse through the etnries
		ArrayList list = (ArrayList)result["entries"];

		// Get the amount of games in the listing
		gamesList = new string[list.Count];
		for (int i=0; i<list.Count; i++)
		{
			// Cast each entry as a hashtable
			Hashtable entry = (Hashtable)list[i];
			// Then, add the game to the listing
			gamesList[i] = this.RemoveURI(entry["game"].ToString());
		}

	}
	
	/// <summary>
	/// Removes the URI part from the given string.
	/// </summary>
	/// <returns>The string without URI.</returns>
	/// <param name="entry">Entry.</param>
	private string RemoveURI(string entry)
	{
		// Remove the starting part of the URI from the received entry
		return entry.Replace("http://example.org/", "");
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
			// Make sure, that we have the proper address
			gamecloud.ChangeServerAddress(GamecloudAddress);
			// Authenticate with gamecloud
			Login();
		}
	}

	/// <summary>
	/// Login method for authenticating with the Gamecloud
	/// </summary>
	private void Login()
	{
		// First, hash the password
		SHA512 hasher = new SHA512Managed();
		UnicodeEncoding encoding = new UnicodeEncoding();
		byte[] hashResult = hasher.ComputeHash(encoding.GetBytes(GamecloudPass));

		// Then, loop over to get the result hash in string representation
		string hash = "";
		foreach (byte x in hashResult)
		{
			hash += String.Format("{0:x2}", x);
		}

		Debug.Log(hash);
		// Send the message to the server
		gamecloud.Authenticate(GamecloudUser, hash, LoginCallback, true);
	}

	/// <summary>
	/// Callback for the Login
	/// </summary>
	/// <param name="error">Error.</param>
	/// <param name="result">Result.</param>
	private void LoginCallback(string error, Hashtable result)
	{
		if (error != null)
		{
			Debug.LogError(error);
		}
		
		// To encode the message into a JSON string
		Debug.Log (JSON.JsonEncode(result));
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
