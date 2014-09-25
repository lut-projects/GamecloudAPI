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

	private bool enableNewOntologyEntry = false;
	private bool showExistingOntologies = true;

	private float MAX_WIDTH = 300;

	// For holding information when selecting existing ontology types
	private int _selectedOntologyViewType = 0;
	private string SelectedViewType = "";

	// For holding the listing of existing ontologies
	private int _selectedExistingOntology = 0;
	private string[] existingOntologiesList;
	private string[] existingOntologiesDescription;

	// Settings
	private int selected = 0;
	private int subSelected = 0;
	private int actionSelected = 0;
	private Gamecloud.Gamecloud gamecloud = Gamecloud.Gamecloud.Instance;// new Gamecloud.Gamecloud("http://example.com", "auth");
	private OntologyEdito ontologyGenerator = new OntologyEdito();

	// For holding the information of the games of the creator
	private string[] gamesList;
	private int _selectedGame = 0;

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
		// Start with creating a checkbox 
		this.enableNewOntologyEntry = EditorGUILayout.Toggle("Show New Ontology Entry", this.enableNewOntologyEntry);
		if (this.enableNewOntologyEntry)
		{
			CreateOntologyView();
		}
		// Fourth, show the existing ontologies
		this.showExistingOntologies = EditorGUILayout.Toggle("Show Existing Ontologies", this.showExistingOntologies);
		if (this.showExistingOntologies)
		{
			DisplayOntologiesView();
		}

	}

	/// <summary>
	/// Displays the ontologies view.
	/// </summary>
	private void DisplayOntologiesView()
	{
		// Add the header
		GUILayout.Label("Display Existing Ontologies", EditorStyles.boldLabel);
		// First, create a dropdown selector for the type of ontologies to show
		ChooseOntologyViewType();
		// Next, draw the action button for updating the selection
		if(GUILayout.Button("Fetch types", GUILayout.MaxWidth(MAX_WIDTH)))
		{
			gamecloud.GetOntologiesByGame(this.SelectedViewType, GetGameName(), "auth", GetOntologiesByGameCallback, true);
		}

		// Check that there are existing ontologies
		if (this.existingOntologiesList != null)
		{
			// If there are more than 0 elements
			if (this.existingOntologiesList.Length > 0)
			{
				// Now, select the game
				this._selectedExistingOntology = EditorGUILayout.Popup("Select Ontology", this._selectedExistingOntology, this.existingOntologiesList, GUILayout.MaxWidth(MAX_WIDTH));
				// Also, show the description
				// Make sure the listing exists
				if (this.existingOntologiesDescription.Length > 0)
				{
					GUILayout.Label("Description:");
					GUILayout.Box(this.existingOntologiesDescription[this._selectedExistingOntology]);
				}

				// And then create a button enabling adding this to the project
				if (GUILayout.Button("Generate in Project", GUILayout.MaxWidth(MAX_WIDTH)))
				{
					GenerateSelectionToProject();
				}
			
			}

		}
	}

	/// <summary>
	/// Generates the selected ontology type to the project and instantiates a Gameobject for it.
	/// </summary>
	private void GenerateSelectionToProject()
	{
		// TODO: Generate the wanted ontology into the project here
		// e.g. Instantiate it!
		Debug.Log("Starting generation of the selection to Project");
		// Fetch the Gain/Lose/Ask hashes from the server
		gamecloud.GetHashesOfEntry("auth", this.GetExistingOntologySelected(), this.SelectedViewType, GetHashesOfEntryCallback, true);
	}
	
	void BuilOntologyObject (Types type, string name, string askHash, string gainHash, string loseHash)
	{
		// Start with Instantiating the new ontology object
		GameObject gameObject = new GameObject(name);
		gameObject.AddComponent<OntologyObject>().DefineOntology(type, name, askHash, gainHash, loseHash);
	
		//OntologyObject ontologyObject = new OntologyObject();

		// Next, define the object types
		//ontologyObject.DefineOntology(type, name, askHash, gainHash, loseHash);

		// And job is done :-D

	}

	void ExtractAndBuilAchievementGameObject (ArrayList list)
	{
		// Prepare to extract all the required info from the result JSON
		string gainHash = "";
		string askHash = "";
		string name = "";

		// Loop over the lists
		foreach (Hashtable entry in list)
		{
			// If we have the proper hashes
			if(entry.ContainsKey("askHash"))
			{
				// Figure out the type
				gainHash = entry["gainHash"].ToString();
				askHash = entry["askHash"].ToString();
				name = this.RemoveURI(entry["name"].ToString());
			}
		}

		Debug.Log("Name: " + name + " gainHash: " + gainHash + " askHash: " + askHash);

		// Okay, now we really need to BUILD the ontology GameObject
		BuilOntologyObject(Types.Achievement, name, askHash, gainHash, null);

	} // End of ExtractAndBuilAchievementGameObject

	void ExtractAndBuilEventGameObject (ArrayList list)
	{
		// Prepare to extract all the required info from the result JSON
		string triggerHash = "";
		string askHash = "";
		string askName = "";
		string triggerName = "";

		// Loop over the lists
		foreach (Hashtable entry in list)
		{
			// If we have the proper hashes
			if(entry.ContainsKey("askHash"))
			{
				// Figure out the contents
				askHash = entry["askHash"].ToString();
				askName = this.RemoveURI(entry["name"].ToString());
			}
			if(entry.ContainsKey("triggerHash"))
			{
				triggerHash = entry["triggerHash"].ToString();
				triggerName = this.RemoveURI(entry["name"].ToString());
			}
		}

		
		// Now, start creating the stuff
		Debug.Log("AskName: " + askName + " AskHash: " + askHash + " TriggerName: " + triggerName + " TriggerHash: " + triggerHash);

	} // End of ExtractAndBuilEventGameObject 

	void ExtractAndBuildItemGameObject (ArrayList list)
	{
		throw new NotImplementedException ();
	} // End of ExtractAndBuildItemGameObject

	/// <summary>
	/// The callback for getting hashes of a certain entry
	/// </summary>
	/// <param name="error">Error.</param>
	/// <param name="result">Result.</param>
	public void GetHashesOfEntryCallback(string error, Hashtable result)
	{
		// Once we get the results
		// We should instantiate an ontology, that contains all this information
		// And put it into the project as a Unity GameObject
		Debug.Log(JSON.JsonEncode(result));

		// Get the type
		string resultType = result["type"].ToString();

		// Next, parse the results into a list
		ArrayList list = (ArrayList)result["hashes"];

		// Then, Act according to the type of the result we got
		if (resultType.Equals("Achievement"))
		{
			ExtractAndBuilAchievementGameObject(list);
		}
		else if (resultType.Equals("Event"))
		{
			ExtractAndBuilEventGameObject(list);
		}
		else if (resultType.Equals("Item"))
		{
			ExtractAndBuildItemGameObject(list);
		}


		/*
		// Ookay, we can read the returned type here
		if(result["type"].ToString() == "Achievement")
		{
			Debug.Log("We got an Achievement reply");

			// Then, we need to parse the gain & ask hashes
			ArrayList list = (ArrayList)result["hashes"];

			// Loop over the lists
			foreach (Hashtable entry in list)
			{
				Debug.Log (JSON.JsonEncode(entry));

				// If we have the proper hashes
				if(entry.ContainsKey("askHash"))
				{
					// Figure out the type
					gainHash = entry["gainHash"].ToString();
					askHash = entry["askHash"].ToString();
					name = this.RemoveURI(entry["name"].ToString());
				}


			} // End of foreach

			Debug.Log("Name: " + name + " gainHash: " + gainHash + " askHash: " + askHash);
		}

		else if (result["type"].ToString() == "Event")
		{
			Debug.Log("We got an Event reply");

			// Then, we need to parse the trigger & ask hashes
			ArrayList list = (ArrayList)result["hashes"];

			string triggerHash = "";
			string askHash = "";
			string askName = "";
			string triggerName = "";

			// Loop over the lists
			foreach(Hashtable entry in list)
			{
				Debug.Log(JSON.JsonEncode(entry));

				// If we have the proper hashes
				if(entry.ContainsKey("askHash"))
				{
					// Figure out the contents
					askHash = entry["askHash"].ToString();
					askName = this.RemoveURI(entry["name"].ToString());
				}
				if(entry.ContainsKey("triggerHash"))
				{
					triggerHash = entry["triggerHash"].ToString();
					triggerName = this.RemoveURI(entry["name"].ToString());
				}
			} // End of foreach

			Debug.Log("AskName: " + askName + " AskHash: " + askHash + " TriggerName: " + triggerName + " TriggerHash: " + triggerHash);
		}
		else if (result["type"].ToString() == "Item")
		{
			Debug.Log("We got an Item reply");
		}

		*/


	}


	/// <summary>
	/// The callback for getting ontologies
	/// </summary>
	/// <param name="error">Error.</param>
	/// <param name="result">Result.</param>
	public void GetOntologiesByGameCallback(string error, Hashtable result)
	{
		Debug.Log(JSON.JsonEncode(result));

		// Create an array list in order to parse through the etnries
		ArrayList list = (ArrayList)result["entries"];
		
		// Get the amount of games in the listing
		existingOntologiesList = new string[list.Count];
		// Also, add the description listing
		existingOntologiesDescription = new string[list.Count];
		for (int i=0; i<list.Count; i++)
		{
			// Cast each entry as a hashtable
			Hashtable entry = (Hashtable)list[i];
			// Then, add the game to the listing
			// Selecting by the type we are using

			if (this.SelectedViewType == "Item")
			{
				existingOntologiesList[i] = this.RemoveURI(entry["item"].ToString());
			}
			else if (this.SelectedViewType == "Event")
			{
				existingOntologiesList[i] = this.RemoveURI(entry["event"].ToString());
			}
			else if (this.SelectedViewType == "Achievement")
			{
				existingOntologiesList[i] = this.RemoveURI(entry["achievement"].ToString());
			}

			// Also, add the comments
			existingOntologiesDescription[i] = entry["comment"].ToString();

		}
		
	}

	/// <summary>
	/// Chooses the type of the ontology for showing in the view.
	/// </summary>
	private void ChooseOntologyViewType()
	{
		string[] options = new string[]
		{
			"Achievement",
			"Event",
			"Item"
		};
		
		this._selectedOntologyViewType = EditorGUILayout.Popup("Ontology Type", this._selectedOntologyViewType, options, GUILayout.MaxWidth(MAX_WIDTH));
		
		// Check if the already existing selected value is higher than the existing options
		// e.g. you have selected from different type and list that has more options (achievement)
		// if so, make the selected to be 0, in order to prevent errors
		if(this._selectedOntologyViewType > options.Length) {
			this._selectedOntologyViewType = 0;
		}
		this.SelectedViewType = options[this._selectedOntologyViewType];
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

		// If the list is not null
		if(this.gamesList != null)
		{
			// If there are more than 0 elements
			if (this.gamesList.Length > 0)
			{
				// Now, select the game
				this._selectedGame = EditorGUILayout.Popup("Games", this._selectedGame, this.gamesList, GUILayout.MaxWidth(MAX_WIDTH));
			}
		}

	}

	/// <summary>
	/// Gets the existing ontology selected.
	/// </summary>
	/// <returns>The existing ontology selected.</returns>
	private string GetExistingOntologySelected()
	{
		return this.existingOntologiesList[this._selectedExistingOntology];
	}

	/// <summary>
	/// Gets the name of the game.
	/// </summary>
	/// <returns>The game name.</returns>
	private string GetGameName()
	{
		return this.gamesList[this._selectedGame];
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
		// Remove, if we get ex: in the start
		entry = entry.Replace("ex:", "");
		// Remove the starting part of the URI from the received entry
		entry = entry.Replace("http://example.org/", "");
		return entry;
	}

	/// <summary>
	/// Creates the connection view.
	/// </summary>
	private void CreateConnectionView()
	{
		// First, the connection testing
		GUILayout.Label("Gamecloud Connection Settings", EditorStyles.boldLabel);
		GamecloudAddress = EditorGUILayout.TextField("Gamecloud Address", GamecloudAddress, GUILayout.MaxWidth(MAX_WIDTH));
		// Set the address to be gamecloud address as well
		gamecloud.ChangeServerAddress(GamecloudAddress);

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
	/// Gets the hash for sha512.
	/// </summary>
	/// <returns>The sha512 hash.</returns>
	/// <param name="text">Text to hash.</param>
	public static string getHashSha512(string text)
	{
		byte[] bytes = Encoding.UTF8.GetBytes(text);
		SHA512Managed hashstring = new SHA512Managed();
		byte[] hash = hashstring.ComputeHash(bytes);
		string hashString = string.Empty;
		foreach (byte x in hash)
		{
			hashString += String.Format("{0:x2}", x);
		}
		return hashString;
	}

	/// <summary>
	/// Login method for authenticating with the Gamecloud
	/// </summary>
	private void Login()
	{
		// Hash the password
		string hash = getHashSha512(GamecloudPass);
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

		// Set the gamecloud authkey from the result
		gamecloud.ChangeAuthkey(result["authToken"].ToString());

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
			// Create the json for sending to backend
			Hashtable json = ontologyGenerator.CreateOntology(gamesList[this._selectedGame]);

			// And then CALL
			gamecloud.CreateOntologyEntry(json, CreateOntologyEntryCallback, true);
		}
	}

	/// <summary>
	/// Creates the ontology entry callback.
	/// </summary>
	/// <param name="error">Error.</param>
	/// <param name="result">Result.</param>
	private void CreateOntologyEntryCallback(string error, Hashtable result)
	{
		// And here we just want to parse the stuff first
		Debug.Log(JSON.JsonEncode(result));
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
