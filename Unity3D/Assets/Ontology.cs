using UnityEngine;
using System.Collections;


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
 public class Ontology : MonoBehaviour {

	// Public values for displaying/editing the ontology stub
	public string Name = "";
	public Types Type;
	public string SubType = "";
	public string Description = "";

	/// <summary>
	/// Creates the ontology.
	/// </summary>
	public void CreateOntology() 
	{
		// Do Stuff!
		Debug.Log("Create Button Pressed!");
	}

 }
