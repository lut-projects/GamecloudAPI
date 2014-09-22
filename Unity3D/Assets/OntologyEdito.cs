using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public enum MyTypes
{
	Achievement = 0,
	Event = 1,
	Item = 2
};

/// <summary>
/// Public class (the main thing to use) for creating & managing ontologies in the system
/// </summary>
public class OntologyEdito {
	
	
	public Dictionary<string, string> AchievementsDictionary = new Dictionary<string, string>();
	public string GamecloudAddress = "http://";
	public string Authkey = "";
	
	// Public values for displaying/editing the ontology stub
	public string Name = "";
	public Types Type;
	public string SubType = "";
	public string Description = "";
	public string ActionType = "";
	
	/// <summary>
	/// Creates the ontology.
	/// </summary>
	public Hashtable CreateOntology(string game) 
	{
		// Decide here whether to create a Item/Event/Achievement ontology
		switch(Type)
		{
		case Types.Achievement:
			return CreateAchievementOntology(game);
		case Types.Event:
			return CreateEventOntology(game);
		case Types.Item:
			return CreateItemOntology(game);
		default:
			throw new Exception("Should not be possible to reach here!");
		}
	}

	private Hashtable CreateAchievementOntology(string game)
	{
		Debug.Log("Creating Achievement Ontology");

		// Okay, lets start working on the new ontology
		Hashtable achievementJson = new Hashtable();
		achievementJson.Add("callType", "createAchievement");
		achievementJson.Add("authkey", Authkey);
		achievementJson.Add("name", "ex:" + Name);
		achievementJson.Add("gameName", "ex:" + game);
		achievementJson.Add("achievementType", "game:" + SubType);
		achievementJson.Add("comment", Description);

		Debug.Log(JSON.JsonEncode(achievementJson));

		// Return the piece
		return achievementJson;
	}

	private Hashtable CreateEventOntology(string game)
	{
		// Okay, lets start working on the new ontology
		Hashtable eventJson = new Hashtable();
		eventJson.Add("callType", "createEvent");
		eventJson.Add("authkey", Authkey);
		eventJson.Add("name", "ex:" + Name);
		eventJson.Add("eventType", "game:" + SubType);
		eventJson.Add("gameName", "ex:" + game);
		eventJson.Add("characterRelation", "no");
		eventJson.Add("itemRelation", "no");
		eventJson.Add("sessionRelation", "no");
		eventJson.Add("comment", Description);
		
		Debug.Log(JSON.JsonEncode(eventJson));

		return eventJson;
	}

	private Hashtable CreateItemOntology(string game)
	{
		// Okay, lets start working on the new ontology
		Hashtable itemJson = new Hashtable();
		itemJson.Add("callType", "createItem");
		itemJson.Add("authkey", Authkey);
		itemJson.Add("name", "ex:" + Name);
		itemJson.Add("itemType", "[]");
		itemJson.Add("itemFeature", "[]");
		itemJson.Add("gameName", game);
		itemJson.Add("comment", Description);
		
		Debug.Log(JSON.JsonEncode(itemJson));

		return itemJson;
	}
	
	public void TestConnection()
	{
		// Test the connection
		//Debug.Log("Testing connection");
	}
	
	/// <summary>
	/// Creates the session identifier.
	/// </summary>
	/// <returns>The session identifier string.</returns>
	public string CreateSessionId()
	{
		// Create the random session
		string sessionId = "Session" + GetISOTimeInUTC() + KeyGenerator.GetUniqueKey(10);
		// And return the results
		return sessionId;
	}
	
	/// <summary>
	/// Gets the ISO time in UT.
	/// </summary>
	/// <returns>The ISO time in UT.</returns>
	public string GetISOTimeInUTC()
	{
		return DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ss.fffffffzzz");
	}
}



