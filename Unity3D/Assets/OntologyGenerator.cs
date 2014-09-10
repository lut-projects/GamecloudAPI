using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;



/// <summary>
/// Enumerates the types of ontology that we have existing in the system
/// </summary>
public enum Types 
{
	Achievement,
	Event,
	Item
}

/// <summary>
/// Public class (the main thing to use) for creating & managing ontologies in the system
/// </summary>
 public class OntologyGenerator : MonoBehaviour {


	public Dictionary<string, string> AchievementsDictionary = new Dictionary<string, string>();
	public string GamecloudAddress = "http://";

	// Public values for displaying/editing the ontology stub
	public string Name = "";
	public Types Type;
	public string SubType = "";
	public string Description = "";
	public string ActionType = "";

	/// <summary>
	/// Creates the ontology.
	/// </summary>
	public void CreateOntology() 
	{
		// Do Stuff!
		Debug.Log("Create Button Pressed!");
		Debug.Log(CreateSessionId());
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



