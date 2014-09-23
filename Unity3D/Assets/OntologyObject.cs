using UnityEngine;
using System.Collections;

public class OntologyObject : MonoBehaviour {

	public string Name;
	public string GainHash;
	public string LoseHash;
	public string AskHash;
	public Types Type;



	public void DefineOntology(Types type, string name, string askHash, string gainHash, string loseHash)
	{
		this.Type = type;
		this.Name = name;
		this.AskHash = askHash;
		this.GainHash = gainHash;
		this.LoseHash = loseHash;
	}

	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}
}
